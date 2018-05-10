using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [Space, Header("Extra Variables")]
    public Image healthBar;
    public float healthLostSpeed;
    public Transform combatTextSpawn;
    public ObjectPooling combatText;
    public Collider collision;
    public NavMeshAgent agent;

    [Space]
    public SkinnedMeshRenderer rend;
    public Material dissolveMaterial;

    [Space]
    public MeshRenderer[] extraRenderers;
    public Material[] extraDissolveMaterials;

    [HideInInspector]
    public bool isDead;

    AnimatorBase animatorBase;
    float deathTime;
    float[] extraDeathTimes;
    Interactable interactable;

    private void Start()
    { 
        interactable = GetComponent<Interactable>();
        animatorBase = GetComponent<AnimatorBase>();

        combatText = ReferenceManager.floatingCombatTextPool;
    }

    void OnEnable()
    {
        health = baseHealth;
        agent.enabled = true;
        extraDeathTimes = new float[extraRenderers.Length];

        isDead = false;
        collision.enabled = true;
        healthBar.transform.parent.gameObject.SetActive(true);

        if(animatorBase)
            animatorBase.ResetDeath();

        gameObject.layer = LayerMask.NameToLayer("Interactable");

        rend.material.SetFloat("_Progress", 1);

        if (extraRenderers.Length > 0)
        {
            for (int i = 0; i < extraRenderers.Length; i++)
            {
                extraRenderers[i].material.SetFloat("_Progress", 1);
                extraDeathTimes[i] += .2f * Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        if (isDead)
        {
            rend.material.SetFloat("_Progress", Mathf.Lerp(1, 0, deathTime));
            deathTime += .2f * Time.deltaTime;

            if(extraRenderers.Length > 0)
            {
                for(int i = 0; i < extraRenderers.Length; i++)
                {
                    extraRenderers[i].material.SetFloat("_Progress", Mathf.Lerp(1, 0, deathTime));
                    extraDeathTimes[i] += .2f * Time.deltaTime;
                }
            }
        }
        else
            UpdateHealthBar();
    }

    public override void TookDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        GameObject obj = combatText.GetPooledObject();

        Text cbtText = obj.GetComponent<Text>();
        cbtText.text = damage.ToString();

        if (obj == null)
        {
            return;
        }

        obj.transform.parent = combatTextSpawn.transform;
        obj.transform.position = combatTextSpawn.transform.position;
        obj.transform.rotation = combatTextSpawn.transform.rotation;
        obj.SetActive(true);

        animatorBase.Hit(numberOfHits);

        AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
        source.PlayOneShot(hitSound);

        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Died());
        }
    }

    public override IEnumerator Died()
    {
        //if (rend && dissolveMaterial)
        //{
        //    Material[] mats = rend.materials;
        //    mats[0] = dissolveMaterial;
        //    rend.materials = mats;
        //}

        //if (extraRenderers.Length > 0)
        //{
        //    for (int i = 0; i < extraRenderers.Length; i++)
        //    {
        //        Material[] mats = extraRenderers[i].materials;
        //        mats[i] = extraDissolveMaterials[i];
        //        extraRenderers[i].materials = mats;
        //    }
        //}

        agent.enabled = false;
        collision.enabled = false;
        interactable.OnDefocused();
        healthBar.transform.parent.gameObject.SetActive(false);
        animatorBase.Death(numberOfDeaths);

        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        gameObject.layer = LayerMask.NameToLayer("Default");

        yield return new WaitForSeconds(despawnTime);
        gameObject.SetActive(false);
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / (float)baseHealth, Time.deltaTime * healthLostSpeed);
    }
}

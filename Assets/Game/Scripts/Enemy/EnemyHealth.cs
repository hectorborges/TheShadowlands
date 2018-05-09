using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [Space, Header("Extra Variables")]
    public Image healthBar;
    public float healthLostSpeed;

    [Space]
    public GameObject hitEffect;
    public Material dissolveMaterial;

    [HideInInspector]
    public bool isDead;

    SkinnedMeshRenderer rend;

    AnimatorBase animatorBase;
    float deathTime;

    void OnEnable()
    {
        animatorBase = GetComponent<AnimatorBase>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        health = baseHealth;
      //  UpdateHealthBar();
    }

    private void Update()
    {
        if (isDead)
        {
            rend.material.SetFloat("_Progress", Mathf.Lerp(1, 0, deathTime));
            deathTime += .2f * Time.deltaTime;
        }
        else
            UpdateHealthBar();
    }

    public override void TookDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
       // UpdateHealthBar();

        if (hitEffect)
            hitEffect.SetActive(true);

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
        healthBar.transform.parent.gameObject.SetActive(false);
        animatorBase.Death(numberOfDeaths);

        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        gameObject.layer = LayerMask.NameToLayer("Default");

        if (rend && dissolveMaterial)
        {
            Material[] mats = rend.materials;
            mats[0] = dissolveMaterial;
            rend.materials = mats;

        }
        yield return new WaitForSeconds(despawnTime);
        //Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / (float)baseHealth, Time.deltaTime * healthLostSpeed);
    }
}

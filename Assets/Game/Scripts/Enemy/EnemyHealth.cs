using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [Space, Header("Extra Variables")]
    public GameObject hitEffect;
    public Material dissolveMaterial;

    [HideInInspector]
    public bool isDead;

    SkinnedMeshRenderer rend;

    Animator anim;
    float deathTime;

    void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        health = baseHealth;
    }

    private void Update()
    {
        if (isDead)
        {
            rend.material.SetFloat("_Progress", Mathf.Lerp(1, 0, deathTime));
            deathTime += .2f * Time.deltaTime;
        }
    }

    public override void TookDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (hitEffect)
            hitEffect.SetActive(true);


        int randomHit = Random.Range(1, numberOfHits + 1);
        anim.SetInteger("Hit", randomHit);

        anim.SetTrigger("Hit");

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
        anim.SetBool("Died", true);

        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        if (gameObject.layer == LayerMask.NameToLayer("Targetable"))
            gameObject.layer = LayerMask.NameToLayer("Default");

        if (rend && dissolveMaterial)
        {
            Material[] mats = rend.materials;
            mats[0] = dissolveMaterial;
            rend.materials = mats;

        }
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}

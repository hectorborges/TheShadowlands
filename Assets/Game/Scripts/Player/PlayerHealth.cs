using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public Image healthBar;
    public float healthLostSpeed;
    public PlayerAnimator playerAnimator;
    public static PlayerHealth instance;
    public Perk[] onDamagedPerks;

    public Image deathBoarder;
    public float deathBoarderSpeed = .5f;
    public GameObject deathScreen;

    [HideInInspector]public float thornsDamagePercentage;
    bool thorns;
    float transparency;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        ResetCharacter();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public override void TookDamage(int damage, GameObject attackingTarget, bool crit)
    {
        playerAnimator.Hit(numberOfHits);
        base.TookDamage(damage, attackingTarget, crit);

        if(thorns)
        {
            float thornsDamage = baseHealth / thornsDamagePercentage;
            attackingTarget.GetComponent<Health>().TookDamage((int)thornsDamage, gameObject, crit);
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / (float)baseHealth, Time.deltaTime * healthLostSpeed);

        float healthPercentage = (float)health / (float)baseHealth;

        transparency =  Mathf.Lerp(transparency, Mathf.Abs(healthPercentage - 1), deathBoarderSpeed);
        Utility.SetTransparency(deathBoarder, (transparency * .5f));
        
    }

    public void SetThornsActive(bool status)
    {
        thorns = status;
    }

    public override IEnumerator Died()
    {
        PlayerMovement.canMove = false;
        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        playerAnimator.Dead(true);
        yield return new WaitForSeconds(despawnTime);
        deathScreen.SetActive(true);
    }

    public void ResetCharacter()
    {
        print("Reseting Character");
        deathScreen.SetActive(false);
        playerAnimator.Dead(false);
        health = (int)stats.GetStatBaseValue(Stat.StatType.Health);
    }

    public void Ressurected()
    {
        PlayerMovement.canMove = true;
        isDead = false;
    }
}
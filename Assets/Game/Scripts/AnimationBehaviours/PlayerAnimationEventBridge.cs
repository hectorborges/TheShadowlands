using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventBridge : MonoBehaviour
{
    public AbilityEffectAnimationEvent[] abilityEffectAnimationEvents;
    PlayerMovement playerMovement;

	void Start ()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
	}
	
    public void Step()
    {
        playerMovement.Step();
    }

    public void AbilityEffect(string _abilityName)
    {
        foreach(AbilityEffectAnimationEvent abilityEffect in abilityEffectAnimationEvents)
        {
            if(abilityEffect.abilityName == _abilityName)
            {
                print(abilityEffect.abilityName);
                abilityEffect.abilityEffect.SetActive(true);

                AudioClip randomEffectSound = abilityEffect.effectSounds[Random.Range(0, abilityEffect.effectSounds.Length)];
                abilityEffect.audioSource.PlayOneShot(randomEffectSound);

                if(abilityEffect.damageCollider)
                {
                    abilityEffect.damageCollider.EnableCollider();
                }

                if(abilityEffect.projectileAbility)
                {
                    abilityEffect.projectileAbility.Fire();
                }

                if(abilityEffect.secondaryEffectSounds.Length > 0)
                {
                    AudioClip randomSecondaryEffectSound = abilityEffect.secondaryEffectSounds[Random.Range(0, abilityEffect.secondaryEffectSounds.Length)];
                    abilityEffect.audioSource.PlayOneShot(randomSecondaryEffectSound);
                }

                StartCoroutine(DisableEffectAfter(abilityEffect.abilityEffect, abilityEffect.disableAfter));
                break;
            }
        }
    }

    IEnumerator DisableEffectAfter(GameObject effectToDisable, float disableAfter)
    {
        yield return new WaitForSeconds(disableAfter);
        effectToDisable.SetActive(false);
    }
}

[System.Serializable]
public class AbilityEffectAnimationEvent
{
    public string abilityName;
    public GameObject abilityEffect;
    public float disableAfter;

    public ProjectileAbility projectileAbility;
    public DisableColliderAfter damageCollider;
    public AudioSource audioSource;
    public AudioClip[] effectSounds;
    public AudioClip[] secondaryEffectSounds;
}

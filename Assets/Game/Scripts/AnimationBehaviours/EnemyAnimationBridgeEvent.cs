using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationBridgeEvent : MonoBehaviour
{
    public EffectAnimationEvent[] abilityEffectAnimationEvents;

    public void AbilityEffect(string _effectName)
    {
        foreach (EffectAnimationEvent abilityEffect in abilityEffectAnimationEvents)
        {
            if (abilityEffect.effectName == _effectName)
            {
                abilityEffect.effect.SetActive(true);
                StartCoroutine(DisableEffectAfter(abilityEffect.effect, abilityEffect.disableAfter));
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
public class EffectAnimationEvent
{
    public string effectName;
    public GameObject effect;
    public float disableAfter;
}

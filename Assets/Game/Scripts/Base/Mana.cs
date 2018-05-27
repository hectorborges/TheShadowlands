using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public int mana;
    public Image manaBar;
    public Stats stats;
    public float manaChangeSpeed = .5f;

    [Space]
    public SoundManager soundManager;
    public GameObject needMoreManaText;

    int currentMana;
    bool waiting;

    private void Start()
    {
        mana = (int)stats.GetStatBaseValue(Stat.StatType.Mana);
        currentMana = mana;
    }

    private void Update()
    {
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, (float)currentMana / (float)mana, Time.deltaTime * manaChangeSpeed);
    }

    public bool ActivateAbility(int manaRequired)                   //Called when using an ability and checks if you have enough mana to fire.
    {                                                               //If you do then it consumes the mana and tells the ability it's okay to fire.
        if (currentMana - manaRequired >= 0)
        {
            currentMana -= manaRequired;
            return true;
        }
        else
        {
            soundManager.NeedMana();
            if(!waiting)
            {
                waiting = true;
                needMoreManaText.SetActive(true);
                StartCoroutine(WaitTime());
            }
            return false;
        }
    }

    public void GainMana(int manaGained)
    {
        if(currentMana + manaGained <= mana)
        {
            currentMana += manaGained;
        }
        else
        {
            currentMana = mana;
        }
    }

    public virtual void UpdateBaseMana()
    {
        mana = (int)stats.GetStatBaseValue(Stat.StatType.Mana);
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(.75f);
        needMoreManaText.SetActive(false);
        waiting = false;
    }
}

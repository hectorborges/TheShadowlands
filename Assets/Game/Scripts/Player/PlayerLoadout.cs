using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadout : MonoBehaviour
{
    [Space, Header("Ability Keys")]
    public KeyCode primaryAbilityKey;
    public KeyCode secondaryAbilityKey;
    public KeyCode abilityOneKey;
    public KeyCode abilityTwoKey;
    public KeyCode abilityThreeKey;
    public KeyCode abilityFourKey;

    [Space, Header("Ability Loadout")]
    public List<Ability> abilities = new List<Ability>();
    public List<Image> abilitySlots;
    public List<Text> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();

    bool[] abilityOnCooldown = new bool[6];
    bool[] abilityActive = new bool[6];
    bool[] abilityDeactive = new bool[6];

    float remainingTime;
    Interactable focus;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        UpdateAbilities(-1);

        for (int i = 0; i < 6; i++)
            cooldownQueues.Add(new Queue<float>());
    }

    //pass in -1 if not changing abilities
    public void UpdateAbilities(int abilitySlotIndex)
    {
        if (abilitySlotIndex != -1)
        {
            abilities[abilitySlotIndex].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
            abilityCharges[abilitySlotIndex].text = abilities[abilitySlotIndex].charges.ToString();

            if (abilities[abilitySlotIndex].abilityCharges <= 1)
                abilityCharges[abilitySlotIndex].enabled = false;
            else
                abilityCharges[abilitySlotIndex].enabled = true;
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
            abilitySlots[i].sprite = abilities[i].abilityIcon;

            if (abilities[i].abilityCharges > 1)
            {
                abilityCharges[i].text = abilities[i].abilityCharges.ToString();
                abilityCharges[i].enabled = true;
            }
        }
    }

    private void Update()
    {
        RecieveInput();
        CheckCooldowns();
    }

    void AbilityInput()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] && abilities[i].CanShoot() && abilityActive[i])
            {
                abilities[i].ActivateAbility();
                abilityCharges[i].text = abilities[i].charges.ToString();

                if (abilities[i].charges >= 0)
                {
                    cooldownQueues[i].Enqueue(Time.time);
                }
            }
            else if (abilities[i] && abilityDeactive[i])
                abilities[i].DeactivateAbility();
        }
    }

    void UpdateAbiltyCharges(Ability ability)
    {
        if (abilities.Contains(ability))
        {
            int abilityIndex = abilities.IndexOf(ability);
            abilityCharges[abilityIndex].text = ability.charges.ToString();
        }
    }

    void CheckCooldowns()
    {
        if (cooldownQueues.Count <= 0) return;

        for (int j = 0; j < abilities.Count; j++)
        {
            Cooldown(j);
        }

        for (int i = 0; i < cooldownQueues.Count; i++)
        {
            if (cooldownQueues[i].Count > 0)
                remainingTime = ((cooldownQueues[i].Peek() + abilities[i].abilityCooldown) - Time.time) / abilities[i].abilityCooldown;

            if (remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    void Cooldown(int abilityIndex)
    {
        if (cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex].Peek() + abilities[abilityIndex].abilityCooldown) - Time.time) / abilities[abilityIndex].abilityCooldown;
            abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if (_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        }
    }

    private void RecieveInput()
    {
        abilityActive[0] = Attack(abilities[0], primaryAbilityKey);
        abilityActive[1] = Attack(abilities[1], secondaryAbilityKey);
        abilityActive[2] = Attack(abilities[2], abilityOneKey);
        abilityActive[3] = Attack(abilities[3], abilityTwoKey);
        abilityActive[4] = Attack(abilities[4], abilityThreeKey);
        abilityActive[5] = Attack(abilities[5], abilityFourKey);

        abilityDeactive[0] = Input.GetKeyUp(primaryAbilityKey);
        abilityDeactive[1] = Input.GetKeyUp(secondaryAbilityKey);
        abilityDeactive[2] = Input.GetKeyUp(abilityOneKey);
        abilityDeactive[3] = Input.GetKeyUp(abilityTwoKey);
        abilityDeactive[4] = Input.GetKeyUp(abilityThreeKey);
        abilityDeactive[5] = Input.GetKeyUp(abilityFourKey);

        AbilityInput();
    }

    public bool Attack(Ability ability, KeyCode abilityKey)
    {
        if(ability.requiresTarget)
        {
            if(focus != null)
            {
                if(Utility.CheckDistance(transform.position, focus.transform.position) < ability.attackDistance)
                {
                    if (ability.abilityInput == Ability.AbilityInput.GetButton)
                        return Input.GetKey(abilityKey);
                    else
                        return Input.GetKeyDown(abilityKey);
                }
            }
        }
        else
        {
            if (ability.abilityInput == Ability.AbilityInput.GetButton)
                return Input.GetKey(abilityKey);
            else
            {
                return Input.GetKeyDown(abilityKey);
            }
        }
        return false;
    }

    public void SetFocus(Interactable newFocus)
    {
        focus = newFocus;
    }
}

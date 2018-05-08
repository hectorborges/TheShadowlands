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
        AbilityInput();
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
        if (abilities[0].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[0] = Input.GetKey(primaryAbilityKey);
        else
            abilityActive[0] = Input.GetKeyDown(primaryAbilityKey);

        if (abilities[1].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[1] = Input.GetKey(secondaryAbilityKey);
        else
            abilityActive[1] = Input.GetKeyDown(secondaryAbilityKey);

        if (abilities[2].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[2] = Input.GetKey(abilityOneKey);
        else
            abilityActive[2] = Input.GetKeyDown(abilityOneKey);


        if (abilities[3].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[3] = Input.GetKey(abilityTwoKey);
        else
            abilityActive[3] = Input.GetKeyDown(abilityTwoKey);


        if (abilities[4].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[4] = Input.GetKey(abilityThreeKey);
        else
            abilityActive[4] = Input.GetKeyDown(abilityThreeKey);

        if (abilities[5].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[5] = Input.GetKey(abilityFourKey);
        else
            abilityActive[5] = Input.GetKeyDown(abilityFourKey);

        abilityDeactive[0] = Input.GetKeyUp(primaryAbilityKey);
        abilityDeactive[1] = Input.GetKeyUp(secondaryAbilityKey);
        abilityDeactive[2] = Input.GetKeyUp(abilityOneKey);
        abilityDeactive[3] = Input.GetKeyUp(abilityTwoKey);
        abilityDeactive[4] = Input.GetKeyUp(abilityThreeKey);
        abilityDeactive[5] = Input.GetKeyUp(abilityFourKey);
    }
}

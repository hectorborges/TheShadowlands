using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerLoadout : MonoBehaviour
{
    public static PlayerLoadout instance;

    [Space, Header("Ability Keys")]
    public KeyCode primaryAbilityKey;
    public KeyCode secondaryAbilityKey;
    public KeyCode abilityOneKey;
    public KeyCode abilityTwoKey;
    public KeyCode abilityThreeKey;
    public KeyCode abilityFourKey;

    [Space, Header("Weapons")]
    public Weapon currentWeapon;
    public GameObject weaponHolder;

    [Space, Header("Ability Loadout")]
    public List<Item> defaultItems;
    public List<Item> itemsInSlots;
    public List<Image> abilitySlots;
    public List<Text> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();

    bool[] abilityOnCooldown = new bool[6];
    bool[] abilityActive = new bool[6];
    bool[] abilityDeactive = new bool[6];

    float remainingTime;
    public static Interactable focus;
    PlayerAnimator playerAnimator;
    NavMeshAgent agent;
    Stats stats;

    bool attacking;
    Coroutine attack;

    Item equippedItem = new Item();

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        playerAnimator = GetComponent<PlayerAnimator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<Stats>();

        for (int i = 0; i < 6; i++)
            cooldownQueues.Add(new Queue<float>());
    }

    private void Start()
    {
        if(currentWeapon)
            ChangeWeapon(currentWeapon);
    }

    public void EquipItem(Item item, int abilitySlotIndex)
    {
        ChangeStats(item);
        UpdateItem(item, abilitySlotIndex);
    }

    void ChangeStats(Item item)
    {
        if (equippedItem != null)
        {
            for (int i = 0; i < equippedItem.itemStats.Count; i++)
            {
                stats.DecreaseStatCurrentValue(equippedItem.itemStats[i].statType, equippedItem.itemStats[i].GetCurrentValue());
            }
        }

        equippedItem = item;

        if(equippedItem != null)
        {
            for (int i = 0; i < equippedItem.itemStats.Count; i++)
            {
                stats.IncreaseStatCurrentValue(equippedItem.itemStats[i].statType, equippedItem.itemStats[i].GetCurrentValue());
            }
        }
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        ChangeWeaponLocation(currentWeapon.itemModel.transform, weaponHolder.transform);
        currentWeapon.itemModel.SetActive(false);

        if(currentWeapon.secondaryModel)
        {
            ChangeWeaponLocation(currentWeapon.secondaryModel.transform, weaponHolder.transform);
            currentWeapon.secondaryModel.SetActive(false);
        }

        currentWeapon = newWeapon;

        ChangeWeaponLocation(currentWeapon.itemModel.transform, currentWeapon.equipLocation.transform);
        currentWeapon.itemModel.SetActive(true);

        if (currentWeapon.secondaryModel)
        {
            ChangeWeaponLocation(currentWeapon.secondaryModel.transform, currentWeapon.secondaryEquipLocation.transform);
            currentWeapon.secondaryModel.SetActive(true);
        }

        playerAnimator.OverrideAnimations(currentWeapon.animatorOverrideController);
    }

    void ChangeWeaponLocation(Transform model, Transform location)
    {
        model.SetParent(location);
        model.localPosition = Vector3.zero;
        model.localRotation = Quaternion.identity;
    }

    public void ResetAttack()
    {
        if(attack != null)
            StopCoroutine(attack);

        attacking = false;
    }

    //pass in -1 if not changing abilities
    public void UpdateItem(Item item, int abilitySlotIndex)
    {
        itemsInSlots[abilitySlotIndex] = item;
        if (abilitySlotIndex != -1 && itemsInSlots[abilitySlotIndex] != null)
        {
            itemsInSlots[abilitySlotIndex].itemAbility.OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities

            print("Equipped Ability is " + itemsInSlots[abilitySlotIndex].itemAbility.abilityName);
            abilityCharges[abilitySlotIndex].text = itemsInSlots[abilitySlotIndex].itemAbility.abilityCharges.ToString();

            agent.stoppingDistance = itemsInSlots[abilitySlotIndex].itemAbility.abilityRange;

            if (itemsInSlots[abilitySlotIndex].itemAbility.abilityCharges <= 1)
                abilityCharges[abilitySlotIndex].enabled = false;
            else
                abilityCharges[abilitySlotIndex].enabled = true;
        }

        if (itemsInSlots[abilitySlotIndex] == null) return;
        for (int i = 0; i < itemsInSlots.Count; i++)
        {
            if(itemsInSlots[i] != null)
            {
                itemsInSlots[i].itemAbility.OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
                abilitySlots[i].sprite = itemsInSlots[i].itemAbility.abilityIcon;

                if (itemsInSlots[i].itemAbility.abilityCharges > 1)
                {
                    abilityCharges[i].text = itemsInSlots[i].itemAbility.abilityCharges.ToString();
                    abilityCharges[i].enabled = true;
                }
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
        if (itemsInSlots.Count <= 0) return;
        for (int i = 0; i < itemsInSlots.Count; i++)
        {
            if (itemsInSlots[i] && itemsInSlots[i].itemAbility.CanShoot() && abilityActive[i])
            {
                if(!attacking)
                {
                    attacking = true;
                    ChangeWeapon(itemsInSlots[i].itemAbility.abilityWeapon);

                    itemsInSlots[i].itemAbility.ActivateAbility();
                    abilityCharges[i].text = itemsInSlots[i].itemAbility.abilityCharges.ToString();

                    if (itemsInSlots[i].itemAbility.abilityCharges >= 0)
                    {
                        cooldownQueues[i].Enqueue(Time.time);
                    }
                    attack = StartCoroutine(Attacking());
                }
            }
            else if (itemsInSlots[i] && abilityDeactive[i])
                itemsInSlots[i].itemAbility.DeactivateAbility();
        }
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    void UpdateAbiltyCharges(Item item)
    {
        if (itemsInSlots.Contains(item))
        {
            int abilityIndex = itemsInSlots.IndexOf(item);
            abilityCharges[abilityIndex].text = item.itemAbility.abilityCharges.ToString();
        }
    }

    void CheckCooldowns()
    {
        if (cooldownQueues.Count <= 0) return;

        for (int j = 0; j < itemsInSlots.Count; j++)
        {
            Cooldown(j);
        }

        for (int i = 0; i < cooldownQueues.Count; i++)
        {
            if (cooldownQueues[i].Count > 0 && cooldownQueues[i].ToArray().Length > 0)
                remainingTime = ((cooldownQueues[i].Peek() + itemsInSlots[i].itemAbility.abilityCooldown) - Time.time) / itemsInSlots[i].itemAbility.abilityCooldown;

            if (remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    void Cooldown(int abilityIndex)
    {
        if (cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex].Peek() + itemsInSlots[abilityIndex].itemAbility.abilityCooldown) - Time.time) / itemsInSlots[abilityIndex].itemAbility.abilityCooldown;

            if (!itemsInSlots[abilityIndex].itemAbility.CheckCooldown())
                _remainingTime = 0;

                abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if (_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        }
    }

    public void ResetCooldown(Item item)
    {
        int index = itemsInSlots.IndexOf(item);

        if(cooldownQueues[index].Count > 0 && cooldownQueues[index].ToArray().Length > 0)
            cooldownQueues[index].Dequeue();

        abilityCooldownProgress[index].fillAmount = 0f;
    }

    private void RecieveInput()
    {
        if (itemsInSlots.Count <= 0) return;
        if (itemsInSlots[0] != null)
            abilityActive[0] = Attack(itemsInSlots[0], primaryAbilityKey);
        if (itemsInSlots[1] != null)
            abilityActive[1] = Attack(itemsInSlots[1], secondaryAbilityKey);
        if (itemsInSlots[2] != null)
            abilityActive[2] = Attack(itemsInSlots[2], abilityOneKey);
        if (itemsInSlots[3] != null)
            abilityActive[3] = Attack(itemsInSlots[3], abilityTwoKey);
        if (itemsInSlots[4] != null)
            abilityActive[4] = Attack(itemsInSlots[4], abilityThreeKey);
        if (itemsInSlots[5] != null)
            abilityActive[5] = Attack(itemsInSlots[5], abilityFourKey);

        abilityDeactive[0] = Input.GetKeyUp(primaryAbilityKey);
        abilityDeactive[1] = Input.GetKeyUp(secondaryAbilityKey);
        abilityDeactive[2] = Input.GetKeyUp(abilityOneKey);
        abilityDeactive[3] = Input.GetKeyUp(abilityTwoKey);
        abilityDeactive[4] = Input.GetKeyUp(abilityThreeKey);
        abilityDeactive[5] = Input.GetKeyUp(abilityFourKey);

        AbilityInput();
    }

    public bool Attack(Item item, KeyCode abilityKey)
    {
        if(item.itemAbility.requiresTarget)
        {
            if(focus != null)
            {
                if(Utility.CheckDistance(transform.position, focus.transform.position) < item.itemAbility.abilityRange)
                {
                    if (item.itemAbility.abilityInput == Ability.AbilityInput.GetButton)
                        return Input.GetKey(abilityKey);
                    else
                        return Input.GetKeyDown(abilityKey);
                }
            }
        }
        else
        {
            if (item.itemAbility.abilityInput == Ability.AbilityInput.GetButton)
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

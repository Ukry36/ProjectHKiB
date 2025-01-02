using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EquipmentManager : MonoBehaviour
{

    #region Singleton
    public static EquipmentManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            theInven = FindObjectOfType<InventoryManager>();
            theDB = FindObjectOfType<DBManager>();
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private InventoryManager theInven;
    private DBManager theDB;

    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image displayImg;

    private const int SLOT1 = 0, SLOT2 = 1, SLOT3 = 2, SLOT4 = 3;
    private int currentSlot = 0;

    [SerializeField] private Image[] imgSlots;
    [SerializeField] private Image[] gradeSlots;
    [SerializeField] private Image[] transitions;

    [HideInInspector] public Item[] EquippedEffects;
    [HideInInspector] public int[] RealEquippedEffectIDs;

    private Color RED = new(1f, 0f, 0f, 1f),
        YELLOW = new(1f, 0.6f, 0, 1f),
        PURPLE = new(0.5f, 0, 1f, 1f),
        BLUE = new(0, 0.55f, 1f, 1f),
        GREEN = new(0.5f, 0.9f, 0, 1f),
        BLACK = new(0.3f, 0.3f, 0.3f, 1f),
        NONE = new(0, 0, 0, 0),
        PINK = new(1f, 0.32f, 0.48f, 1f),
        DARKBLUE = new(0.32f, 0.48f, 1f, 1f),
        MINT = new(0.65f, 1f, 0.8f, 1f),
        WHITE = new(1f, 1f, 1f, 1f);


    private void Start()
    {
        EquippedEffects = new Item[] { new(), new(), new(), new() };
        RealEquippedEffectIDs = new int[] { 0, 0, 0, 0 };
    }


    // make every imgs transparent
    private void InitSlots()
    {
        Color color = imgSlots[0].color;
        color.a = 0;

        for (int i = 0; i < imgSlots.Length; i++)
        {
            imgSlots[i].sprite = null;
            imgSlots[i].color = color;
        }
    }


    // show equipped items and transition lines
    public void ShowEquip()
    {
        // make every imgs transparent and show only existing sprites
        InitSlots();
        Color color = imgSlots[0].color;
        color.a = 1;
        for (int i = 0; i < imgSlots.Length; i++)
        {
            if (EquippedEffects[i].ID != 0)
            {
                imgSlots[i].sprite = EquippedEffects[i].Icon;
                imgSlots[i].color = color;
            }
        }


        #region Show Transition and Set Grade
        // initialize transition lines
        for (int i = 0; i < transitions.Length; i++)
        {
            transitions[i].color = NONE;
            transitions[i].gameObject.SetActive(false);
        }


        // booleans for drawing closed lines
        bool SLOT12 = false;
        bool SLOT13 = false;
        bool SLOT14 = false;


        // Equipped == Grade
        if (RealEquippedEffectIDs[SLOT1] != 0)
        {
            SetItemGrade(SLOT1, 3);
            if (RealEquippedEffectIDs[SLOT1] == RealEquippedEffectIDs[SLOT2])
            {
                transitions[0].gameObject.SetActive(true);
                transitions[0].color = PINK;
                SLOT12 = true;
                SetItemGrade(SLOT2, 3);
            }
            if (RealEquippedEffectIDs[SLOT1] == RealEquippedEffectIDs[SLOT3])
            {
                transitions[4].gameObject.SetActive(true);
                transitions[4].color = PINK;
                SLOT13 = true;
                SetItemGrade(SLOT3, 3);
            }
            if (RealEquippedEffectIDs[SLOT1] == RealEquippedEffectIDs[SLOT4])
            {
                transitions[3].gameObject.SetActive(true);
                transitions[3].color = PINK;
                SLOT14 = true;
                SetItemGrade(SLOT4, 3);
            }
        }
        if (RealEquippedEffectIDs[SLOT2] != 0)
        {
            if (RealEquippedEffectIDs[SLOT2] == RealEquippedEffectIDs[SLOT3])
            {
                transitions[1].gameObject.SetActive(true);
                if (SLOT12 && SLOT13)
                    transitions[1].color = PINK;
                else
                    transitions[1].color = DARKBLUE;
            }
            if (RealEquippedEffectIDs[SLOT2] == RealEquippedEffectIDs[SLOT4])
            {
                transitions[5].gameObject.SetActive(true);
                if (SLOT12 && SLOT14)
                    transitions[5].color = PINK;
                else
                    transitions[5].color = DARKBLUE;
            }
        }
        if (RealEquippedEffectIDs[SLOT3] != 0 && RealEquippedEffectIDs[SLOT3] == RealEquippedEffectIDs[SLOT4])
        {
            transitions[2].gameObject.SetActive(true);
            if (SLOT13 && SLOT14)
                transitions[2].color = PINK;
            else
                transitions[2].color = DARKBLUE;
        }
        #endregion


        #region Show ItemGrade
        // color item slots
        for (int i = 0; i < EquippedEffects.Length; i++)
            gradeSlots[i].color = EquippedEffects[i].Equipped switch
            {
                3 => PINK,
                2 => DARKBLUE,
                1 => MINT,
                _ => WHITE,
            };
        #endregion
    }


    // if there is pair of effects which can transit into synthesized effects, 
    // change realEqdEffIDs' transformed effect's ID into synthesized one
    public void SetTransition()
    {
        RealEquippedEffectIDs =
        new int[] {EquippedEffects[SLOT1].ID, EquippedEffects[SLOT2].ID,
                   EquippedEffects[SLOT3].ID, EquippedEffects[SLOT4].ID};

        for (int i = 0; i < theDB.TransitedEffectList.Length; i++) // in all transited effects,
        {
            int[] transitionPath = theDB.TransitedEffectList[i].TransitionPath;

            // if all IDs in transition path exist in RealEquippedEffectIDs (varies through this squence)
            bool exists = true;
            for (int j = 0; j < transitionPath.Length; j++)
            {
                if (!Array.Exists(RealEquippedEffectIDs, ID => ID == transitionPath[j]))
                    exists = false;
            }

            // convert those effects' ID (in real eq eff) into transited effect's ID
            if (exists)
            {
                for (int j = 0; j < transitionPath.Length; j++)
                {
                    for (int k = 0; k < RealEquippedEffectIDs.Length; k++)
                    {
                        if (RealEquippedEffectIDs[k] == transitionPath[j])
                        {
                            RealEquippedEffectIDs[k] = theDB.TransitedEffectList[i].ID;
                            SetItemGrade(k, 2);
                        }
                    }
                }
            }
        }
        // it means that faster transited effect in DBM has higher priority
    }



    // change every equipped item's grade(item.Equipped) into 1
    private void DefaultItemGrade()
    {
        for (int i = 0; i < EquippedEffects.Length; i++)
        {
            if (EquippedEffects[i].ID != 0)
                SetItemGrade(i, 1);
        }
    }


    // change one item's grade in Equipment UI and Inventory UI
    private void SetItemGrade(int _slot, int _grade)
    {
        EquippedEffects[_slot].Equipped = _grade;
        theInven.InventoryItemList.Find(item => item.ID == EquippedEffects[_slot].ID).Equipped = _grade;
    }


    // from MenuManager
    // show item's info
    public void SelectSlot(int _selectedSlot)
    {
        Item item = theDB.itemList.Find(item => item.ID == RealEquippedEffectIDs[_selectedSlot]);
        description.text = item.Name + "\n" + item.Description;
        displayImg.sprite = item.Icon;
    }


    // from MenuManager
    // if there is no item, move to inventory for equip sequence, 
    // if there is item, unequip
    public void ClickSlot(int _selectedSlot)
    {
        currentSlot = _selectedSlot;
        Item item = theDB.itemList.Find(item => item.ID == RealEquippedEffectIDs[_selectedSlot]);

        if (EquippedEffects[_selectedSlot].ID == 0)
            EquipEffect();
        else
            UnequipEffect();
    }


    // get into equipsequene
    private void EquipEffect()
    {
        MenuManager.instance.OpenEquipSequenceInventory();
    }


    // last of equipsequence
    // equip desired item, rearrange transition lines and grade slots
    public void AfterEquipSequence(Item item)
    {
        EquippedEffects[currentSlot] = item;
        DefaultItemGrade();
        SetTransition();
        ShowEquip();
    }


    // unequip desired item, rearrange transition lines and grade slots
    private void UnequipEffect()
    {
        SetItemGrade(currentSlot, 0);
        EquippedEffects[currentSlot] = new Item();
        DefaultItemGrade();
        SetTransition();
        ShowEquip();
    }


    // trriger to change player's form
    public void ApplyEffect()
    {
        PlayerManager.instance.ActivateEquippedEffect(RealEquippedEffectIDs);
    }
}

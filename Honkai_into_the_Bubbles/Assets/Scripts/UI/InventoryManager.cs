using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private DBManager theDB;

    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image selectedImage;
    [SerializeField] private string[] tabDiscription;
    [SerializeField] private Sprite[] selectedTabSprites;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Transform ItemGridLayout;
    public GameObject TabGridLayout;
    
    [SerializeField] private GameObject nextPageButton, prevPageButton;

    [HideInInspector] public List<Item> InventoryItemList;
    private List<Item> TabItemList;
    private ItemSlot[] slots;
    private int SelectedTab;
    private int page;
    private int maxPage;
    private int slotCount;
    private const int MAXSLOT = 8;

    [HideInInspector] public bool equipSequence;

    private void Awake()
    {
        // singleton
        if (instance == null)
            instance = this;

        theDB = FindObjectOfType<DBManager>();
    }
    void Start()
    {
        InventoryItemList = new List<Item>();
        TabItemList = new List<Item>();
        slots = ItemGridLayout.GetComponentsInChildren<ItemSlot>();
        InitSlots();
        nextPageButton.SetActive(false);
        prevPageButton.SetActive(false);
        pageText.gameObject.SetActive(false);
    }

    public void InitSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
        nextPageButton.SetActive(false);
        prevPageButton.SetActive(false);
        pageText.gameObject.SetActive(false);
    }

    public void SelectTab(int _selectedTab)
    {
        SelectedTab = _selectedTab;
        TabItemList.Clear();

        switch(_selectedTab)
        {
            case 0:
                for(int i = 0; i < InventoryItemList.Count; i++)
                {
                    if(Item.ItemType.Effect == InventoryItemList[i].Type)
                        TabItemList.Add(InventoryItemList[i]);
                }   
                break;
            case 1:
                for (int i = 0; i < InventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == InventoryItemList[i].Type)
                        TabItemList.Add(InventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < InventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Cloth == InventoryItemList[i].Type)
                        TabItemList.Add(InventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < InventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == InventoryItemList[i].Type)
                        TabItemList.Add(InventoryItemList[i]);
                }
                break;
        } 

        description.text = tabDiscription[SelectedTab];
        selectedImage.sprite = selectedTabSprites[SelectedTab];

        TabItemList = SortItems(TabItemList);

        maxPage = TabItemList.Count / MAXSLOT;

        page = 0;
        ShowPage();
    }

    public void SelectItem(int _selectedItem)
    {
        description.text = TabItemList[_selectedItem + page * MAXSLOT].Name + "\n" + TabItemList[_selectedItem + page * MAXSLOT].Description;
        selectedImage.sprite = TabItemList[_selectedItem + page * MAXSLOT].Icon;
    }
    public void ClickItem(int _selectedItem)
    {
        if (!equipSequence)
        {
            //dosmth
        }
        else
        {
            EquipmentManager.instance.AfterEquipSequence(TabItemList[_selectedItem + page * MAXSLOT]);
            MenuManager.instance.CloseEquipSequenceInventory();
        }
    }
    public void DeselectItem(int _selectedItem)
    {
        description.text = tabDiscription[SelectedTab];
        selectedImage.sprite = selectedTabSprites[SelectedTab];
    }


#region Sort
    public List<Item> SortItems(List<Item> list)
    {
        return list.OrderByDescending(item => item.Rare).ThenBy(item => item.Name).ToList();
    }

    public List<Item> SortItemsInEquip(List<Item> list)
    {
        return list.OrderByDescending(item => item.Equipped).ThenByDescending(item => item.Rare).ThenBy(item => item.Name).ToList();
    }
#endregion

    
    public void ShowPage()
    {
        InitSlots();

        if (TabItemList.Count > MAXSLOT)
        {
            nextPageButton.SetActive(true);
            prevPageButton.SetActive(true);
            pageText.gameObject.SetActive(true);
        }
        
        slotCount = -1;
        for (int i = page * MAXSLOT; i < TabItemList.Count; i++)
        {
            slotCount = i - (page * MAXSLOT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].AddItem(TabItemList[i]);

            if (slotCount == MAXSLOT - 1)
                break;
        }
        pageText.text = (page + 1).ToString();
    }


    public void NextPage()
    {
        page++;
        if (page > maxPage)
            page = 0;
        ShowPage();
    }
    public void PrevPage()
    {
        page--;
        if (page < 0)
            page = maxPage;
        ShowPage();
    }

    
    public void GetItem(int _ID, int _count = 1)
    {
        for(int i = 0; i < theDB.itemList.Count; i++) 
        {
            if (_ID == theDB.itemList[i].ID) 
            {
                for(int j = 0; j < InventoryItemList.Count; j++) 
                {
                    if (_ID == InventoryItemList[j].ID) 
                    {
                        InventoryItemList[j].Count += _count;
                        return;
                    }
                }
                InventoryItemList.Add(theDB.itemList[i]); 
                InventoryItemList[InventoryItemList.Count - 1].Count = _count;
                return;
            }
        }
        Debug.LogError("ERROR: ItemID not found!");
    }
}
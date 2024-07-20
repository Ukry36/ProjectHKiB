using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            theInput = FindObjectOfType<InputManager>();
            theEquip = FindObjectOfType<EquipmentManager>();
            theInven = FindObjectOfType<InventoryManager>();
            thePlayer = FindObjectsOfType<Playable>(true);
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion


    private InputManager theInput;
    private EquipmentManager theEquip;
    private InventoryManager theInven;
    private Playable[] thePlayer;
    [SerializeField] private Status theStat;

    [SerializeField] private GameObject _pauseCanvasGO;
    [SerializeField] private GameObject _inventoryCanvasGO;
    [SerializeField] private GameObject _equipmentCanvasGO;
    [SerializeField] private GameObject _playCanvasGO;
    [SerializeField] private GameObject _equipSlotFirst;
    [SerializeField] private GameObject _inventoryFirst;
    [SerializeField] private GameObject _itemListFirst;

    private bool isPaused;
    private bool inEquipment;
    private bool inEquipSequenceInventory;
    private bool inInventory;
    private bool inItemList;

    [SerializeField] private UnityEngine.UI.Image HPbar;
    [SerializeField] private TextMeshProUGUI HPtext;


    private void Start()
    {
        _pauseCanvasGO.SetActive(false);
        _inventoryCanvasGO.SetActive(false);
        _equipmentCanvasGO.SetActive(false);
    }

    private void Update()
    {
        HPtext.text = theStat.currentHP + "/" + theStat.maxHP;
        HPbar.transform.localScale = new Vector3((float)theStat.currentHP / theStat.maxHP, 1, 0);
        if (isPaused)
        {
            if (theInput.CancelInput)
                Resume();
        }
        else if (inEquipment)
        {
            if (theInput.CancelInput || theInput.EquipmentOpenCloseInput)
                CloseEquipment();
        }
        else if (inEquipSequenceInventory)
        {
            if (theInput.CancelInput)
                CloseEquipSequenceInventory();
            if (theInput.EquipmentOpenCloseInput)
            {
                CloseEquipSequenceInventory();
                CloseEquipment();
            }
        }
        else if (inInventory)
        {
            if (theInput.CancelInput || theInput.InventoryOpenCloseInput)
                CloseInventory();
        }
        else if (inItemList)
        {
            if (theInput.CancelInput)
                CloseItemList();
            else if (theInput.InventoryOpenCloseInput)
            {
                CloseItemList();
                CloseInventory();
            }
        }
        else
        {
            if (theInput.CancelInput)
                Pause();
            else if (theInput.EquipmentOpenCloseInput)
                OpenEquipment();
            else if (theInput.InventoryOpenCloseInput)
                OpenInventory();
        }
    }

    private void StopPlayerforUI()
    {
        for (int i = 0; i < thePlayer.Length; i++)
            thePlayer[i].savedInput = Vector3.zero;
        theInput.StopPlayerInput(true);
    }

    private void ResumePlayerforUI()
    {
        theInput.StopPlayerInput(false);
    }

    // pause
    #region Pause / Resume Functions

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        _pauseCanvasGO.SetActive(true);
        theInput.stopPlayer = true;
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _pauseCanvasGO.SetActive(false);
        theInput.stopPlayer = false;
    }

    #endregion



    // equipment
    #region Open / Close Equipment Functions
    private void OpenEquipment()
    {
        inEquipment = true;
        _equipmentCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_equipSlotFirst);
        StopPlayerforUI();
    }

    private void CloseEquipment()
    {
        inEquipment = false;
        _equipmentCanvasGO.SetActive(false);
        ResumePlayerforUI();

        theEquip.ApplyEffect();
    }
    #endregion

    #region Select / Click Slot Functions

    public void SelectSlotAction(int _selectedSlot)
    {
        theEquip.SelectSlot(_selectedSlot);
    }

    public void ClickSlotAction(int _selectedSlot)
    {
        theEquip.ClickSlot(_selectedSlot);
    }

    #endregion

    #region Open / Close Equip Sequence Inventory Functions

    public void OpenEquipSequenceInventory()
    {
        inEquipSequenceInventory = true;
        inEquipment = false;
        _inventoryCanvasGO.SetActive(true);
        theInven.SelectTab(0);
        EventSystem.current.SetSelectedGameObject(_itemListFirst);
        theInven.TabGridLayout.SetActive(false);
        theInven.equipSequence = true;
    }

    public void CloseEquipSequenceInventory()
    {
        inEquipSequenceInventory = false;
        inEquipment = true;
        theInven.InitSlots();
        _inventoryCanvasGO.SetActive(false);
        theInven.TabGridLayout.SetActive(true);
        theInven.equipSequence = false;
        EventSystem.current.SetSelectedGameObject(_equipSlotFirst);
    }
    #endregion



    // inventory
    #region Open / Close Inventory Functions

    private void OpenInventory()
    {
        inInventory = true;
        _inventoryCanvasGO.SetActive(true);
        StopPlayerforUI();
        EventSystem.current.SetSelectedGameObject(_inventoryFirst);
    }

    private void CloseInventory()
    {
        inInventory = false;
        _inventoryCanvasGO.SetActive(false);
        ResumePlayerforUI();
    }
    #endregion

    #region Open / Close ItemList Functions

    public void OpenItemList(int _SelectedTab)
    {
        inItemList = true;
        inInventory = false;
        theInven.SelectTab(_SelectedTab);
        EventSystem.current.SetSelectedGameObject(_itemListFirst);
    }

    private void CloseItemList()
    {
        inItemList = false;
        OpenInventory();
        theInven.InitSlots();
    }

    #endregion

    #region Select / Click / Deselect Item Function

    public void SelectItemAction(int _selectedItem)
    {
        theInven.SelectItem(_selectedItem);
    }

    public void ClickItemAction(int _selectedItem)
    {
        theInven.ClickItem(_selectedItem);
    }

    public void DeselectItemAction(int _selectedItem)
    {
        theInven.DeselectItem(_selectedItem);
    }

    #endregion

    #region Inventory Prev / Next Page Functions

    public void ToNextPage()
    {
        theInven.NextPage();
        EventSystem.current.SetSelectedGameObject(_itemListFirst);
    }

    public void ToPrevPage()
    {
        theInven.PrevPage();
        EventSystem.current.SetSelectedGameObject(_itemListFirst);
    }

    #endregion
}

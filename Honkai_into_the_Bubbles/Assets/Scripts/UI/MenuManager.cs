using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [SerializeField] private Image fadeImage;
    private Color disablePlayUIColor;

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

    [SerializeField] private Image HPbar;
    [SerializeField] private TextMeshProUGUI HPtext;
    [SerializeField] private TextMeshProUGUI GSTtext;
    [SerializeField] private TextMeshProUGUI FPSTtext;
    float deltaTime;

    public AudioMixer MasterMixer;
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Slider AMBSlider;

    public GameObject GPUI;
    private bool disablePlayUI = false;
    private bool disablePostProcessing = true;

    private void Start()
    {
        _pauseCanvasGO.SetActive(false);
        _inventoryCanvasGO.SetActive(false);
        _equipmentCanvasGO.SetActive(false);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        FPSTtext.text = "fps: " + 1.0f / deltaTime;
        HPtext.text = theStat.CurrentHP + "/" + theStat.maxHP;
        HPbar.transform.localScale = new Vector3((float)theStat.CurrentHP / theStat.maxHP, 1, 0);
        if (isPaused)
        {
            if (theInput.PauseInput)
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
            if (theInput.PauseInput)
                Pause();
            else if (theInput.EquipmentOpenCloseInput)
                OpenEquipment();
            else if (theInput.InventoryOpenCloseInput)
                OpenInventory();
        }
    }

    private void StopPlayerforUI()
    {
        theInput.StopPlayerInput(true);
    }

    private void ResumePlayerforUI()
    {
        theInput.StopPlayerInput(false);
    }

    #region Fadein / Fadeout

    public void SetFadeColor(Color _color)
    {
        _color.a = fadeImage.color.a;
        disablePlayUIColor = _color;
    }

    public IEnumerator FadeCoroutine(float _opacity, float _fadeTime)
    {
        StopCoroutine(nameof(FadeOutCoroutine));
        StopCoroutine(nameof(FadeInCoroutine));
        if (disablePlayUIColor.a < _opacity)
            yield return FadeOutCoroutine(_opacity, _fadeTime);
        else
            yield return FadeInCoroutine(_opacity, _fadeTime);
    }
    private IEnumerator FadeOutCoroutine(float _opacity, float _fadeTime)
    {
        float timer = _fadeTime;
        float speed = _opacity / _fadeTime;
        while (timer > 0 && disablePlayUIColor.a < _opacity)
        {
            disablePlayUIColor.a += speed * Time.unscaledDeltaTime;
            fadeImage.color = disablePlayUIColor;
            timer -= Time.deltaTime;
            yield return null;
        }
        disablePlayUIColor.a = _opacity;
        fadeImage.color = disablePlayUIColor;
        if (timer > 0) yield return new WaitForSeconds(timer);
    }
    private IEnumerator FadeInCoroutine(float _opacity, float _fadeTime)
    {
        float timer = _fadeTime;
        float speed = (1 - _opacity) / _fadeTime;
        while (timer > 0 && disablePlayUIColor.a > _opacity)
        {
            disablePlayUIColor.a -= speed * Time.unscaledDeltaTime;
            fadeImage.color = disablePlayUIColor;
            timer -= Time.deltaTime;
            yield return null;
        }
        disablePlayUIColor.a = _opacity;
        fadeImage.color = disablePlayUIColor;
        if (timer > 0) yield return new WaitForSeconds(timer);
    }

    #endregion

    // play
    #region Graffiti Countdown

    public void GraffitiCountDownEnable(float _time)
    {
        GSTtext.gameObject.SetActive(true);
        StartCoroutine(GraffitiCountDown(_time));
    }

    private IEnumerator GraffitiCountDown(float _time)
    {
        while (_time > 0)
        {
            GSTtext.text = (Mathf.Floor(_time * 10000f) / 10000f).ToString();
            yield return null;
            _time = _time < 0 ? 0 : _time - Time.unscaledDeltaTime;
        }
        GraffitiCountDownDisable();
    }

    public void GraffitiCountDownDisable()
    {
        GSTtext.gameObject.SetActive(false);
        StopCoroutine(nameof(GraffitiCountDown));
    }

    #endregion

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

    public void AudioControl(string _type)
    {
        float volume = _type switch
        {
            "Master" => MasterSlider.value,
            "BGM" => BGMSlider.value,
            "SFX" => SFXSlider.value,
            "AMB" => AMBSlider.value,
            _ => MasterSlider.value
        };

        MasterMixer.SetFloat(_type, volume <= -40f ? -80 : volume);

    }

    public void PlayUIToggle()
    {
        disablePlayUI = !disablePlayUI;
        if (disablePlayUI)
        {
            _playCanvasGO.SetActive(false);
            GPUI.SetActive(false);
        }
        else
        {
            _playCanvasGO.SetActive(true);
            GPUI.SetActive(true);
        }
    }

    public void PostProcessingToggle()
    {
        disablePostProcessing = !disablePostProcessing;

        CameraManager.instance.TogglePostProcessing(disablePostProcessing);

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

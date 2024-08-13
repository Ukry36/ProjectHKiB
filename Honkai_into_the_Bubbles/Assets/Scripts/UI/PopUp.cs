using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    private AudioManager theAudio;
    public string keySound;
    public string enterSound;
    public string cancelSound;
    public string popUpSound;

    public Image NoPanel;
    public Image YesPanel;

    public TextMeshProUGUI description;

    public bool activated;
    private bool keyInput;
    private bool result;

    private Color selected = new(1, 1, 1, 1);
    private Color deselected = new(1, 1, 1, 0.6f);
    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowPopUp(string _description)
    {
        activated = true;
        result = true;
        Select();
        description.text = _description;
        StartCoroutine(KeyIn());
    }

    public bool GetResult()
    {
        return result;
    }

    IEnumerator KeyIn()
    {
        yield return new WaitForSeconds(0.012f);
        keyInput = true;
    }

    public void Select()
    {
        if (result)
        {
            NoPanel.color = deselected;
            YesPanel.color = selected;
        }
        else
        {
            YesPanel.color = deselected;
            NoPanel.color = selected;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                result = true;
                Select();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                result = false;
                Select();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                keyInput = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                result = false;
                keyInput = false;
                activated = false;
            }
        }
    }
}

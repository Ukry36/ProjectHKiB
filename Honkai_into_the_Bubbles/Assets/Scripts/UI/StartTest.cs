using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartTest : MonoBehaviour
{
    public static StartTest instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        PlayEvent();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayEvent()
    {
        MenuManager.instance.SetFadeColor(Color.black);
        StartCoroutine(MenuManager.instance.FadeCoroutine(1, 0));

        InputManager.instance.StopPlayerInput(true);

        MenuManager.instance.Normaltext.text = tmp.text;
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        yield return new WaitUntil(() => InputManager.instance.UIConfirmInput);
        yield return MenuManager.instance.FadeCoroutine(0, 0);

        InputManager.instance.StopPlayerInput(false);

        MenuManager.instance.Normaltext.text = "";
    }
}

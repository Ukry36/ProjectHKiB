using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    #region Singleton
    static public ChoiceManager instance;
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
    #endregion Singleton

    private AudioManager theAudio;

    private string Q;
    private List<string> AList;

    public GameObject GO;

    public TextMeshProUGUI QT;
    public TextMeshProUGUI[] AT;

    public GameObject[] A_Panel;

    public Animator anim;

    public string keySound;
    public string enterSound;

    public bool choicing;
    public bool keyInput;

    private int count;

    private int result;

    private WaitForSeconds waitTime = new(0.0012f);
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        AList = new List<string>();
        for (int i = 0; i < AT.Length; i++)
        {
            AT[i].text = "";
            A_Panel[i].SetActive(false);
        }
        QT.text = "";

    }

    public void ShowChoice(Choice _choice)
    {
        GO.SetActive(true);
        result = 0;
        Q = _choice.Q;
        Selection();
        choicing = true;

        for (int i = 0; i < _choice.A.Length; i++)
        {
            AList.Add(_choice.A[i]);
            A_Panel[i].SetActive(true);
            count = i;
        }

        anim.SetBool("appear", true);
        StartCoroutine(ChoiceCoroutine());
    }

    public void ExitChoice()
    {
        for (int i = 0; i < count + 1; i++)
        {
            AT[i].text = "";
            A_Panel[i].SetActive(false);
        }
        QT.text = "";
        anim.SetBool("appear", false);
        AList.Clear();
        choicing = false;
    }

    public int GetResult()
    {
        GO.SetActive(false);
        return result;
    }

    IEnumerator ChoiceCoroutine()
    {

        StartCoroutine(QCoroutine());
        StartCoroutine(A0Coroutine());
        if (count >= 1)
            StartCoroutine(A1Coroutine());
        if (count >= 2)
            StartCoroutine(A2Coroutine());
        if (count >= 3)
            StartCoroutine(A3Coroutine());
        if (count >= 4)
            StartCoroutine(A3Coroutine());

        yield return new WaitForSeconds(0.5f);
        keyInput = true;
    }

    IEnumerator QCoroutine()
    {
        for (int i = 0; i < Q.Length; i++)
        {
            QT.text += Q[i];
            yield return waitTime;
        }
    }

    IEnumerator A0Coroutine()
    {
        for (int i = 0; i < AList[0].Length; i++)
        {
            AT[0].text += AList[0][i];
            yield return waitTime;
        }
    }

    IEnumerator A1Coroutine()
    {
        for (int i = 0; i < AList[1].Length; i++)
        {
            AT[1].text += AList[1][i];
            yield return waitTime;
        }
    }

    IEnumerator A2Coroutine()
    {
        for (int i = 0; i < AList[2].Length; i++)
        {
            AT[2].text += AList[2][i];
            yield return waitTime;
        }
    }

    IEnumerator A3Coroutine()
    {
        for (int i = 0; i < AList[3].Length; i++)
        {
            AT[3].text += AList[3][i];
            yield return waitTime;
        }
    }

    IEnumerator A4Coroutine()
    {
        for (int i = 0; i < AList[4].Length; i++)
        {
            AT[4].text += AList[4][i];
            yield return waitTime;
        }
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.12f);
        keyInput = true;
    }

    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                keyInput = false;
                theAudio.Play(keySound);
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                keyInput = false;
                theAudio.Play(keySound);
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                keyInput = false;
                theAudio.Play(enterSound);
                ExitChoice();
            }
        }

    }

    public void Selection()
    {
        Color color = A_Panel[0].GetComponent<Image>().color;
        color.r = 1f;
        color.g = 0.7843138f;
        color.b = 0;

        for (int i = 0; i <= count; i++)
        {
            A_Panel[i].GetComponent<Image>().color = color;
        }

        color.r = 1f;
        color.g = 1f;
        color.b = 0.5f;
        A_Panel[result].GetComponent<Image>().color = color;
        if (choicing)
            StartCoroutine(DelayCoroutine());
    }
}

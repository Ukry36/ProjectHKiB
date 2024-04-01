using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    public static DialogueManager instance;

    private void awake()
    {
        if(instance == null)
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

    private StandingManager theSTM;

    public Text text;
    public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    private List<string> listSpeakers;
    private List<string> listStandings;
    private Sprite dialogueWindow;

    public int count; // Count of sentences

    public Animator animSprite;
    public Animator animDialogueWindow;


    public Animator animFace;
    public Animator animEyebrow;
    public Animator animEyelash;
    public Animator animEye;
    public Animator animMouth;
    public Animator animAura;
    
    public bool talking;
    public bool keyActivated;

    private AudioManager theAudio;
    public string enterSound;


    void Start()
    {
        talking = false;
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSpeakers = new List<string>();
        listStandings = new List<string>();
        theAudio = FindObjectOfType<AudioManager>();
        theSTM = FindObjectOfType<StandingManager>();
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;
        keyActivated = false;

        for(int i=0; i<dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSpeakers.Add(dialogue.speakers[i]);
            listStandings.Add(dialogue.standings[i]);
        }
        dialogueWindow = dialogue.dialogueWindow;

        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);

        rendererDialogueWindow.sprite = dialogueWindow;

        animFace.SetBool("Appear", true);
        animEyebrow.SetBool("Appear", true);
        animEyelash.SetBool("Appear", true);
        animEye.SetBool("Appear", true);
        animMouth.SetBool("Appear", true);
        
        StartCoroutine(StartDialogueCoroutine());

    }

    public void ExitDialogue()
    {
        talking = false;
        count = 0;
        text.text = "";
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);

        animAura.SetBool("Appear", false);
        animFace.SetBool("Appear", false);
        animEyebrow.SetBool("Appear", false);
        animEyelash.SetBool("Appear", false);
        animEye.SetBool("Appear", false);
        animMouth.SetBool("Appear", false);
        listSentences.Clear();
        listSpeakers.Clear();
        listStandings.Clear();
    }

    IEnumerator StartDialogueCoroutine()
    {
        if(count > 0)
        {
            if(listStandings[count] != listStandings[count-1]) // => sprite 등을 바꾸기 위한 조건!!
            {
                animSprite.SetBool("Change",true);

                animFace.SetBool("Change", true);
                animEyebrow.SetBool("Change", true);
                animEyelash.SetBool("Change", true);
                animEye.SetBool("Change", true);
                animMouth.SetBool("Change", true);
                animAura.SetBool("Appear",false);

                yield return new WaitForSeconds(0.1f);
                //rendererSprite.GetComponent<SpriteRenderer>().sprite = listStandings[count]; // == rendererSprite.sprite = listSprites[count] 
                //>> standingmanager로!!!
                theSTM.SetStanding(listStandings[count]);
                theSTM.SetAura(listStandings[count]);

                animSprite.SetBool("Change",false);

                animFace.SetBool("Change", false);
                animEyebrow.SetBool("Change", false);
                animEyelash.SetBool("Change", false);
                animEye.SetBool("Change", false);
                animMouth.SetBool("Change", false);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            //rendererSprite.GetComponent<SpriteRenderer>().sprite = listStandings[count];
            theSTM.SetStanding(listStandings[0]);
            yield return new WaitForSeconds(0.1f);
            theSTM.SetAura(listStandings[count]);
        }
        
        
        text.text += "     ";
        for(int i=0; i<listSpeakers[count].Length; i++)
        {
            text.text += listSpeakers[count][i];
        }
        text.text += "\n";
        for(int i=0; i<listSentences[count].Length; i++) //Length는 문자열에도 쓸 수 있다 ~> count번째 문장의 i번째 글자를 입력함
        {
            text.text += listSentences[count][i];
            yield return new WaitForSeconds(0.01f);
        }
        keyActivated = true;
        
    }

    
    void Update()
    {
        if(talking && keyActivated)
        {
            
            if(Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                theAudio.Play(enterSound);
                text.text = ""; // 이번 문장의 로그를 삭제!
                count++; // 다음 문장으로!
                if(count == listSentences.Count) // 마지막 문장이면!
                {
                    StopAllCoroutines();
                    ExitDialogue(); // 끝!
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine()); // 코루틴 다시 시작!(?)
                }

            }
        
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomFace
{
    public string name;
    public int sprite;
    public Sprite face;
    public Sprite eyebrow;
    public Sprite eyelash;
    public Sprite eye;
    public Sprite mouth;
    public bool aura;
}

public class StandingManager : MonoBehaviour
{
    [SerializeField]
    public static StandingManager instance;
    

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

    public Sprite[] spriteList;
    public Sprite[] auraList;

    public SpriteRenderer rendererSprite; // sprite - audioclip, spriterenderer - audiosource
    public SpriteRenderer rendererFace;
    public SpriteRenderer rendererEyebrow;
    public SpriteRenderer rendererEyelash;
    public SpriteRenderer rendererEye;
    public SpriteRenderer rendererMouth;
    public SpriteRenderer rendererAura;
    public Animator animAura;

    public CustomFace[] customFace;

    
    public void SetStanding(string _name)
    {
        for(int i=0; i<customFace.Length; i++) 
        {
            if(_name == customFace[i].name)
            {
                rendererSprite.sprite = spriteList[customFace[i].sprite];
                rendererFace.sprite = customFace[i].face;
                rendererEyebrow.sprite = customFace[i].eyebrow;
                rendererEyelash.sprite = customFace[i].eyelash;
                rendererEye.sprite = customFace[i].eye;
                rendererMouth.sprite = customFace[i].mouth;
            }

        }
    }

    public void SetAura(string _name)
    {
        for(int i=0; i<customFace.Length; i++) 
        {
            if(_name == customFace[i].name)
            {
                StopAllCoroutines();
                if(customFace[i].aura)
                {
                    animAura.SetBool("Appear", true);
                    
                    StartCoroutine(AuraCoroutine());
                }
                else
                {
                    animAura.SetBool("Appear", false);
                    //StopAllCoroutines();
                }
            }

        }
    }

    IEnumerator AuraCoroutine()
    {
        for(int i=0; i< auraList.Length; i++)
        {
            rendererAura.sprite = auraList[i];
            yield return new WaitForSeconds(0.3f); 
            if(i == auraList.Length - 1)
                i = -1;
        }

    }


}

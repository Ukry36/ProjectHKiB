using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private List<MoveSprite> characters;
    private CameraManager theCamera;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
    }

    public void PreloadCharacter()
    {
        characters = ToList();
    }

    public List<MoveSprite> ToList()
    {
        List<MoveSprite> tempList = new();
        MoveSprite[] temp = FindObjectsOfType<MoveSprite>();

        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        return tempList;
    }

    public void Move(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++) // 배열: Length 리스트: Count
        {
            if (_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void Freeze(string _name)
    {
        if (_name != "Camera")
        {
            for (int i = 0; i < characters.Count; i++)
                if (_name == characters[i].characterName)
                    characters[i].freeze = true;
        }
        else theCamera.freeze = true;


        if (_name == "ALL")
            for (int i = 0; i < characters.Count; i++)
                characters[i].freeze = true;
    }

    public void UnFreeze(string _name)
    {
        if (_name != "Camera")
        {
            for (int i = 0; i < characters.Count; i++)
                if (_name == characters[i].characterName)
                    characters[i].freeze = false;
        }
        else theCamera.freeze = false;


        if (_name == "ALL")
            for (int i = 0; i < characters.Count; i++)
                characters[i].freeze = false;
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);
                switch (_dir)
                {

                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                }

            }
        }
    }

    public void SetInvisible(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    public void Setvisible(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetBoxColOff(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }

    public void SetBoxColOn(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

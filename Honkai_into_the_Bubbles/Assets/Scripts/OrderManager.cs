using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private List<Entity> characters;
    private CameraManager theCamera;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
    }

    public void PreloadCharacter()
    {
        characters = ToList();
    }

    public List<Entity> ToList()
    {
        List<Entity> tempList = new();
        Entity[] temp = FindObjectsOfType<Entity>();

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
            if (_name == characters[i].Name)
            {
                //characters[i].Move(_dir);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].Name)
            {
                characters[i].Animator.SetFloat("DirX", 0f);
                characters[i].Animator.SetFloat("DirY", 0f);
                switch (_dir)
                {

                    case "UP":
                        characters[i].Animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].Animator.SetFloat("DirY", -1f);
                        break;
                    case "RIGHT":
                        characters[i].Animator.SetFloat("DirX", 1f);
                        break;
                    case "LEFT":
                        characters[i].Animator.SetFloat("DirX", -1f);
                        break;
                }

            }
        }
    }

    public void SetInvisible(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].Name)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    public void Setvisible(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].Name)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetBoxColOff(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].Name)
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }

    public void SetBoxColOn(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].Name)
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

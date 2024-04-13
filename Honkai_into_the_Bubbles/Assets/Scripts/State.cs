using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool isPlayer = false;
    public int maxHP = 100;
    public int currentHP;
    public int maxGP = 20;
    public int currentGP;

    public int ATK = 100;
    public int DEF = 0;
    public int CritRate = 0;
    public int CritDMG = 10;
    [HideInInspector] public MoveSprite moveSprite;

    private void Start()
    {

    }

    public void Hit(int _dmg, bool _crit, bool _strong, Vector3 _dir)
    {
        moveSprite.Hit();
        int trueDmg;
        if (_dmg > DEF)
            trueDmg = _dmg - DEF;
        else
            trueDmg = 1;

        HPControl(-trueDmg);

/*
        Vector3 vector = this.transform.position;
        vector.y += Random.Range(-1f, 1f);
        vector.x += Random.Range(-1f, 1f);

        var clone = Instantiate(FloatingText_PreFab, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = "-" + trueDmg;

        Color color = clone.GetComponent<FloatingText>().text.color;
        color = new Color(0.8f, 0.2f, 0.3f, 1f);
        if (_crit)
        {
            color.b -= 0.3f;
            clone.GetComponent<FloatingText>().text.fontSize += 2;
        }
        clone.GetComponent<FloatingText>().text.color = color;


        clone.GetComponent<FloatingText>().moveSpeed = 0.1f;
        clone.GetComponent<FloatingText>().destroyTime = 0.5f;
        clone.transform.SetParent(parent.transform);
*/
        
        if (_strong)
        {
            moveSprite.Grrogy(_dir);
        }
            
    }

    public void SetHitAnimObject()
    {
        moveSprite = GetComponentsInChildren<MoveSprite>(false)[0];
    }

    public void HPControl(int _o)
    {
        currentHP += _o;
        if (currentHP > maxHP)
            currentHP = maxHP;
        if (currentHP < 0)
        {
            currentHP = 0;
            moveSprite.Die();
        }
    }

    public void GPControl(int _o)
    {
        currentGP += _o;
        if (currentGP > maxGP)
            currentGP = maxGP;
        if (currentGP < 0)
            currentGP = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isPlayer = false;
    public bool isStealth = false;
    public bool superArmor = false; // no knockback
    public bool invincible = false; // no damage
    public int maxHP = 100;
    public int currentHP;
    public int maxGP = 20;
    public int currentGP;

    public int ATK = 100;
    public int DEF = 0;
    public int CritRate = 0;
    public int CritDMG = 10;
    [HideInInspector] public Entity entity;


    public void Hit(int _dmg, bool _crit, bool _strong, Vector3 _dir)
    {
        int trueDmg;
        if (_dmg > DEF)
            trueDmg = _dmg - DEF;
        else
            trueDmg = 1;

        if (!invincible)
        {
            StartCoroutine(entity.HitCoroutine());
            HPControl(-trueDmg);
        }


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
            Debug.Log("이거 고쳐야함!!!!!!!!");
            entity.Knockback(_dir, 1); /////////// strong을 int형으로!!!!!!!
        }
    }

    public void SetHitAnimObject()
    {
        entity = GetComponentsInChildren<Entity>(false)[0];
    }

    public void HPControl(int _o)
    {
        currentHP += _o;
        if (currentHP > maxHP)
            currentHP = maxHP;
        if (currentHP <= 0)
        {
            currentHP = 0;
            entity.Die();
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

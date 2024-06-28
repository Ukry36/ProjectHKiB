using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    #region Singleton

    public static DBManager instance;

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
    #endregion

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    private Status thePStat;
    public GameObject FloatingText_PreFab;
    public GameObject parent;

    public void FloatText(string _type, int _num)
    {
        Vector3 vector = thePStat.transform.position;
        vector.y += Random.Range(0, 3f);
        vector.x += Random.Range(0, 3f);

        var clone = Instantiate(FloatingText_PreFab, vector, Quaternion.Euler(Vector3.zero));
        switch (_type)
        {
            case "HEAL":
                clone.GetComponent<FloatingText>().text.color = new Color(0.5f, 0.9f, 0, 1f);
                break;
            case "SP":
                clone.GetComponent<FloatingText>().text.color = new Color(1f, 0.6f, 0, 1f);
                break;
        }
        clone.GetComponent<FloatingText>().text.text = "+ " + _num;
        clone.GetComponent<FloatingText>().moveSpeed = 0.1f;
        clone.GetComponent<FloatingText>().destroyTime = 0.5f;
        clone.transform.SetParent(parent.transform);
    }


    public void UseItem(int _ID)
    {
        switch (_ID)
        {
            case 20000:
                if (thePStat.maxHP >= thePStat.currentHP + 1)
                    thePStat.currentHP += 1;
                else
                    thePStat.currentHP = thePStat.maxHP;
                FloatText("HEAL", 1);
                break;
            case 20001:
                if (thePStat.maxHP >= thePStat.currentHP + 3)
                    thePStat.currentHP += 3;
                else
                    thePStat.currentHP = thePStat.maxHP;
                FloatText("HEAL", 3);
                break;
            case 20002:
                if (thePStat.maxHP >= thePStat.currentHP + 10)
                    thePStat.currentHP += 10;
                else
                    thePStat.currentHP = thePStat.maxHP;
                FloatText("HEAL", 10);
                break;
            case 20003:
                if (thePStat.maxHP >= thePStat.currentHP + 30)
                    thePStat.currentHP += 30;
                else
                    thePStat.currentHP = thePStat.maxHP;
                FloatText("HEAL", 30);
                break;
            case 20004:
                if (thePStat.maxGP >= thePStat.currentGP + 1)
                    thePStat.currentGP += 1;
                else
                    thePStat.currentGP = thePStat.maxGP;
                FloatText("SP", 1);
                break;
            case 20005:
                if (thePStat.maxGP >= thePStat.currentGP + 3)
                    thePStat.currentGP += 3;
                else
                    thePStat.currentGP = thePStat.maxGP;
                FloatText("SP", 3);
                break;
            case 20006:
                if (thePStat.maxGP >= thePStat.currentGP + 10)
                    thePStat.currentGP += 10;
                else
                    thePStat.currentGP = thePStat.maxGP;
                FloatText("SP", 10);
                break;
            case 20007:
                if (thePStat.maxGP >= thePStat.currentGP + 30)
                    thePStat.currentGP += 30;
                else
                    thePStat.currentGP = thePStat.maxGP;
                FloatText("SP", 30);
                break;
        }
    }

    void Start()
    {
        thePStat = FindObjectOfType<Status>();

        itemList.Add(new Item());

        itemList.Add(new Item(10000, "체력물약", "대쉬 기능 해금", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10001, "잠자는 미녀", "잠자는 미녀 해금", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10002, "백조의 호수", "백조의 호수 해금", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10003, "머리끈", "", Item.ItemType.Effect, 2));
        itemList.Add(new Item(10004, "키", "이동속도 증가, 공격 방식 변경", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10005, "드론", "공격 방식 변경", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10006, "스페이드2", "가하는 피해 20% 증가", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10007, "은화", "공격 불가, 적의 타겟이 되지 않음", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10008, "박스", "공격 불가, 함정에 받는 피해 0", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10009, "좌스터", "끼잉끼잉!", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10010, "헤드셋", "음악 재생 가능", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10011, "월광", "공격 방식 변경", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10012, "우산", "비", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10013, "게임기", "", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10014, "옛 사진", "", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10015, "수정 장미", "", Item.ItemType.Effect, 6));
        itemList.Add(new Item(10016, "롱패딩", "추운 구역 진입 가능", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10017, "팬지꽃", "", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10018, "이불", "이동속도 감소, 추운 구역 진입 가능", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10019, "조명", "어두운 구역에서 발광", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10020, "액자", "이동속도 감소, 공격 불가, 받는 피해 0", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10021, "머리카락", "", Item.ItemType.Effect, 2));
        itemList.Add(new Item(10022, "검댕", "공격 속도 증가", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10023, "무한의 증명", "대쉬 스킬 해금", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10024, "공허의 증명", "", Item.ItemType.Effect, 2));
        itemList.Add(new Item(10025, "거울", "의상 기능 해금", Item.ItemType.Effect, 6));
        itemList.Add(new Item(10026, "낡은 키", "이동속도 감소, 공격방식 변경", Item.ItemType.Effect, 5));
        itemList.Add(new Item(10028, "가면", "받는 피해 20% 감소", Item.ItemType.Effect, 3));
        itemList.Add(new Item(10029, "네모네모", "공격 불가, 받는 피해 0", Item.ItemType.Effect, 4));
        itemList.Add(new Item(10030, "수상한 약물", "", Item.ItemType.Effect, 2));
        itemList.Add(new Item(10032, "카메라", "", Item.ItemType.Effect, 2));

        itemList.Add(new Item(10036, "36", "", Item.ItemType.Effect, 0));

        itemList.Add(new Item(60000, "델타의 무기", "", Item.ItemType.Effect, 5, 1, 0, new int[] { 10001, 10002 }));
        itemList.Add(new Item(60001, "일심동체", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 60000, 10006 }));
        itemList.Add(new Item(60002, "중장토끼(원거리)", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 10004, 10005 }));
        itemList.Add(new Item(60003, "중장토끼(근거리)", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 60002, 10012 }));
        itemList.Add(new Item(60004, "중장토끼(발키리)", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 60003, 10030 }));
        itemList.Add(new Item(60005, "눈꽃", "", Item.ItemType.Effect, 4, 1, 0, new int[] { 10012, 10016 }));
        itemList.Add(new Item(60006, "X", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 10005, 10026, 10011 }));
        itemList.Add(new Item(60007, "투사이드업", "", Item.ItemType.Effect, 2, 1, 0, new int[] { 10003, 10021 }));
        itemList.Add(new Item(60008, "실험가운", "", Item.ItemType.Effect, 4, 1, 0, new int[] { 10023, 10030 }));
        itemList.Add(new Item(60009, "칠흑", "", Item.ItemType.Effect, 4, 1, 0, new int[] { 10022, 10024 }));
        itemList.Add(new Item(60010, "좌스터의 꼬리", "", Item.ItemType.Effect, 5, 1, 0, new int[] { 10009, 10030 }));
        itemList.Add(new Item(60011, "최적의 수면조건", "", Item.ItemType.Effect, 6, 1, 0, new int[] { 10002, 10018 }));
        itemList.Add(new Item(60012, "키라라", "", Item.ItemType.Effect, 4, 1, 0, new int[] { 10007, 10008 }));
        itemList.Add(new Item(60013, "흑백 드론", "", Item.ItemType.Effect, 5, 1, 0, new int[] { 10005, 10014 }));
        itemList.Add(new Item(60014, "흑백 낡은 키", "", Item.ItemType.Effect, 5, 1, 0, new int[] { 10026, 10014 }));
        itemList.Add(new Item(60015, "흑백 가면", "", Item.ItemType.Effect, 3, 1, 0, new int[] { 10028, 10014 }));
        itemList.Add(new Item(60016, "흑백 액자", "", Item.ItemType.Effect, 3, 1, 0, new int[] { 10020, 10014 }));
        itemList.Add(new Item(60017, "흑백 키", "", Item.ItemType.Effect, 5, 1, 0, new int[] { 10004, 10014 }));
        // 마저 추가할 것! 
        // !!!!!!주의!!!!!! 2개짜리와 3개짜리의 path가 겹친다면 무조건 3개짜리가 뒤로 간다!!!!!

        itemList.Add(new Item(20000, "HP팩", "HP를 1 회복", Item.ItemType.Use, 2));
        itemList.Add(new Item(20001, "HP팩", "HP를 3 회복", Item.ItemType.Use, 3));
        itemList.Add(new Item(20002, "HP팩", "HP를 10 회복", Item.ItemType.Use, 4));
        itemList.Add(new Item(20003, "HP팩", "HP를 30 회복", Item.ItemType.Use, 5));
        itemList.Add(new Item(20004, "SP팩", "SP를 1 회복", Item.ItemType.Use, 2));
        itemList.Add(new Item(20005, "SP팩", "SP를 3 회복", Item.ItemType.Use, 3));
        itemList.Add(new Item(20006, "SP팩", "SP를 10 회복", Item.ItemType.Use, 4));
        itemList.Add(new Item(20007, "SP팩", "SP를 30 회복", Item.ItemType.Use, 5));


    }

}

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

    public List<Item> itemList = new();

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


    void Start()
    {
        thePStat = FindObjectOfType<Status>();

        itemList.Add(new Item());

        // N: Neutral, A: Assist, C: Combat, H: Hybrid, P: Peace, S: Summon
        itemList.Add(new Item(10000, "체력물약",
        "정체를 알 수 없는 물약. 마시면 힘이 난다. 잘하면 페인트로 쓸 수도..? \n <A> 메인 효과: 대쉬, 서브 효과: 대쉬",
        Item.ItemType.Effect));

        itemList.Add(new Item(10001, "잠자는 미녀",
        "잡으면 그리운 느낌이 드는 무기.\n <C>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10002, "백조의 호수",
        "잡으면 슬픈 느낌이 드는 무기. \n <C>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10003, "머리끈",
        "머리끈이다. ... \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10004, "키",
        "중장토끼의 키. \n <H> 메인 효과: 바이크를 소환, 특수 효과: 바이크에 탑승",
        Item.ItemType.Effect));

        itemList.Add(new Item(10005, "드론",
        "중장토끼의 드론 형태. \n <H> 메인/서브 효과: 자동 원거리 공격을 하는 드론이 소환, 특수효과: 원거리 공격을 보조하는 메카닉 소환",
        Item.ItemType.Effect));

        itemList.Add(new Item(10006, "스페이드2",
        "무엇이든 2배로 늘려준다고 한다. 원리는 알 수 없다. \n <A> 메인 효과: 일부 공격의 타수 증가, 서브 효과: 공격력 20 증가",
        Item.ItemType.Effect));

        itemList.Add(new Item(10007, "은화",
        "이걸 만지면 고양이가 된 기분이 든다. \n <P> 메인 효과: 적에게 발각될 확률 감소, 특수효과: 특정 생물을 끌어들일 수 있음.",
        Item.ItemType.Effect));

        itemList.Add(new Item(10008, "박스",
        "덮어쓰면 알 수 없는 안정감이 든다. \n <P> 메인 효과: 적에게 발각될 확률 감소, 특수효과: 함정 등의 대미지 무효",
        Item.ItemType.Effect));

        itemList.Add(new Item(10009, "좌스터",
        "끼잉끼잉! \n <S> 메인/서브 효과: 자동 원거리 공격을 하는 좌스터가 소환",
        Item.ItemType.Effect));

        itemList.Add(new Item(10010, "헤드셋",
        "오래된 헤드셋. 뿔 때문에 착용하기 귀찮다. \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10011, "월광",
        "-----------------",
        Item.ItemType.Effect));

        itemList.Add(new Item(10012, "우산",
        "파란색 우산. 왠지 이걸 쓰기만 하면 비가 내린다. 혹시 정어리도 내리지 않을까? \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10013, "게임기",
        "내 게임기. \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10014, "옛 사진",
        "기억을 불러일으키는 오래된 사진이다. \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10015, "수정 장미",
        "누군가의 고결한 영혼이 담긴 물건이다. \n <A> 메인 효과: 1가지 스킬을 골라 추가, 서브 효과: 랜덤 스킬을 추가 ",
        Item.ItemType.Effect));

        itemList.Add(new Item(10016, "롱패딩",
        "지니고만 있어도 추위가 사라지는 듯한 따듯함이다. \n <A> 메인 효과: 추위 대미지 무효, 서브 효과: 추위 대미지 50% 감소",
        Item.ItemType.Effect));

        itemList.Add(new Item(10017, "팬지꽃",
        "시들지 않는 신비한 꽃. 꽃술이 신기하게 빛난다. \n <A> 메인/서브 효과: 간이 워프지점을 마크 가능",
        Item.ItemType.Effect));

        itemList.Add(new Item(10018, "이불",
        "아늑하다. 들어가있으면 왠지 움직이기 싫어진다. \n <P> 메인 효과: 추위 대미지 무효",
        Item.ItemType.Effect));

        itemList.Add(new Item(10019, "조명",
        "머리가 조명으로 바뀐 것 같다. 왠지 앞은 여전히 잘 보인다. 조금 불안정해서 가끔 꺼진다. \n <A> 메인 효과: 주위와 바라보는 방향을 밝힌다. 서브 효과: 주위를 밝힌다.",
        Item.ItemType.Effect));

        itemList.Add(new Item(10020, "액자",
        "왠지 안으로 들어갈 수 있는 액자. 움직일 순 있지만 느리다. \n <P> 메인 효과: 대미지 무효",
        Item.ItemType.Effect));

        itemList.Add(new Item(10021, "머리카락",
        "수상한 머리카락 몇 올. 내 것은 아닌 것 같다. \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10022, "검댕",
        "만지면 손에 검은 먼지가 묻어나올 것 같다. \n <A> 메인 효과: ---------------",
        Item.ItemType.Effect));

        itemList.Add(new Item(10023, "무한의 증명",
        "커다란 팬던트. 무언가의 힘을 느낄 수 있다. \n <A> 메인 효과: 회피 지속시간 증가, 서브 효과: 연속 회피 횟수 증가",
        Item.ItemType.Effect));

        itemList.Add(new Item(10024, "공허의 증명",
        "작은 팬던트. 동전같아보이기도 한다. \n <A> 메인 효과: 회피 거리 증가, 서브 효과: 회피 쿨타임 감소",
        Item.ItemType.Effect));

        itemList.Add(new Item(10025, "거울",
        "----------------", Item.ItemType.Effect, 6));

        itemList.Add(new Item(10026, "낡은 키",
        "낡은 로봇의 열쇠다. \n <H> 메인 효과: ------------, 특수효과: 포탑으로 변해 사격",
        Item.ItemType.Effect));

        itemList.Add(new Item(10028, "가면",
        "동료의 것과 비슷한 느낌의 가면이다. \n <C>",
        Item.ItemType.Effect));

        // 삭제 가능성
        itemList.Add(new Item(10029, "네모네모",
        "안에 들어갈 수 있다. 불편하긴 하지만 엄청 단단한 것 같다. \n <P> 메인 효과: 대미지 무효",
        Item.ItemType.Effect));

        itemList.Add(new Item(10030, "수상한 약물",
        "정체를 알 수 없는 약물. 마시면... \n <N>",
        Item.ItemType.Effect));

        itemList.Add(new Item(10032, "카메라",
        "", Item.ItemType.Effect));

        itemList.Add(new Item(10036, "36",
        "",
        Item.ItemType.Effect));



        itemList.Add(new Item(60000, "델타의 무기",
        "익숙한 감각의 대검 두 자루. \n <C>",
        Item.ItemType.Effect, 1, 0, new int[] { 10001, 10002 }));

        itemList.Add(new Item(60001, "일심동체",
        "왠지 이전보다 무기를 더 잘 다룰 수 있을 것 같다. \n <C>",
        Item.ItemType.Effect, 1, 0, new int[] { 60000, 10006 }));

        itemList.Add(new Item(60002, "중장토끼(원거리)",
        "살아움직이기 시작했다.. \n <S> 메인/서브 효과: 자동 원거리 공격을 하는 중장토끼가 소환",
        Item.ItemType.Effect, 1, 0, new int[] { 10004, 10005 }));

        itemList.Add(new Item(60003, "중장토끼(근거리)",
        "검을 쥐여줬다.. \n <S> 메인/서브 효과: 자동 근거리 공격을 하는 중장토끼가 소환",
        Item.ItemType.Effect, 1, 0, new int[] { 60002, 10012 }));

        itemList.Add(new Item(60004, "중장토끼(발키리)",
        "나랑 같은 크기까지 커졌다. 좀 더 전투를 원활히 할 수 있을 것 같다. \n <H> 메인/서브효과: 자동 근거리 공격을 하는 중장토끼가 소환, 특수효과: 중장토끼와 교대",
        Item.ItemType.Effect, 1, 0, new int[] { 60003, 10030 }));

        itemList.Add(new Item(60005, "눈꽃",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10012, 10016 }));

        itemList.Add(new Item(60006, "X",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10005, 10026, 10011 }));

        itemList.Add(new Item(60007, "투사이드업",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10003, 10021 }));

        itemList.Add(new Item(60008, "실험가운",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10023, 10030 }));

        itemList.Add(new Item(60009, "칠흑",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10022, 10024 }));

        itemList.Add(new Item(60010, "좌스터의 꼬리",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10009, 10030 }));

        itemList.Add(new Item(60011, "최적의 수면조건",
        "이거라면 잠에 들 수 있을 것 같다.. \n <P> 메인 효과: 거점으로 복귀",
        Item.ItemType.Effect, 1, 0, new int[] { 10002, 10018 }));

        itemList.Add(new Item(60012, "키라라",
        "",
        Item.ItemType.Effect, 1, 0, new int[] { 10007, 10008 }));

        // 마저 추가할 것! 
        // !!!!!!주의!!!!!! 2개짜리와 3개짜리의 path가 겹친다면 무조건 3개짜리가 뒤로 간다!!!!!

        itemList.Add(new Item(20000, "HP팩", "HP를 1 회복", Item.ItemType.Use));
        itemList.Add(new Item(20001, "HP팩", "HP를 3 회복", Item.ItemType.Use));
        itemList.Add(new Item(20002, "HP팩", "HP를 10 회복", Item.ItemType.Use));
        itemList.Add(new Item(20003, "HP팩", "HP를 30 회복", Item.ItemType.Use));
        itemList.Add(new Item(20004, "GP팩", "GP를 1 회복", Item.ItemType.Use));
        itemList.Add(new Item(20005, "GP팩", "GP를 3 회복", Item.ItemType.Use));
        itemList.Add(new Item(20006, "GP팩", "GP를 10 회복", Item.ItemType.Use));
        itemList.Add(new Item(20007, "GP팩", "GP를 30 회복", Item.ItemType.Use));


    }

}

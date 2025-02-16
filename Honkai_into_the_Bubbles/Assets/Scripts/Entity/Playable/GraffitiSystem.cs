using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GraffitiSystem : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject NexusPrefab;
    [HideInInspector] public List<Vector2> activatedTileList = new();
    [SerializeField] private Attractive attractive;
    private Vector2 LB;
    public List<Skill> skillList = new();

    private EffectVanishControl EVC;


    private void OnEnable()
    {
        GraffitiSystemManager.instance.playerGS = this;
    }


    public void StartGraffiti()
    {
        GraffitiSystemManager.instance.OnGraffitiStart();
        attractive.DisableAttract();
        grid.SetActive(true);
        StartCoroutine(TimeScalerCoroutine(true));
        grid.transform.position = new Vector3((int)transform.position.x, (int)transform.position.y);
        activatedTileList.Clear();
        CameraManager.instance.ZoomViaOrig(0.707f, 0.5f);
        var clone = PoolManager.instance.ReuseGameObject(NexusPrefab, this.transform.position, quaternion.identity);
        EVC = clone.GetComponent<EffectVanishControl>();
        PostProcessManager.instance.ChromaticImpact(0, 1);
    }

    IEnumerator TimeScalerCoroutine(bool _down)
    {
        int c = 1;
        if (_down)
            c = -1;
        for (int i = 0; i < 21; i++)
        {
            yield return null;
            Time.timeScale += 0.045f * c;
        }
    }

    public int[] EndGraffiti()
    {
        GraffitiSystemManager.instance.OnGraffitiEnd();
        attractive.DisableAttract();
        StartCoroutine(TimeScalerCoroutine(false));
        grid.SetActive(false);
        attractive.EnableAttract();
        CameraManager.instance.ReturntoOrigRes(0.1f);
        EVC.ExitbyOrder();
        return GetSkillNum();
    }

    public void AddTile(Vector2 _co)
    {
        if (!activatedTileList.Exists(co => co == _co))
        {
            PlayerManager.instance.theStat.GPControl(-1, _silence: true);
            activatedTileList.Add(_co);
        }
    }

    private void Normalize()
    {
        LB = Vector2.zero;
        for (int i = 0; i < activatedTileList.Count; i++)
        {
            if (activatedTileList[i].x < LB.x)
                LB.x = activatedTileList[i].x;
            if (activatedTileList[i].y < LB.y)
                LB.y = activatedTileList[i].y;
        }

        for (int i = 0; i < activatedTileList.Count; i++)
        {
            activatedTileList[i] -= LB;
        }
    }

    private void Turn90()
    {
        for (int i = 0; i < activatedTileList.Count; i++)
        {
            activatedTileList[i] = new Vector2(-activatedTileList[i].y, activatedTileList[i].x);
        }
        Normalize();
    }

    private int[] GetSkillNum()
    {
        Normalize();

        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillCommand.Count == activatedTileList.Count)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (CheckSameList(skillList[i].skillCommand, activatedTileList))
                        return new int[] { i, activatedTileList.Count };
                    Turn90();
                }
            }
        }
        return new int[] { -1, activatedTileList.Count };
    }

    private bool CheckSameList(List<Vector2> _l1, List<Vector2> _l2)
    {
        for (int i = 0; i < _l1.Count; i++)
        {
            if (!_l2.Contains(_l1[i]))
                return false;
        }
        return true;
    }

}

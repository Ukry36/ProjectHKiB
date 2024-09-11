using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPlacerTest : MonoBehaviour
{
    private ObjectCustomizeTest objectCustomizer;

    [SerializeField] private List<GameObject> gameObjects = new();
    private int select;

    [SerializeField] private string placedSFX;
    [SerializeField] private string cannotPlaceSFX;
    [SerializeField] private string scrollSFX;

    [SerializeField] private LayerMask cannotplaceLayer;

    // Start is called before the first frame update
    void Start()
    {
        objectCustomizer = FindObjectOfType<ObjectCustomizeTest>();
        foreach (var obj in gameObjects)
        {
            obj.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnEnable()
    {
        gameObjects[select].SetActive(true);
    }

    private void Update()
    {
        this.transform.position = Vector3Int.FloorToInt(PlayerManager.instance.transform.position);
        if (InputManager.instance.NextInput) NextObject();
        else if (InputManager.instance.PrevInput) PrevObject();
        else if (InputManager.instance.AttackInput) PlaceObject();
        else if (InputManager.instance.DodgeInput) RemoveObject();
    }

    public void NextObject()
    {
        int prevSelect = select;
        select++;
        select = select >= gameObjects.Count ? 0 : select;
        gameObjects[prevSelect].SetActive(false);
        gameObjects[select].SetActive(true);
        AudioManager.instance.PlaySoundFlat(scrollSFX);
    }

    public void PrevObject()
    {
        int nextSelect = select;
        select--;
        select = select < 0 ? gameObjects.Count - 1 : select;
        gameObjects[nextSelect].SetActive(false);
        gameObjects[select].SetActive(true);
        AudioManager.instance.PlaySoundFlat(scrollSFX);
    }

    public void PlaceObject()
    {
        if (!Physics2D.OverlapCircle(this.transform.position, 0.5f, cannotplaceLayer))
        {
            ListObject(Instantiate(gameObjects[select], this.transform.position, quaternion.identity));
            AudioManager.instance.PlaySoundFlat(placedSFX);
        }
        else
        {
            AudioManager.instance.PlaySoundFlat(cannotPlaceSFX);
        }
    }

    private void RemoveObject()
    {
        Debug.Log("dd");
        if (objectCustomizer.placedGameObjects.Exists(a => a.transform.position == this.transform.position))
        {
            List<GameObject> gameObjects = objectCustomizer.placedGameObjects.FindAll(a => a.transform.position == this.transform.position);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                objectCustomizer.placedGameObjects.Remove(gameObjects[i]);
                Destroy(gameObjects[i]);
            }
        }
    }

    public void ListObject(GameObject _obj)
    {
        objectCustomizer.placedGameObjects.Add(_obj);
    }

}

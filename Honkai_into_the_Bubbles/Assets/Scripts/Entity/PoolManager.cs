using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{

    #region Singleton

    public static PoolManager instance;

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

    [SerializeField] private Pool[] poolArray = null;

    private Dictionary<int, Queue<GameObject>> poolDictionary = new();

    private Transform objectPoolTransform;

    public GameObject hitParticle;

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
    }

    private void Start()
    {
        objectPoolTransform = this.gameObject.transform;

        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize);
        }
    }

    private void CreatePool(GameObject _prefab, int _poolSize)
    {
        int poolKey = _prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < _poolSize; i++)
            {
                GameObject newObject = Instantiate(_prefab, objectPoolTransform);

                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    public GameObject ReuseGameObject(GameObject _prefab, Vector3 _position, Quaternion _rotation)
    {
        int poolKey = _prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject objectToReuse = GetObjectFromPool(poolKey);

            ResetObject(_position, _rotation, objectToReuse, _prefab);

            objectToReuse.SetActive(true);

            return objectToReuse;
        }
        else
        {
            Debug.Log("No Object Pool for " + _prefab);
            return null;
        }
    }

    private GameObject GetObjectFromPool(int _poolKey)
    {
        GameObject objectToReuse = poolDictionary[_poolKey].Dequeue();

        poolDictionary[_poolKey].Enqueue(objectToReuse);

        if (objectToReuse.activeSelf == true)
        {
            objectToReuse.SetActive(false);
        }

        return objectToReuse;
    }

    private void ResetObject(Vector3 _position, Quaternion _rotation, GameObject _objectToReuse, GameObject _prefab)
    {
        _objectToReuse.transform.SetPositionAndRotation(_position, _rotation);
        _objectToReuse.transform.localScale = _prefab.transform.localScale;
    }
}
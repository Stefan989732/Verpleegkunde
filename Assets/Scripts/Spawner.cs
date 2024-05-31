using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    // Dictionary to hold tag-prefab mappings
    private Dictionary<string, Queue<GameObject>> prefabPoolDictionary;

    // Assign prefabs and their tags in the Inspector
    [System.Serializable]
    public struct TagPrefabPair
    {
        public string tag;
        public GameObject prefab;
    }
    public TagPrefabPair[] tagPrefabPairs;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePrefabPoolDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize the dictionary with tag-prefab pairs
    void InitializePrefabPoolDictionary()
    {
        prefabPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pair in tagPrefabPairs)
        {
            if (!prefabPoolDictionary.ContainsKey(pair.tag))
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                prefabPoolDictionary.Add(pair.tag, objectPool);
            }
        }
    }

    // Method to spawn (activate) objects based on tag
    public void Spawn(string tag, Vector3 position, Quaternion rotation)
    {
        if (prefabPoolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> objectPool = prefabPoolDictionary[tag];

            // Try to get an inactive object from the pool
            if (objectPool.Count > 0)
            {
                GameObject objectToSpawn = objectPool.Dequeue();
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.SetActive(true);
            }
            else
            {
                // If no inactive object is available, instantiate a new one
                GameObject prefab = GetPrefabByTag(tag);
                if (prefab != null)
                {
                    GameObject newObject = Instantiate(prefab, position, rotation);
                    newObject.SetActive(true);
                }
            }
        }
    }

    // Method to get the prefab based on the tag
    private GameObject GetPrefabByTag(string tag)
    {
        foreach (var pair in tagPrefabPairs)
        {
            if (pair.tag == tag)
            {
                return pair.prefab;
            }
        }
        return null;
    }

    // Method to deactivate (return) objects back to the pool
    public void Deactivate(GameObject objectToDeactivate, string tag)
    {
        objectToDeactivate.SetActive(false);
        if (prefabPoolDictionary.ContainsKey(tag))
        {
            prefabPoolDictionary[tag].Enqueue(objectToDeactivate);
        }
    }
}

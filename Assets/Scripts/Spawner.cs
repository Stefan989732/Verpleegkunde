using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    public GameObject prefabToSpawn;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Spawn(Vector3 position)
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, position, Quaternion.identity);
        }
    }
}

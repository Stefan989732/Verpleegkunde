using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    [System.Serializable]
    public class TagData
    {
        public string tag;
        public GameObject gameObject;
    }

    public TagData[] tagDatas;
    public static TagManager Instance { get; private set; }
    public Dictionary<string, TagData> tagPoolDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeTagPoolDictionary();
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeTagPoolDictionary()
    {
        tagPoolDictionary = new Dictionary<string, TagData>();
        foreach (var tagData in tagDatas)
        {
            if (!tagPoolDictionary.ContainsKey(tagData.tag))
            {
                tagPoolDictionary.Add(tagData.tag, tagData);
            }
        }
    }

    public TagData GetTagData(string tag)
    {
        if (tagPoolDictionary.ContainsKey(tag))
        {
            return tagPoolDictionary[tag];
        }
        else
        {
            return null;
            Destroy(gameObject);
        }
    }
}

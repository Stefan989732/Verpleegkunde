using System.Collections.Generic;
using UnityEngine;

public class DragToTrayManager : MonoBehaviour
{
[System.Serializable]
    public class DraggableObjectInfo
    {
        public string objectTag;
        public int maxCopies;
        [HideInInspector] public int currentCopies;
    }

    public DraggableObjectInfo[] draggableObjects;

    private Dictionary<string, DraggableObjectInfo> objectInfoDict;

    void Start()
    {
        objectInfoDict = new Dictionary<string, DraggableObjectInfo>();
        foreach (var objInfo in draggableObjects)
        {
            objectInfoDict[objInfo.objectTag] = objInfo;
        }
    }

    public bool CanCreateNewInstance(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            return objectInfoDict[objectTag].currentCopies < objectInfoDict[objectTag].maxCopies;
        }
        return false;
    }

    public void IncrementInstanceCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            objectInfoDict[objectTag].currentCopies++;
        }
    }

    public void DecrementInstanceCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            objectInfoDict[objectTag].currentCopies--;
        }
    }

    public int GetCopyCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            return objectInfoDict[objectTag].currentCopies;
        }
        return 0;
    }
}

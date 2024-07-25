using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// based on a tagging system, manage how many copies can be created
/// </summary>
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

    /// <summary>
    /// checks if the user can create a new instance 
    /// </summary>
    /// <param name="objectTag"></param>
    /// <returns></returns>
    public bool CanCreateNewInstance(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            return objectInfoDict[objectTag].currentCopies < objectInfoDict[objectTag].maxCopies;
        }
        return false;
    }

    /// <summary>
    /// Increase the object instance count
    /// </summary>
    /// <param name="objectTag"></param>
    public void IncrementInstanceCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            objectInfoDict[objectTag].currentCopies++;
        }
    }

    /// <summary>
    /// Decrease the object instance count
    /// </summary>
    /// <param name="objectTag"></param>
    public void DecrementInstanceCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            objectInfoDict[objectTag].currentCopies--;
        }
    }

    /// <summary>
    /// Get the amount of copies if available, else 0
    /// </summary>
    /// <param name="objectTag"></param>
    /// <returns></returns>
    public int GetCopyCount(string objectTag)
    {
        if (objectInfoDict.ContainsKey(objectTag))
        {
            return objectInfoDict[objectTag].currentCopies;
        }
        return 0;
    }
}

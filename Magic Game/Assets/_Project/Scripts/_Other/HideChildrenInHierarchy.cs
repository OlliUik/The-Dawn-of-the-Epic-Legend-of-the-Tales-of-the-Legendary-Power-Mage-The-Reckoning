using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hide child objects/transforms in Hierarchy view.
/// </summary>

/*
* USAGE: Add this script to a gameobject, and it will automatically hide
* all child objects/transforms in hierarchy view.
* To temporarily disable the feature, just disable the script in inspector.
*/

[ExecuteAlways]
public class HideChildrenInHierarchy : MonoBehaviour
{
    void OnEnable()
    {
    #if UNITY_EDITOR
        HideAllChildren(true);
    #else
        Destroy(this);
    #endif
    }

    void OnDisable()
    {
    #if UNITY_EDITOR
        HideAllChildren(false);
    #endif
    }

    void HideAllChildren(bool b)
    {
        List<Transform> childTransforms = new List<Transform>();
        List<Component> components = new List<Component>();

        GetAllChildren(transform, childTransforms);
        GetAllComponents(childTransforms, components);

        if (components.Count > 0)
        {
            foreach (Component item in components)
            {
                if (b)
                {
                    item.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
                }
                else
                {
                    item.hideFlags = HideFlags.None;
                }
            }
        }
    }

    void GetAllChildren(Transform parent, List<Transform> list)
    {
        foreach (Transform item in parent)
        {
            list.Add(item);
            if (item.childCount > 0)
            {
                GetAllChildren(item, list);
            }
        }
    }

    void GetAllComponents(List<Transform> listOfTransforms, List<Component> listOfComponents)
    {
        foreach (Transform item in listOfTransforms)
        {
            listOfComponents.AddRange(item.gameObject.GetComponents(typeof(Component)));
        }
    }
}

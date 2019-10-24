using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Gets every child Transform attached to this Transform.
    /// </summary>
    public static Transform[] GetAllChildren(this Transform parent)
    {
        if (parent.childCount <= 0)
        {
            return null;
        }
        
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
            if (child.childCount > 0)
            {
                foreach (Transform subChild in GetAllChildren(child))
                {
                    children.Add(subChild);
                }
            }
        }
        return children.ToArray();
    }
}

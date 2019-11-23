using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvScrolling : MonoBehaviour
{

    [SerializeField] private Material scrollableMaterial = null;
    [SerializeField] private float scrollSpeed = 8f;
    [SerializeField] private Vector2 direction = new Vector2(1, 0);

    private Vector2 currentOffset = Vector2.zero;

    void Start()
    {
        currentOffset = scrollableMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        currentOffset += direction * scrollSpeed * Time.deltaTime;
        scrollableMaterial.SetTextureOffset("_MainTex", currentOffset);
    }
}

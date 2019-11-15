using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationLoop : MonoBehaviour
{
    public static GenerationLoop loop;

    public GameObject generator = null;
    public bool isGenerating = false;

    private GameObject currentGenerator = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private Vector3 startPos = Vector3.zero;
    [SerializeField] private Quaternion startRot = Quaternion.identity;

    private void Start()
    {
        loop = this;

        startPos = player.transform.position;
        startRot = player.transform.rotation;

        if (generator != null)
        {
            currentGenerator = Instantiate(generator, transform);
        }
    }

    private void Update()
    {
        if (isGenerating)
        {
            Destroy(currentGenerator);
            isGenerating = false;
        }

        if (currentGenerator == null)
        {
            player.transform.position = startPos;
            player.transform.rotation = startRot;
            currentGenerator = Instantiate(generator, transform);
        }
    }
}
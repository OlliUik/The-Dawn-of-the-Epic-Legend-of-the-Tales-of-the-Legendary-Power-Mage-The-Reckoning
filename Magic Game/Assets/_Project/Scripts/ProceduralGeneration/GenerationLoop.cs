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
    [SerializeField] private Vector3 startRot = Vector3.zero;

    private void Start()
    {
        loop = this;

        startPos = player.transform.position;
        startRot = player.GetComponent<ThirdPersonCamera>().lookDirection;

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
            player.GetComponent<PlayerMovement>().Teleport(startPos, startRot);
            currentGenerator = Instantiate(generator, transform);
        }
    }
}
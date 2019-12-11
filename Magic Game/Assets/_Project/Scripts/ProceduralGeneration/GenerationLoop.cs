using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationLoop : MonoBehaviour
{
    public GameObject generator = null;
    public bool isGenerating = false;

    [SerializeField] private GameObject player = null;
    private GameObject currentGenerator = null;
    private Vector3 startPos = Vector3.zero;
    private Vector3 startRot = Vector3.zero;

    private void Awake()
    {
        startPos = player.transform.position;
        startRot = player.GetComponent<ThirdPersonCamera>().lookDirection;

        if (generator != null)
        {
            currentGenerator = Instantiate(generator, transform);
            player.GetComponent<PlayerCore>().GetHUD().ActivateGeneration(currentGenerator.GetComponent<LevelGenerator>());
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
            player.GetComponent<PlayerCore>().GetHUD().ActivateGeneration(currentGenerator.GetComponent<LevelGenerator>());
        }
    }
}
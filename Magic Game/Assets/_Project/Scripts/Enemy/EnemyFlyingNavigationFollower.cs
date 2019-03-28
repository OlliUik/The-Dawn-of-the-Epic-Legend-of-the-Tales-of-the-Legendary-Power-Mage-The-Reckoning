using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingNavigationFollower : MonoBehaviour
{
    [SerializeField] private float desiredHeight = 10.0f;
    [SerializeField] private EnemyCore cEnemyCore = null;

    private float currentHeight = 0.0f;

    void Start()
    {
        currentHeight = transform.position.y;
    }

    void Update()
    {
        Vector3 currentPosition = cEnemyCore.navigation.transform.position;
        currentHeight = Mathf.Lerp(currentHeight, cEnemyCore.navigation.transform.position.y + desiredHeight, Time.deltaTime);
        currentPosition.y = currentHeight;

        transform.position = currentPosition;
    }
}

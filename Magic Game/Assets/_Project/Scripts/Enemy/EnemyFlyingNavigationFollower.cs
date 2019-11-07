using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingNavigationFollower : MonoBehaviour
{
    [SerializeField] private float desiredHeight = 10.0f;
    [SerializeField] private float maxFloatingRange = 1.2f;
    [SerializeField] private EnemyCore cEnemyCore = null;
    private bool atDesiredHieght = false;

    private float currentHeight = 0.0f;

    void Start()
    {
        currentHeight = transform.position.y;
    }

    void Update()
    {
        if(!atDesiredHieght)
        {
            TakeOff();
        } else
        {
            Floating();
        }
    }

    void TakeOff()
    {
        Vector3 currentPosition = cEnemyCore.cNavigation.transform.position;
        currentHeight = Mathf.Lerp(currentHeight, cEnemyCore.cNavigation.transform.position.y + desiredHeight, Time.deltaTime);
        currentPosition.y = currentHeight;

        transform.position = currentPosition;
        if(currentHeight.Equals(desiredHeight))
        {
            atDesiredHieght = true;
        }
    }

    void Floating()
    {
        Vector3 currentPosition = cEnemyCore.cNavigation.transform.position;
        float bRange = Random.Range(-maxFloatingRange, maxFloatingRange);
        currentHeight = Mathf.Lerp(currentHeight, cEnemyCore.cNavigation.transform.position.y + bRange, Time.deltaTime);
        currentPosition.y = currentHeight;

        transform.position = currentPosition;
    }
}

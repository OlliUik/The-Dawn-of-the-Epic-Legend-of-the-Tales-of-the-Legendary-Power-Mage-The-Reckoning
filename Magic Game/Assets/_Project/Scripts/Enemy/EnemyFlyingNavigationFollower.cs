using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingNavigationFollower : MonoBehaviour
{
    [SerializeField] private float desiredHeight = 10.0f;
    [SerializeField] private float maxFloatingRange = 1.5f;
    [SerializeField] private EnemyCore cEnemyCore = null;

    private bool atDesiredHieght = false;
    private bool floatToDesiredHeight = false;

    private float minFloatingRange = 0.5f;
    private float fRange = 0.0f;
    private float currentHeight = 0.0f;

    void Start()
    {
        currentHeight = cEnemyCore.cNavigation.transform.position.y;
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
        atDesiredHieght = MoveYPos(desiredHeight);
    }

    void Floating()
    {
        if(!floatToDesiredHeight)
        {
            fRange = Random.Range(minFloatingRange, maxFloatingRange);
            float yPos = desiredHeight + fRange;
            floatToDesiredHeight = MoveYPos(yPos);
        } else
        {
            floatToDesiredHeight = !MoveYPos(desiredHeight);
        } 
    }

    bool MoveYPos(float yPos)
    {
        Vector3 currentPosition = cEnemyCore.cNavigation.transform.position;
        currentHeight = Mathf.Lerp(currentHeight, yPos, Time.deltaTime);
        currentPosition.y = currentHeight;
        transform.position = currentPosition;

        if (Mathf.Abs(yPos - currentHeight) < 0.08)
        {
            return true;
        }

        return false;
    }
}

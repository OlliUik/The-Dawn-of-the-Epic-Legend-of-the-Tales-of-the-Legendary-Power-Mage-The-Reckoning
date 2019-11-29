using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyFlyingNavigationFollower : MonoBehaviour
{
    [SerializeField] private float desiredHeight = 10.0f;
    [SerializeField] private float maxFloatingRange = 1.5f;
    [SerializeField] private float maxMovingRange = 10f;
    [SerializeField] private float minMovingRange = 5f;
    [SerializeField] private EnemyCore cEnemyCore = null;

    private bool atDesiredHieght = false;
    private bool floatToDesiredHeight = false;
    private bool moveTowardPosition = false;
    private bool atDesiredPosition = false;

    private float minFloatingRange = 0.5f;
    private float fRange = 0.0f;
    private float currentHeight = 0.0f;

    private float mRange = 0.0f;
    private Vector3 mPos;
    private Vector3 mDirection;

    void Start()
    {
        currentHeight = cEnemyCore.cNavigation.transform.position.y;
    }

    void Update()
    {
        if(!atDesiredHieght)
        {
            TakeOff();
        } else if (!atDesiredPosition)
        {
            RandomMove();
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

    void RandomMove()
    {
        if (!moveTowardPosition)
        {
            mRange = Random.Range(minMovingRange, maxMovingRange);
            Vector3 currentPosition = cEnemyCore.cNavigation.transform.position;
            Vector3 ranV3 = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
            ranV3 =  ranV3 * mRange;
            ranV3.y = currentHeight;
            mPos = currentPosition + ranV3;

            moveTowardPosition = true;
        } else
        {
            MoveOnPlane(mPos);
        }
    }

    void MoveOnPlane(Vector3 pos)
    {
        Vector3 currentPosition = cEnemyCore.cNavigation.transform.position;
        currentPosition = Vector3.Lerp(currentPosition, pos, Time.deltaTime);
        currentPosition.y = currentHeight;
        transform.position = currentPosition;
        
        if (Vector3.Distance(currentPosition, pos) < 0.6)
        {
            atDesiredPosition = true;
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

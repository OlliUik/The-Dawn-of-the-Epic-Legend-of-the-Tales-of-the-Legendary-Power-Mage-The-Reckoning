using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectingEnemy : MonoBehaviour
{
    //call when pushing enemy by push/knockback spell
    public void PushBack(float distance)
    {
        transform.position = transform.position + (-transform.forward * distance);
    }

    //Call to teleport enemy
    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }
}

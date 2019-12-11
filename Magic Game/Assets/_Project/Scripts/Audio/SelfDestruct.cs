using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

  
// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            Destroy(gameObject, 4); //5 is how many seconds you want before the object deletes itself
 }
    }
}

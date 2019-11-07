using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomEveryXSeconds : MonoBehaviour
{
    public float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time == 5.0f)
        {
            //this.GetComponent<SfxManager>().sfxManager.Play(1, 2);
            time = 0.0f;

        }

    }

  
}

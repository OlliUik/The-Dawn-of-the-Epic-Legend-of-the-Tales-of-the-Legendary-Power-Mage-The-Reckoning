using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClipRandomizer))]
public class MusicEventHandler : MonoBehaviour
{
    //[SerializeField] private GameObject player = null;
    //[SerializeField] private Vector2 volumeRange = Vector2.up;
    //[SerializeField] private float fadeTime = 2.0f;

    //private bool enemySeesPlayer = false;
    private AudioSource source = null;
    //private EnemyCore.EState[] enemyStates = { EnemyCore.EState.ATTACK, EnemyCore.EState.CASTING, EnemyCore.EState.ESCAPE };

    void Start()
    {
        source = GetComponent<AudioSource>();
        //InvokeRepeating("Check", 0.0f, 1.0f);
        GetComponent<AudioClipRandomizer>().Randomize();
        source.Play();
        StartCoroutine(WaitAudio());
    }

    //void Update()
    //{
    //    if (enemySeesPlayer)
    //    {
    //        if (source.volume < volumeRange.y)
    //        {
    //            source.volume += 1 / fadeTime * Time.deltaTime;
    //        }
    //    }
    //    else
    //    {
    //        if (source.volume > volumeRange.x)
    //        {
    //            source.volume -= 1 / fadeTime * Time.deltaTime;
    //        }
    //    }
    //}

    //void Check()
    //{
    //    enemySeesPlayer = false;

    //    foreach (GameObject item in GlobalVariables.teamBadBoys)
    //    {
    //        if (item.tag == "Enemy")
    //        {
    //            EnemyCore ec = item.GetComponent<EnemyCore>();

    //            if (ec.cVision.bCanSeeTarget)
    //            {
    //                //if (ec.currentState == EnemyCore.EState.ATTACK
    //                //    || ec.currentState == EnemyCore.EState.CASTING
    //                //    || ec.currentState == EnemyCore.EState.ESCAPE)

    //                foreach (EnemyCore.EState state in enemyStates)
    //                {
    //                    if (state == ec.currentState)
    //                    {
    //                        if (ec.cVision.targetGO == player)
    //                        {
    //                            enemySeesPlayer = true;
    //                            return;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private IEnumerator WaitAudio()
    {
        yield return new WaitForSecondsRealtime(source.clip.length);
        GetComponent<AudioClipRandomizer>().Randomize();
        source.Play();
        StartCoroutine(WaitAudio());
    }
}

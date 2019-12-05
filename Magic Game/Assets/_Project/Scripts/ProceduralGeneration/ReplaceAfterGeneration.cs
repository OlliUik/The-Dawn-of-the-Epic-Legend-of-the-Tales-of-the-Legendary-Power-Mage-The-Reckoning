using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Segment))]
public class ReplaceAfterGeneration : MonoBehaviour
{
    [SerializeField] private GameObject replaceWith = null;

    private LevelGenerator generator = null;

    private void Start()
    {
        generator = transform.parent.gameObject.GetComponent<LevelGenerator>();

        if (generator != null && replaceWith != null)
        {
            Debug.Log("Starting coroutine WaitForLevelGeneration.");
            StartCoroutine(WaitForLevelGeneration());
        }
        else
        {
            Debug.LogError(this + " Couldn't find LevelGenerator script or Replace With object!");
        }
    }

    private IEnumerator WaitForLevelGeneration()
    {
        while (!generator.isDone)
        {
            yield return null;
        }
        GameObject replace = Instantiate(replaceWith, transform.position, transform.rotation, transform.parent);
        Segment originalSegment = GetComponent<Segment>();
        Segment replaceSegment = replace.GetComponent<Segment>();

        for (int i = 0; i < originalSegment.doorways.Length; i++)
        {
            replaceSegment.doorways[i].gameObject.SetActive(originalSegment.doorways[i].gameObject.activeInHierarchy);
        }

        //Destroy(gameObject);
        gameObject.SetActive(false);
        yield break;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreAnimation : MonoBehaviour, AnimationElement
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private float maxTime = 3;
    private float score = 0.0f;
    private float counter = 0.0f;
    private int dc = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = 0.ToString();
        counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        score = ScoreSystem.scoreSystem.score;
        dc = (int) (score / (maxTime * (1.0f / Time.deltaTime)));
        
        if (counter != score)
        {
            if (dc < 1)
            {
                dc = 1;
                if ((counter + dc) > score)
                {
                    counter = score;
                }
                else
                {
                    counter += dc;
                }
            } else
            {
                if ((counter + dc) > score)
                {
                    counter = score;
                }
                else
                {
                    counter += dc;
                }
            }
        }
    }

    public IEnumerator Animator()
    {
        scoreText.text = counter.ToString();
        yield return new WaitForFixedUpdate();
    }
}

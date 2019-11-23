using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankDetailDisplay : MonoBehaviour
{
    [SerializeField] private Text rankText = null;
    private ScoreSystem scoreSystem = null;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = ScoreSystem.scoreSystem;
        DisplayRankText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayRankText()
    {
        rankText.text = scoreSystem.rankInfo.getRankText();
    }
}

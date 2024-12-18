using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    int currentScore;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Animator anim;


    public void IncrementScore()
    {
        anim.SetTrigger("Scoring");
        currentScore++;
        scoreText.text = currentScore.ToString();
    }

    public void SuperIncrementScore()
    {
        anim.SetTrigger("Scoring");
        currentScore += 3;
        scoreText.text = currentScore.ToString();
    }

    public int GetScore()
    {
        return currentScore;
    }


}

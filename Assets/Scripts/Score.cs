using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Text scoreText;
    private int totalScore;

    public int scoreMultilplier;

    private void FixedUpdate()
    {
        totalScore += scoreMultilplier;
        scoreText.text = totalScore.ToString();

    }
}

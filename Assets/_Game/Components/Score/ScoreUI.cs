using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    Score score;
    [SerializeField] TMP_Text textElement;
    [SerializeField] Animation scoreAnimation;
    private void Start()
    {
        score = FindObjectOfType<Score>();
        score.onScoreChange += OnScoreChange;
    }
    private void OnScoreChange()
    {
        textElement.text = "[" + score.CurrentScore + "]";
        scoreAnimation.Rewind();
        scoreAnimation.Play();
    }
}

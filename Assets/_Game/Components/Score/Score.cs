using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] ScriptableScore scoreSheet;
    public delegate void OnScoreChange();
    public event OnScoreChange onScoreChange;
    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;
    }
    private void Start()
    {
        currentScore = 0;
        OnGameEnd();
    }
    public void Modify(int amount)
    {
        currentScore += amount;
        onScoreChange?.Invoke();
    }
    private void OnGameEnd()
    {
        scoreSheet.Add(currentScore);
    }
}

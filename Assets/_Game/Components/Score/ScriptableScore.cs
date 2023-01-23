using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Sheet", menuName = "ScriptableObjects/Score/ScriptableScore")]
public class ScriptableScore : ScriptableObject
{
    [SerializeField] List<int> scores = new List<int>();

    public void Add(int score)
    {
        scores.Add(score);
    }
    public void SortScores()
    {
        scores.Sort((a, b) => a.CompareTo(b));
    }
    public int[] GetTopScores(int size)
    {
        SortScores();
        return scores.ToArray()[0..size];
    }
}

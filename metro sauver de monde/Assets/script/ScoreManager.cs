using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public static int currentScore = 0;
    public static List<int> leaderboard = new List<int>();
    private static readonly int maxLeaderboardEntries = 10;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
            ResetScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ResetScore();
    }
    
    public static void ResetScore()
    {
        currentScore = 0;
    }
    
    public static void AddScore(float impactVelocity, int level)
    {
        // Score = impact velocity * (level / 10)
        int score = Mathf.RoundToInt(impactVelocity * (level / 10f));
        currentScore += score;
        Debug.Log($"Enemy killed! Impact: {impactVelocity:F2}, Level: {level}, Score added: {score}, Total: {currentScore}");
    }
    
    public static void SaveScore()
    {
        if (currentScore <= 0) return;
        
        leaderboard.Add(currentScore);
        leaderboard.Sort((a, b) => b.CompareTo(a)); // Sort descending
        
        if (leaderboard.Count > maxLeaderboardEntries)
            leaderboard.RemoveAt(leaderboard.Count - 1);
        
        SaveLeaderboard();
        Debug.Log($"Score saved: {currentScore}");
    }
    
    private static void LoadLeaderboard()
    {
        leaderboard.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            string key = "LeaderboardScore_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                int score = PlayerPrefs.GetInt(key);
                leaderboard.Add(score);
            }
        }
    }
    
    private static void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboard.Count && i < maxLeaderboardEntries; i++)
        {
            string key = "LeaderboardScore_" + i;
            PlayerPrefs.SetInt(key, leaderboard[i]);
        }
        PlayerPrefs.Save();
    }
    
    public static List<int> GetLeaderboard()
    {
        return new List<int>(leaderboard);
    }
    
    public static void ResetLeaderboard()
    {
        leaderboard.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            string key = "LeaderboardScore_" + i;
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
        Debug.Log("Leaderboard reset!");
    }
}

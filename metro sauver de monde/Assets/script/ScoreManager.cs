using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [System.Serializable]
    public struct ScoreEntry
    {
        public int score;
        public string name;

        public ScoreEntry(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }
    
    public static int currentScore = 0;
    public static List<ScoreEntry> leaderboard = new List<ScoreEntry>();
    private static readonly int maxLeaderboardEntries = 10;
    private static int pendingScore = 0;
    private static int pendingInsertIndex = -1;
    private static bool hasPendingScore = false;
    
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
        
        if (BeginSaveScore())
        {
            FinalizePendingScore("Rookie");
        }
    }

    public static bool BeginSaveScore()
    {
        if (currentScore <= 0) return false;

        int insertIndex = GetInsertIndex(currentScore);
        if (insertIndex < 0) return false;

        pendingScore = currentScore;
        pendingInsertIndex = insertIndex;
        hasPendingScore = true;
        return true;
    }

    public static void FinalizePendingScore(string name)
    {
        if (!hasPendingScore) return;

        string finalName = string.IsNullOrEmpty(name) ? "Rookie" : name;
        leaderboard.Insert(pendingInsertIndex, new ScoreEntry(pendingScore, finalName));

        if (leaderboard.Count > maxLeaderboardEntries)
            leaderboard.RemoveAt(leaderboard.Count - 1);

        SaveLeaderboard();
        Debug.Log($"Score saved: {pendingScore} ({finalName})");
        ClearPendingScore();
    }

    public static void ClearPendingScore()
    {
        pendingScore = 0;
        pendingInsertIndex = -1;
        hasPendingScore = false;
    }

    private static int GetInsertIndex(int score)
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (score > leaderboard[i].score)
                return i;
        }

        if (leaderboard.Count < maxLeaderboardEntries)
            return leaderboard.Count;

        return -1;
    }
    
    private static void LoadLeaderboard()
    {
        leaderboard.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            string scoreKey = "LeaderboardScore_" + i;
            if (PlayerPrefs.HasKey(scoreKey))
            {
                int score = PlayerPrefs.GetInt(scoreKey);
                string nameKey = "LeaderboardName_" + i;
                string name = PlayerPrefs.HasKey(nameKey) ? PlayerPrefs.GetString(nameKey) : "Player";
                leaderboard.Add(new ScoreEntry(score, name));
            }
        }
    }
    
    private static void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboard.Count && i < maxLeaderboardEntries; i++)
        {
            string scoreKey = "LeaderboardScore_" + i;
            string nameKey = "LeaderboardName_" + i;
            PlayerPrefs.SetInt(scoreKey, leaderboard[i].score);
            PlayerPrefs.SetString(nameKey, string.IsNullOrEmpty(leaderboard[i].name) ? "Player" : leaderboard[i].name);
        }
        PlayerPrefs.Save();
    }
    
    public static List<ScoreEntry> GetLeaderboard()
    {
        return new List<ScoreEntry>(leaderboard);
    }
    
    public static void ResetLeaderboard()
    {
        leaderboard.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            string scoreKey = "LeaderboardScore_" + i;
            string nameKey = "LeaderboardName_" + i;
            PlayerPrefs.DeleteKey(scoreKey);
            PlayerPrefs.DeleteKey(nameKey);
        }
        PlayerPrefs.Save();
        Debug.Log("Leaderboard reset!");
    }
}

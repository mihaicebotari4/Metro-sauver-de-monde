using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public class DeathScreenUI : MonoBehaviour
{
    public static DeathScreenUI instance;
    
    public GameObject deathPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI leaderboardText;
    public Button retryButton;
    public Button quitButton;
    public Button resetScoresButton;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
            
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }
    
    void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        if (resetScoresButton != null)
            resetScoresButton.onClick.AddListener(ResetScores);
    }
    
    public void ShowDeathScreen()
    {
        Time.timeScale = 0f; // Pause game
        
        // Save score to leaderboard
        ScoreManager.SaveScore();
        
        // Display final score
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {ScoreManager.currentScore}";
        
        // Display leaderboard
        if (leaderboardText != null)
        {
            string leaderboardDisplay = "LEADERBOARD\n\n";
            List<int> board = ScoreManager.GetLeaderboard();
            
            for (int i = 0; i < board.Count && i < 10; i++)
            {
                leaderboardDisplay += $"{i + 1}. {board[i]}\n";
            }
            
            leaderboardText.text = leaderboardDisplay;
        }
        
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }
    
    public void Retry()
    {
        Time.timeScale = 1f; // Resume time
        logic.retryRequested = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    
    public void ResetScores()
    {
        Time.timeScale = 1f;
        ScoreManager.ResetLeaderboard();
        
        // Refresh leaderboard display
        if (leaderboardText != null)
        {
            leaderboardText.text = "LEADERBOARD\n\nScores Reset!";
        }
        
        Debug.Log("All scores reset!");
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1f; // Resume time
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

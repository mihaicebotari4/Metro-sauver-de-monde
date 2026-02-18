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
    public GameObject namePanel;
    public TMP_Dropdown nameDropdown;
    public Button confirmNameButton;
    public List<string> nameOptions = new List<string> { "Rookie", "Atlas", "Comet", "Nova", "Echo", "Bolt", "Metro", "Pulse" };
    public string defaultName = "Rookie";
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
            
        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (namePanel != null)
            namePanel.SetActive(false);
    }
    
    void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        if (resetScoresButton != null)
            resetScoresButton.onClick.AddListener(ResetScores);

        if (confirmNameButton != null)
            confirmNameButton.onClick.AddListener(ConfirmName);
    }

    void Update()
    {
        // Toggle retry button with O key
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (retryButton != null)
                retryButton.gameObject.SetActive(!retryButton.gameObject.activeSelf);
        }

        // Toggle quit button with K key
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (quitButton != null)
                quitButton.gameObject.SetActive(!quitButton.gameObject.activeSelf);
        }

        // Toggle reset scores button with L key
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (resetScoresButton != null)
                resetScoresButton.gameObject.SetActive(!resetScoresButton.gameObject.activeSelf);
        }
    }
    
    public void ShowDeathScreen()
    {
        Time.timeScale = 0f; // Pause game

        bool needsName = ScoreManager.BeginSaveScore();
        
        // Display final score
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {ScoreManager.currentScore}";
        
        if (needsName)
        {
            if (nameDropdown != null)
                PopulateNameOptions();

            if (namePanel != null)
            {
                namePanel.SetActive(true);
            }
            else
            {
                ScoreManager.FinalizePendingScore(GetDefaultName());
            }
        }
        else
        {
            ScoreManager.ClearPendingScore();
        }

        UpdateLeaderboardText();
        
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }
    
    public void Retry()
    {
        Time.timeScale = 1f; // Resume time
        ScoreManager.ClearPendingScore();
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

    public void ConfirmName()
    {
        ScoreManager.FinalizePendingScore(GetSelectedName());

        if (namePanel != null)
            namePanel.SetActive(false);

        UpdateLeaderboardText();
    }

    private void PopulateNameOptions()
    {
        if (nameDropdown == null)
            return;

        List<string> options = new List<string>();
        if (nameOptions != null && nameOptions.Count > 0)
            options.AddRange(nameOptions);
        else
            options.Add(defaultName);

        nameDropdown.ClearOptions();
        nameDropdown.AddOptions(options);
    }

    private string GetSelectedName()
    {
        if (nameDropdown != null && nameDropdown.options.Count > 0)
            return nameDropdown.options[nameDropdown.value].text;

        return GetDefaultName();
    }

    private string GetDefaultName()
    {
        if (nameOptions != null && nameOptions.Count > 0)
            return nameOptions[0];

        return defaultName;
    }

    private void UpdateLeaderboardText()
    {
        if (leaderboardText == null)
            return;

        string leaderboardDisplay = "LEADERBOARD\n\n";
        List<ScoreManager.ScoreEntry> board = ScoreManager.GetLeaderboard();

        if (board.Count == 0)
        {
            leaderboardDisplay += "No scores yet";
        }
        else
        {
            for (int i = 0; i < board.Count && i < 10; i++)
            {
                string name = string.IsNullOrEmpty(board[i].name) ? "Player" : board[i].name;
                leaderboardDisplay += $"{i + 1}. {name} - {board[i].score}\n";
            }
        }

        leaderboardText.text = leaderboardDisplay;
    }
}

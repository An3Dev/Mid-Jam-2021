using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnScoreChanges(int score)
    {
        // change score UI
    }

    public void OnLivesChanges(int lives)
    {
        if (lives <= 0 && !isGameOver)
        {
            Debug.Log("Game Over");
            isGameOver = true;

            // change lives UI
            UIManager.Instance.OnGameOver();
            ObjectSpawner.Instance.OnGameOver();
            Stats.SaveHighScore();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }
}

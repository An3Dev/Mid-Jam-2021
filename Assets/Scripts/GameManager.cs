using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver;

    bool isPaused = false;

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

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                UIManager.Instance.OnPause();
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                UIManager.Instance.OnClickUnpause();
                isPaused = false;
            }
        }
    }

    public void OnUnpause()
    {
        Time.timeScale = 1;
        isPaused = false;
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
        Time.timeScale = 1;

        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");
    }
}

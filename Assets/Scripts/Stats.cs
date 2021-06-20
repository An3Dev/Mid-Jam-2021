using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance;

    public static float paintBallSpeed = 9;
    public static int minContainerColorChangePasses = 2, maxContainerColorChangePasses = 6;


    private int defaultScore = 0, defaultLives = 3;
    public static int score = 0;
    public static int lives = 3;
    public static int totalLives = 3;

    public static int highScore = 0;

    public static float startSpeed = 2;
    public static float maxSpeed = 4;
    public static int scoreAtMaxSpeed = 30;

    public static float paintGunFiringGap = 0.5f;

    public static float timeBetweenSpawns = 2;

    public static float containerChangeSpawnDelay = 4;

    public static float maxViewportObjectPos = 0.7f, minViewportObjectPos = 0.3f;

    public static float maxDistanceFromCenterScore = 30;

    public AnimationCurve distanceFromCenter;


    // Player prefs
    const string highScoreKey = "HighScore";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        score = defaultScore;
        lives = defaultLives;

        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    public static void IncreaseScore()
    {
        score++;
        //GameManager.Instance.OnScoreChanges(score);
        UIManager.Instance.OnScoreChanged(score);
    }

    public static void DecreaseLives(int amount)
    {
        lives -= amount;
        GameManager.Instance.OnLivesChanges(lives);
        UIManager.Instance.OnLivesChanged(lives);
    }

    public static float GetCurrentSpawnTimeGap()
    {
        return timeBetweenSpawns;
    }

    public static void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
        PlayerPrefs.Save();
    }

    public static float GetCurrentObjectSpeed()
    {
        return Mathf.Lerp(startSpeed, maxSpeed, (float)score / scoreAtMaxSpeed);
    }

    public float GetMaxDistanceFromCenterMultiplier()
    {
        return distanceFromCenter.Evaluate(score / maxDistanceFromCenterScore);
    }
}

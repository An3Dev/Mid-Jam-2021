using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform colorSelectionContainer;
    public GameObject colorSelectionButtonPrefab;
    public TextMeshProUGUI scoreText, livesText, highScoreText;
    public Animator canvasAnimator;

    public PaintGun paintGun;

    int selectedColorIndex;
    ColorButton[] colorButtonsArray;

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
        colorButtonsArray = new ColorButton[Colors.Instance.GetTotalNumColors()];
        for (int i = 0; i < colorButtonsArray.Length; i++)
        {
            colorButtonsArray[i] = Instantiate(colorSelectionButtonPrefab, colorSelectionContainer).GetComponent<ColorButton>();
            colorButtonsArray[i].SetColor(i);
        }

        livesText.text = "Lives: " + Stats.lives;
        highScoreText.text = "Best: " + Stats.highScore;
    }

    public void OnGameOver()
    {
        canvasAnimator.SetTrigger("GameOver");
    }

    public void OnScoreChanged(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void OnLivesChanged(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void OnColorButtonPressed(int colorIndex)
    {
        selectedColorIndex = colorIndex;
        UpdateColorButtonSelection();
        paintGun.SetCurrentColor(selectedColorIndex);
    }

    public void OnColorKeybindPressed(int colorIndex)
    {
        selectedColorIndex = colorIndex;

        // update UI
        UpdateColorButtonSelection();
    }

    void UpdateColorButtonSelection()
    {
        for(int i = 0; i < colorButtonsArray.Length; i++)
        {
            if (colorButtonsArray[i].GetColor() == selectedColorIndex)
            {
                colorButtonsArray[i].OnSelect();
            } else
            {
                colorButtonsArray[i].OnDeselect();
            }
        }
    }
}

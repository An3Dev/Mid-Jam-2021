using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorButton : MonoBehaviour
{
    Image image;
    RectTransform rectTransform;
    int colorIndex;

    bool selected;
    Vector2 selectedSize = new Vector2(40, 40);
    Vector2 regularSize = new Vector2(30, 30);

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = regularSize;
    }

    public void SetColor(int index)
    {
        colorIndex = index;
        image.color = Colors.Instance.GetColorByIndex(colorIndex);
    }

    public int GetColor()
    {
        return colorIndex;
    }

    public void OnClick()
    {
        //OnSelect();
        UIManager.Instance.OnColorButtonPressed(colorIndex);
    }

    public void OnSelect()
    {
        selected = true;
        rectTransform.sizeDelta = selectedSize;
    }
    public void OnDeselect()
    {
        selected = false;
        rectTransform.sizeDelta = regularSize;
    }
}

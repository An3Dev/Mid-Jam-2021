using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private int colorIndex;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public int GetColorIndex()
    {
        return colorIndex;
    }

    public void SetColor(int index)
    {
        spriteRenderer.color = Colors.Instance.GetColorByIndex(index);
        colorIndex = index;
    }

}

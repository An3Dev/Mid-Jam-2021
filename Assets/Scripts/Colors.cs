using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public static Colors Instance;
    public Color[] colors;

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
    }
    public int GetTotalNumColors()
    {
        return colors.Length;
    }

    public int GetRandomColorIndex()
    {
        return Random.Range(0, colors.Length);
    }

    public int GetRandomColorIndexExcludingColor(int color)
    {
        int i = Random.Range(0, colors.Length);
        while(i == color)
        {
            i = Random.Range(0, colors.Length);
        }

        return i;
    }

    public Color GetColorByIndex(int index)
    {
        return colors[index];
    }


}

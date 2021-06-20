using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private int colorIndex;
    SpriteRenderer spriteRenderer;
    Material mat;

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out spriteRenderer);
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        mat = spriteRenderer.material;
    }
    public int GetColorIndex()
    {
        return colorIndex;
    }

    public void SetColor(int index)
    {
        //spriteRenderer.color = Colors.Instance.GetColorByIndex(index);
        Material newMat = mat;
        Color color = Colors.Instance.GetColorByIndex(index);
        newMat.color = color;
        Vector4 v = new Vector4(color.r, color.g, color.b, color.a) * 2f;
        newMat.SetColor("_EmissionColor", v);
        spriteRenderer.material = newMat;
        colorIndex = index;
    }

}

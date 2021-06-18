using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    ColorObject colorObject;
    private void Awake()
    {
        colorObject = GetComponent<ColorObject>();
    }

    private void OnEnable()
    {
        //Debug.Log("Play container animation");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ObjectToContain"))
        {
            // if the object is the same color, then play a correct animation and particle system
            // else, play the incorrect particle system and animation

            if (collision.gameObject.GetComponent<ColorObject>().GetColorIndex() == colorObject.GetColorIndex())
            {
                Debug.Log("Correct color");
                collision.gameObject.GetComponent<ObjectToContain>().OnCorrect();
            }
            else
            {
                Debug.Log("Incorrect color");
                collision.gameObject.GetComponent<ObjectToContain>().OnIncorrect();
            }
        }
    }
}

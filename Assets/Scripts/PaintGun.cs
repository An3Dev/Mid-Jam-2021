using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
    public Transform pivot, barrelEnd;
    public GameObject paintBallPrefab;
    public float maxRot = 50;

    Camera cam;

    int startPaintBallAmount = 20;
    PaintBall[] paintBalls;

    int selectedColorIndex = 0;

    private void Awake()
    {
        paintBalls = new PaintBall[startPaintBallAmount];
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        for(int i = 0; i < startPaintBallAmount; i++)
        {
            GameObject paintBall = Instantiate(paintBallPrefab, barrelEnd);
            paintBalls[i] = paintBall.GetComponent<PaintBall>();
            paintBall.SetActive(false);
        }
    }

    void RandomizePaintBall(PaintBall ball)
    {
        ball.GetColorObject().SetColor(Colors.Instance.GetRandomColorIndex());
    }

    int GetSpawnableBallIndex()
    {
        for (int i = 0; i < paintBalls.Length; i++)
        {
            if (!paintBalls[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        Debug.Log("Not enough paint balls");
        return 0;
    }

    private void Update()
    {
        // make gun look toward the cursor
        Vector3 cursorWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPos.z = 0;

        pivot.up = (cursorWorldPos - pivot.position).normalized;
        ClampGunRotation();

        //Debug.Log(cursorWorldPos);

        // if user clicks on number 1 through 6, then select a color;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedColorIndex = 0;
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedColorIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedColorIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedColorIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedColorIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedColorIndex = 5;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot ball");
            PaintBall ball = paintBalls[GetSpawnableBallIndex()];
            ball.transform.parent = pivot;
            ball.transform.localPosition = Vector3.zero;
            ball.gameObject.SetActive(true);
            ball.GetColorObject().SetColor(selectedColorIndex);
            //RandomizePaintBall(ball);
            ball.SetMovement(barrelEnd.up, ObjectProgression.Instance.paintBallSpeed);

            // play shoot effect from barrel
            // play shoot sound
        }
    }

    void ClampGunRotation()
    {
        if (pivot.localEulerAngles.z > maxRot && pivot.localEulerAngles.z < 180)
        {
            pivot.localRotation = Quaternion.Euler(0, 0, maxRot);
        }
        else if (pivot.localEulerAngles.z < 360 - maxRot && pivot.localEulerAngles.z > 180)
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 360 - maxRot);
        }
    }

}

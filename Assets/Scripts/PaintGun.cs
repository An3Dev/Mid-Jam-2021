using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaintGun : MonoBehaviour
{
    public Transform pivot, barrelEnd;
    public GameObject paintBallPrefab;
    public float maxRot = 50;
    public ColorObject[] colorIndicators;

    public AudioClip shootSound;

    Camera cam;

    int startPaintBallAmount = 20;
    PaintBall[] paintBalls;


    int selectedColorIndex = 0;
    int totalColors;

    float lastFireTime = -5;

    public ParticleSystemRenderer psr;
    public ParticleSystem ps;

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

    private void Start()
    {
        totalColors = Colors.Instance.GetTotalNumColors();
        selectedColorIndex = 0;
        UpdateColorUI();
        UpdateColorIndicators();      
    }

    void UpdateColorIndicators()
    {
        for(int i = 0; i < colorIndicators.Length; i++)
        {
            colorIndicators[i].SetColor(selectedColorIndex);
        }
    }

    public void SetCurrentColor(int i)
    {
        selectedColorIndex = i;
        UpdateColorIndicators();
    }

    public void UpdateColorUI()
    {
        UIManager.Instance.OnColorKeybindPressed(selectedColorIndex);
        UpdateColorIndicators();
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

        if (Time.timeScale != 0)
        {
            // make gun look toward the cursor
            Vector3 cursorWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPos.z = 0;

            pivot.up = (cursorWorldPos - pivot.position).normalized;
        }
        
        ClampGunRotation();

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedColorIndex--;
            if (selectedColorIndex < 0) selectedColorIndex = 0;
            UpdateColorUI();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedColorIndex++;
            if (selectedColorIndex > totalColors - 1) selectedColorIndex = totalColors- 1;
            UpdateColorUI();
        }
        // if user clicks on number 1 through 6, then select a color;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedColorIndex = 0;
            UpdateColorUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedColorIndex = 1;
            UpdateColorUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedColorIndex = 2;
            UpdateColorUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedColorIndex = 3;
            UpdateColorUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedColorIndex = 4;
            UpdateColorUI();
        }

        if (Time.timeSinceLevelLoad - lastFireTime > Stats.paintGunFiringGap && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Not over buttons");
            if (Input.GetMouseButtonDown(0))
            {
                lastFireTime = Time.timeSinceLevelLoad;
                PaintBall ball = paintBalls[GetSpawnableBallIndex()];
                ball.transform.parent = barrelEnd;
                ball.transform.localPosition = Vector3.zero;
                ball.gameObject.SetActive(true);
                ball.GetColorObject().SetColor(selectedColorIndex);
                //RandomizePaintBall(ball);
                ball.SetMovement(barrelEnd.up, Stats.paintBallSpeed);

                AudioManager.Instance.PlayOneShotSFX(shootSound);

                Material newMat = psr.material;
                Color color = Colors.Instance.GetColorByIndex(selectedColorIndex);
                newMat.color = color;
                Vector4 v = new Vector4(color.r, color.g, color.b, color.a) * 2f;
                newMat.SetColor("_EmissionColor", v);
                psr.material = newMat;

                ps.Play();       
            }
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

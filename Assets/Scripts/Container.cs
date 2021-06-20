using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public static Container Instance;

    public AudioClip absorbSFX, deflectSFX, changeColorSFX, hitDeadzoneSFX;

    ColorObject colorObject;

    int numObjectsForContainerChange = 0;
    int newNumObjectsForContainerChange = 2;

    int numObjectsThatPassedForThisColor = 0;

    ObjectToContain lastObject;
    float lastTriggerTime = 0;

    Rigidbody2D rb;
    Animator animator;

    float input = 0;
    float speed = 2;
    Camera cam;

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

        colorObject = GetComponent<ColorObject>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        //Debug.Log("Play container animation");
    }

    private void Update()
    {
        input = Input.GetAxis("Vertical");
        
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.up * input * speed;

        float yPos = transform.position.y;
        if (yPos <= ObjectSpawner.minYPos && input < 0)
        {
            //transform.position = transform.position;
            rb.velocity = Vector3.zero;

        }
        else if (yPos > ObjectSpawner.maxYPos && input > 0)
        {
            //transform.position = transform.position;
            rb.velocity = Vector3.zero;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ObjectToContain"))
        {
            // if triggered by the same exact object
            if (lastObject != null && lastObject.gameObject == collision.gameObject && Time.timeSinceLevelLoad - lastTriggerTime < 1f)
            {
                Debug.Log("Same object");
                return;
            }

            lastTriggerTime = Time.timeSinceLevelLoad;
            lastObject = collision.gameObject.GetComponent<ObjectToContain>();

            // if the object is the same color, then play a correct animation and particle system
            // else, play the incorrect particle system and animation

            if (lastObject.GetColorObject().GetColorIndex() == colorObject.GetColorIndex())
            {
                lastObject.OnCorrect();
                Stats.IncreaseScore();
                animator.SetTrigger("Absorb");
                AudioManager.Instance.PlayOneShotSFX(absorbSFX);

            }
            else
            {
                lastObject.OnIncorrect();
                Stats.DecreaseLives(1);
                animator.SetTrigger("Deflect");
                AudioManager.Instance.PlayOneShotSFX(deflectSFX);
            }

            //numObjectsThatPassedForThisColor++;
            //Debug.Log(numObjectsThatPassedForThisColor);

            // if the container was passed by the required amount of objects to change color
            if (lastObject.GetIsContainerTrigger())
            {
                //ChangeColor();
                // play retreat animation
                animator.SetTrigger("ChangeColor");
                AudioManager.Instance.PlayOneShotSFX(changeColorSFX);
            }
        }
    }

    public void OnHitDeadzone(GameObject ballGO)
    {
        ObjectToContain ball = ballGO.GetComponent<ObjectToContain>();

        ball.OnIncorrect();
        Stats.DecreaseLives(1);
        animator.SetTrigger("HitDeadzone");

        AudioManager.Instance.PlayOneShotSFX(hitDeadzoneSFX);
    }

    public ColorObject GetColorObject()
    {
        return colorObject;
    }

    // is called by animation
    public void ChangeColor()
    {
        colorObject.SetColor(Colors.Instance.GetRandomColorIndexExcludingColor(colorObject.GetColorIndex()));
    }
}

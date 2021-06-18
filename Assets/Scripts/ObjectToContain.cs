using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToContain : MonoBehaviour
{
    ColorObject colorObject;
    Rigidbody2D rb;

    bool move = false;
    float speed;
    Vector2 direction;

    float shrinkTime = 0.5f;

    private void Awake()
    {
        colorObject = GetComponent<ColorObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (move)
        {
            rb.velocity = direction * speed;
        }
    }

    public ColorObject GetColorObject()
    {
        return colorObject;
    }

    public void SetMove(bool move)
    {
        this.move = move;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PaintBall"))
        {
            Debug.Log("Hit by paint ball");

            // set this object color to the color of the paint ball
            colorObject.SetColor(collision.collider.GetComponent<ColorObject>().GetColorIndex());
        }
    }

    public void OnCorrect()
    {
        Debug.Log("Correct");
        Shrink();
    }

    public void OnIncorrect()
    {
        Debug.Log("Incorrect");
        Shrink();
    }

    public void Shrink()
    {
        StartCoroutine("ShrinkObject");
    }

    IEnumerator ShrinkObject()
    {
        float step = 0;

        while (step < 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector2.zero, step);
            step += Time.deltaTime / shrinkTime;
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
        transform.localScale = Vector3.one;

        StopCoroutine("ShrinkObject");
    }
}

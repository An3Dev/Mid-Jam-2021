using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    ColorObject colorObject;
    Rigidbody2D rb;

    Vector2 direction;
    float speed;
    private void Awake()
    {
        colorObject = GetComponent<ColorObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    public ColorObject GetColorObject()
    {
        return colorObject;
    }

    public void SetMovement(Vector2 direction, float speed)
    {
        transform.parent = null;
        this.direction = direction;
        this.speed = speed;

        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("PaintGun"))
        {
            Debug.Log("Paint ball collided");
            gameObject.SetActive(false);
        }
    }
}

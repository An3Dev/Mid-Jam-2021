using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    ColorObject colorObject;
    Rigidbody2D rb;

    Vector2 direction;
    float speed;

    public AudioClip explodeSoundEffect;
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

        // disables object after 6 seconds
        CancelInvoke(nameof(DisableSelf));
        Invoke(nameof(DisableSelf), 6);
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PaintGun"))
        {
            AudioManager.Instance.PlayOneShotSFX(explodeSoundEffect);
            gameObject.SetActive(false);
        }
    }
}

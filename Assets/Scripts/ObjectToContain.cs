using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToContain : MonoBehaviour
{
    ColorObject colorObject;
    Rigidbody2D rb;
    Collider2D collider;

    bool move = false;
    float speed;
    Vector2 direction;

    float shrinkTime = 0.5f;
    bool isContainerColorTrigger = false;

    private void Awake()
    {
        colorObject = GetComponent<ColorObject>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        rb.Sleep();
        rb.WakeUp();
        CancelInvoke(nameof(DisableObject));

    }

    private void FixedUpdate()
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

    public void SetIsContainerTrigger(bool isTrigger)
    {
        isContainerColorTrigger = isTrigger;
    }

    public bool GetIsContainerTrigger()
    {
        return isContainerColorTrigger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintBall"))
        {
            // set this object color to the color of the paint ball
            colorObject.SetColor(collision.GetComponent<ColorObject>().GetColorIndex());
            
            SpawnParticleSystem("PaintBallParticleSystem", collision.transform.position);
        }
    }

    public void SpawnParticleSystem(string name, Vector2 position)
    {
        GameObject g = Instantiate((GameObject)Resources.Load(name));
        ParticleSystemRenderer ps = g.GetComponent<ParticleSystemRenderer>();
        g.transform.position = position;

        Material newMat = ps.material;
        Color color = Colors.Instance.GetColorByIndex(colorObject.GetColorIndex());
        newMat.color = color;
        Vector4 v = new Vector4(color.r, color.g, color.b, color.a) * 2f;
        newMat.SetColor("_EmissionColor", v);
        ps.material = newMat;
    }

    public void OnGameOver()
    {
        SpawnParticleSystem("GameOverObjectParticleSystem", transform.position);

        gameObject.SetActive(false);
    }

    public void OnCorrect()
    {
        Shrink();
    }

    public void OnIncorrect()
    {
        //Shrink();
        move = false;
        rb.AddForce((Vector3.left + Vector3.up * Random.Range(-1f, 1f)) * 50, ForceMode2D.Impulse);
        collider.isTrigger = true;
        Invoke(nameof(DisableObject), 4);
    }

    void DisableObject()
    {
        collider.isTrigger = false;
        gameObject.SetActive(false);
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
        transform.position = new Vector3(-10, 0);
        transform.localScale = Vector3.one;
        CancelInvoke(nameof(DisableObject));
        StopCoroutine("ShrinkObject");
    }
}

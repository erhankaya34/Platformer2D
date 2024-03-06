using System;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float targetY;
    private float startY;
    [SerializeField] private float speed = 3.0f;
    private bool movingUp = true;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startY = transform.position.y;
        rb.gravityScale = 0;
    }

    void Update()
    {
        float yVelocity = 0;

        if (movingUp)
        {
            if (transform.position.y < targetY)
            {
                yVelocity = speed;
            }
            else
            {
                movingUp = false;
            }
        }
        else
        {
            if (transform.position.y > startY)
            {
                yVelocity = -speed;
            }
            else
            {
                movingUp = true;
            }
        }

        rb.velocity = new Vector2(0, yVelocity);
    }
}
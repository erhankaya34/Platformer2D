using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _groundDetectionRadius = 0.5f;
    public Transform groundCheck;
    public Transform playerCheckX; // For player detection alng the x axis
    public float detectionDistance = 3f; // Player detection distance

    private Rigidbody2D _rigidbody;
    private bool movingRight = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);
    }

    void Update()
    {
        Move();
        DetectPlayer(); // Player detection function
    }

    void Move()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = movingRight ? _speed : -_speed;
        _rigidbody.velocity = velocity;
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = movingRight ? new Vector3(0, 0, 0) : new Vector3(0, -180, 0);
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null || playerCheckX == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, _groundDetectionRadius);
        Gizmos.DrawWireCube(playerCheckX.position,
            new Vector3(detectionDistance * 2, 0.1f, 0)); // For player detection along the x-axis
    }

    void DetectPlayer()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, _groundDetectionRadius);
        Collider2D playerCollider = Physics2D.OverlapBox(playerCheckX.position,
            new Vector2(detectionDistance * 2, 0.1f), 0f,
            LayerMask.GetMask("Player")); // Player detection along the x-axis

        if (!isGrounded)
        {
            Flip();
        }

        if (playerCollider != null)
        {
            // If player is detected, face towards player's position
            Transform player = playerCollider.transform;
            if (player.position.x < transform.position.x && !movingRight)
            {
                Flip();
            }
            else if (player.position.x > transform.position.x && movingRight)
            {
                Flip();
            }
        }
    }
}
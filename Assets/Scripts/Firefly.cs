using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    public Transform target; 
    [SerializeField] private float moveSpeed = 10f; 
    [SerializeField] private float smoothness = 0.5f; 

    [SerializeField] private float swingMagnitude = 0.1f;
    [SerializeField] private float swingSpeed = 2f;

    private Rigidbody2D _rigidbody;
    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 targetPosition = new Vector2(target.position.x + 2, target.position.y + 4);
            Vector2 currentPosition = _rigidbody.position;

            Vector2 smoothedPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothness, moveSpeed);
            float offset = Mathf.Sin(Time.time * swingSpeed) * swingMagnitude;
            
            var nextPos = smoothedPosition + new Vector2(0f, offset);
            _rigidbody.MovePosition(nextPos);
        }
    }

}


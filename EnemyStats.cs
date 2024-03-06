using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _originalMaterial;
    [SerializeField] private Material _damagedMaterial;
    private float flashDuration = 0.1f;

    public float damage;

    private Rigidbody2D _rigidbody;
    [SerializeField] private float _knockBackForceX, _knockBackForceY;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 attackerPosition)
    {
        currentHealth -= damage;
        Vector2 knockbackDirection = transform.position - attackerPosition;
        knockbackDirection.Normalize();
        _rigidbody.AddForce(new Vector2(knockbackDirection.x * _knockBackForceX, _knockBackForceY), ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _animator.SetTrigger("death");
            Destroy(gameObject, 0.4f);
        }
        else
        {
            GetComponent<SpriteRenderer>().material = _damagedMaterial;
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashDuration);
        GetComponent<SpriteRenderer>().material = _originalMaterial;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RestartTrigger"))
        {
            GetComponent<SpriteRenderer>().material = _damagedMaterial;
            currentHealth -= currentHealth;
            _animator.SetTrigger("death");
            Destroy(gameObject, 0.4f);
        }
    }
}
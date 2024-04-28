using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Material _originalMaterial;
    [SerializeField] private Material _damagedMaterial;
    private float flashDuration = 0.1f;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;

    [SerializeField] private float _maxHealth = 100;
    private float _currentHealth;

    [SerializeField] private float _maxMana = 100;
    private float _currentMana;
    [SerializeField] private float manaRegenerationRate = 1f;

    private CharacterController _character;
    private Animator _animator;

    public bool isImmune;
    [SerializeField] float immunityTime;

    private bool isInHealingZone = false;
    private float healInterval = 1f;
    private float healTimer;

    [SerializeField] private Color originalManaBarColor;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<CharacterController>();
    }

    void Start()
    {
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;
        originalManaBarColor = manaBar.color;
    }

    void Update()
    {
        HealthControl();
        ManaControl();
        RegenerateMana();

        if (isInHealingZone)
        {
            HealOverTime();
        }
    }

    public void SetMana(float mana)
    {
        _currentMana = mana;
        if (_currentMana > _maxMana)
        {
            _currentMana = _maxMana;
        }
        else if (_currentMana < 0)
        {
            _currentMana = 0;
        }
    }

    public float GetCurrentMana()
    {
        return _currentMana;
    }

    public float GetMaxMana()
    {
        return _maxMana;
    }

    public void ConsumeAllMana()
    {
        _currentMana = 0;
    }

    private void HealthControl()
    {
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        healthBar.fillAmount = _currentHealth / _maxHealth;
    }

    private void ManaControl()
    {
        if (_currentMana > _maxMana)
        {
            _currentMana = _maxMana;
        }

        manaBar.fillAmount = _currentMana / _maxMana;
    }

    private void RegenerateMana()
    {
        if (_currentMana < _maxMana)
        {
            _currentMana += manaRegenerationRate * Time.deltaTime;
            _currentMana = Mathf.Min(_currentMana, _maxMana);
        }
    }

    public void FlashManaBar()
    {
        StartCoroutine(FlashManaBarCoroutine());
    }

    private IEnumerator FlashManaBarCoroutine()
    {
        manaBar.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        manaBar.color = originalManaBarColor;
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        StartCoroutine(ImmunityControl());
        AudioManager.instance.PlayAudio(AudioManager.instance.PlayerDamageAS);


        if (_currentHealth <= 0)
        {
            _character.canMove = false;
            StartCoroutine(RestartLevel());
        }
        else
        {
            GetComponent<SpriteRenderer>().material = _damagedMaterial;
            StartCoroutine(Flash());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isImmune)
        {
            TakeDamage(collision.GetComponent<EnemyStats>().damage);
        }

        if (collision.CompareTag("TrapDamage") && !isImmune)
        {
            TakeDamage(10);
        }

        if (collision.CompareTag("Heal"))
        {
            isInHealingZone = true;
            healTimer = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Heal"))
        {
            isInHealingZone = false;
        }
    }

    private void HealOverTime()
    {
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;
        }
        else
        {
            Heal(10);
            healTimer = healInterval;
        }
    }

    private void Heal(float healAmount)
    {
        if (_currentHealth < _maxHealth)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.HealingAS);
            _currentHealth += healAmount;
        }
        else if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }


    public void ResetHealth()
    {
        _currentHealth = 0;
    }

    public IEnumerator RestartLevel()
    {
        _animator.SetTrigger("die");
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashDuration);
        GetComponent<SpriteRenderer>().material = _originalMaterial;
    }

    private IEnumerator ImmunityControl()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityTime);
        isImmune = false;
    }
}
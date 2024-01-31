using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    private PlayerStats playerStats;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool isFacingRight = true;
    private bool isGrounded;
    private float moveInput;
    private float _lookDirection;
    [SerializeField] private float fallMultiplier;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDistance;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] private float damage;

    [SerializeField] private float attackRate = 2f;
    private float nextAttack = 0;

    private GameObject clone;
    private bool isCloneVisible = false;

    public bool canMove = true;
    private bool isDashing;
    public float dashTime;
    public float dashSpeed;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        isGrounded = CheckGround();
        CheckDash();
        AttackRateControl();
        CheckMove();
        CheckJump();
        ApplyExtraGravity();
        CheckTeleport();
        CheckAttack();
        UpdateAttackPointPosition();
        CheckFacing();
    }

    private void AttackRateControl()
    {
        if (Time.time > nextAttack)
        {
            CheckAttack();
        }
    }


    private void CheckFacing()
    {
        if (!Mathf.Approximately(_lookDirection, 0))
        {
            Flip();
        }
    }

    private void CheckDash()
    {
        if (Input.GetButtonDown("Dash") && playerStats.GetCurrentMana() >= 70)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        if (playerStats.GetCurrentMana() >= 70)
        {
            playerStats.SetMana(playerStats.GetCurrentMana() - 70);

            isDashing = true;
            float originalSpeed = moveSpeed;
            moveSpeed = dashSpeed;
            _rigidbody.velocity = new Vector2(moveSpeed * _lookDirection, _rigidbody.velocity.y);

            yield return new WaitForSeconds(dashTime);

            moveSpeed = originalSpeed;
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            isDashing = false;
        }
    }

    private void CheckMove()
    {
        if (!canMove || isDashing) return;

        moveInput = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(moveInput * moveSpeed, _rigidbody.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (!Mathf.Approximately(moveInput, 0))
        {
            _lookDirection = moveInput > 0 ? 1 : -1;
        }
    }

    private void CheckJump()
    {
        if (!isGrounded || !canMove) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            _animator.SetBool("isJump", true);
        }
        else
        {
            _animator.SetBool("isJump", false);
        }
    }


    private void ApplyExtraGravity()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void CheckTeleport()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCloneVisible)
            {
                CreateClone();
            }
            else
            {
                TeleportToClone();
            }
        }
    }

    private void CreateClone()
    {
        if (playerStats.GetCurrentMana() >= playerStats.GetMaxMana()) 
        { 
            playerStats.ConsumeAllMana();
            clone = new GameObject("Clone");
            clone.transform.position = transform.position;

            SpriteRenderer cloneRenderer = clone.AddComponent<SpriteRenderer>();
            cloneRenderer.color = new Color(1f, 1f, 1f, 0.7f); //color - opacity
            cloneRenderer.sprite = GetComponent<SpriteRenderer>().sprite; //setting clone's sprite as main character's
            cloneRenderer.sortingLayerName = "Foreground"; // sorting layer setting

            // clone collider setting
            Collider2D cloneCollider = clone.AddComponent<BoxCollider2D>();
            cloneCollider.isTrigger = true;

            // resize of clone
            clone.transform.localScale = new Vector3(2.015748f, 2.295006f, 1f);

            isCloneVisible = true;
        }
    }

    private void TeleportToClone()
    {
        if (clone != null)
        {
            transform.position = clone.transform.position;
            Destroy(clone);
            isCloneVisible = false;
        }
    }

    private void Flip()
    {
        isFacingRight = _lookDirection > 0;
        _spriteRenderer.flipX = !isFacingRight;
    }

    public void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
            nextAttack = Time.time + 1f / attackRate;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack2();
            nextAttack = Time.time + 1f / attackRate;
        }
    }

    private void AttackPointPosition(float direction)
    {
        Vector3 localPos = attackPoint.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * direction;
        attackPoint.localPosition = localPos;
    }

    private void UpdateAttackPointPosition()
    {
        if (isFacingRight)
        {
            AttackPointPosition(1);
        }
        else
        {
            AttackPointPosition(-1);
        }
    }


    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            canMove = false;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack1");
            nextAttack = Time.time + 6f;
            StartCoroutine(ResetVelocityAfterDelay());

            Collider2D closestEnemy = Physics2D.OverlapCircle(attackPoint.position, attackDistance, enemyLayers);
            damage = 34f;
            if (closestEnemy != null)
            {
                closestEnemy.GetComponent<EnemyStats>().TakeDamage(damage, transform.position);
            }
        }
    }

    public void Attack2()
    {
        if (Time.time > nextAttack)
        {
            canMove = false;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack2");
            nextAttack = Time.time + 6f;
            StartCoroutine(ResetVelocityAfterDelay());

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, enemyLayers);
            damage = 20f;
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(damage, transform.position);
            }
        }
    }

    IEnumerator ResetVelocityAfterDelay()
    {
        yield return new WaitForSeconds(0.4f);
        _rigidbody.velocity = new Vector2(moveInput * moveSpeed, _rigidbody.velocity.y);
        canMove = true;
    }

    private bool CheckGround()
    {
        float checkDistance = 0.1f;
        Vector2 startPosition = transform.position;
        Vector2 direction = Vector2.down;

        Debug.DrawLine(startPosition, startPosition + direction * checkDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, checkDistance);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RestartTrigger"))
        {
            playerStats.ResetHealth();
            canMove = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StartCoroutine(RestartLevel());
        }
    }

    public IEnumerator RestartLevel()
    {
        _animator.SetTrigger("die");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
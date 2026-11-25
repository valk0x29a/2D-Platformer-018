//Maria Tumidajewicz, Maciej Tuleja
using UnityEngine;
using System.Collections;

public class EagleController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.1f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.1f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveRange = 1.0f;

    const float rayLength = 1.25f;
    float startPositionX = 1.0f;
    bool isMovingRight = false;
    bool isDead = false;

    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPositionX = transform.position.x;
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        spriteRenderer.flipX = true;
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        spriteRenderer.flipX = false;
    }

    void Update()
    {
        if (isDead) { return; }
        if (isMovingRight)
        {
            if (transform.position.x < startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                isMovingRight = false;
            }
        }
        else
        {
            if (transform.position.x > startPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {
                isMovingRight = true;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Player"))
        {
            if (transform.position.y < collision.transform.position.y)
            {
                isDead = true;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.33f);
        gameObject.SetActive(false);
    }
}

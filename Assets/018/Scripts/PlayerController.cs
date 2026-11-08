//Maria Tumidajewicz, Maciej Tuleja
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game018
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement parameters")]
        [Range(0.1f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
        [Range(0.1f, 20.0f)] [SerializeField] private float jumpForce = 6.0f;
        [SerializeField] private LayerMask groundLayer;

        const float rayLength = 1.25f;
        Vector2 startPosition;

        Rigidbody2D rb2D;
        Animator animator;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPosition = transform.position;
        }
        private bool IsGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value);
        }

        private void Jump()
        {
            if (IsGrounded())
            {
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump!!!");
            }
        }

        void Update()
        {
            if (GameMenager.Instance.currentGameState == GameState.GAME)
            {

                animator.SetBool("isRunning", false);
                spriteRenderer.flipX = false;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    animator.SetBool("isRunning", true);
                }
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    animator.SetBool("isRunning", true);
                    spriteRenderer.flipX = true;
                }
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                //Debug.DrawRay(transform.position, rayLength * Vector2.down, Color.white, 0.2f);
                animator.SetBool("isGrounded", IsGrounded());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameMenager.Instance.currentGameState == GameState.GAME)
                {
                    GameMenager.Instance.PauseMenu();
                }
                else if (GameMenager.Instance.currentGameState == GameState.PAUSE_MENU)
                {
                    GameMenager.Instance.InGame();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("LevelExit"))
            {
                Debug.Log("Game Over");
            }
            if(collision.CompareTag("LevelFall"))
            {
                SceneManager.LoadScene("Level 1");
            }
            if (collision.CompareTag("bonus"))
            {
                Debug.Log("bonus");
                GameMenager.Instance.AddPoints(10);
                collision.gameObject.SetActive(false);
            }
            if (collision.CompareTag("Enemy"))
            {
                if (transform.position.y > collision.gameObject.transform.position.y)
                {
                    Debug.Log("killed an enemy");
                }
            }
        }
    }
}

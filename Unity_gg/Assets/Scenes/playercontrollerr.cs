using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;  
public class PlayerController : MonoBehaviour
{
    public string playerName = "player";
    public string nextSceneName = "Stage2";
    public int hp = 100;
    public float moveSpeed = 5f;
    public float jumpPower = 8f;
    public Vector3 startPosition;
    public Transform visual;
    public GameObject gameOverPanel;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    public int lives = 3;
    private bool isGameOver = false;
    private float facing = 1f;
    public TMP_Text livesText;
    private Vector2 moveInput;

    public float fallLimit = -10f;
    public Vector2 airScale = new Vector2(0.8f, 1.2f);


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("착지");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("공중");
        }
    }

        
    

    void Start()
    {
            rb = GetComponent<Rigidbody2D>();
            transform.position = startPosition;
             livesText.text = "LIFE " + lives;

    }
    void LoseLife()
    {
        lives -= 1;
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        livesText.text = "LIFE " + lives;

        if (lives <= 0)
        {
            isGameOver = true;
           gameOverPanel.SetActive(true);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0) 
        { 
            facing = 1f; 
        }
        else if (moveInput.x < 0)
        { 
            facing = -1f;
        }
    }
 

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && !isGameOver)
        {
            Debug.Log("점프!");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
        transform.Translate(Vector3.right * moveInput.x * moveSpeed * Time.deltaTime);
        }

        if (transform.position.y < fallLimit)
        {
            LoseLife();
        }

        if (isGrounded)
        { visual.localScale = new Vector3(facing, 1f, 1f); }
        else
        {
            visual.localScale = new Vector3(facing * airScale.x,
          airScale.y, 1f);
        }

    }

 private void OnTriggerEnter2D(Collider2D collision)
   {
        if(collision.CompareTag("Goal"))
        {
           SceneManager.LoadScene(nextSceneName);

        }

        if (collision.CompareTag("Trap"))
        {
            LoseLife();
        }

    }
}
    


using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID = 1;
    public bool doAnomalyDetection = false;
    public float moveSpeed = 5f;        // Movement speed of the player
    public float sprintSpeedMultiplier = 2f; // Speed multiplier when sprinting
    public float jumpForce = 10f;       // Force applied when jumping
    public Transform groundCheck;       // Reference to a point checking if the player is grounded
    public LayerMask groundLayer;       // Layer mask for detecting ground
    public float saveInterval = 0.1f;   // Interval at which to save player data (in seconds)
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float saveTimer;
    private DataManager dataManager;
    private AnomalyDetectionManager anomalyDetectionManager;
    private bool facingRight = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dataManager = DataManager.instance; // Access the DataManager singleton instance
        anomalyDetectionManager = AnomalyDetectionManager.instance; // Access the AnomalyDetectionManager singleton instance
        
        if(doAnomalyDetection)
            anomalyDetectionManager.TrainModelWithData(dataManager.GetPlayerSpeedData(playerID));
    }

    private void Update()
    {
        // Save player data timer update
        saveTimer += Time.deltaTime;

        // Move the player horizontally
        float moveInput = Input.GetAxis("Horizontal");
        float currentMoveSpeed = moveSpeed;

        // Check if "Ctrl" key is pressed to sprint
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentMoveSpeed *= sprintSpeedMultiplier;
        }

        if(Mathf.Abs(moveInput) >= 0.01)
        {
            animator.SetInteger("playerState", 1); // Turn on run animation
            if (saveTimer >= saveInterval)
            {
                SavePlayerData();
                saveTimer = 0f;
            }
        }
        else
        {
            animator.SetInteger("playerState", 0);
        }

        rb.velocity = new Vector2(moveInput * currentMoveSpeed, rb.velocity.y);

        // Jump if the player is grounded and the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetInteger("playerState", 2); // Turn on jump animation
        }
        
        if(facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }
    
    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void SavePlayerData()
    {
        // Save player speed, position (x and y), and time
        PlayerData playerData = new PlayerData
        {
            PlayerId = this.playerID,  // You can set a unique ID for each player if needed
            PlayerName = "Player1",
            Time = Time.time,
            PlayerX = transform.position.x,
            PlayerY = transform.position.y,
            PlayerSpeed = Mathf.Abs((float)Math.Round(rb.velocity.magnitude, 1)) // Absolute value of velocity for speed
        };

        if (doAnomalyDetection)
        {
            anomalyDetectionManager.PredictIfSpeedIsAnomaly(playerData);
        }
        else
        {
            dataManager.UpdatePlayerData(playerData.PlayerId, playerData.PlayerName, playerData.Time, playerData.PlayerX, playerData.PlayerY, playerData.PlayerSpeed);
        }
    }
}

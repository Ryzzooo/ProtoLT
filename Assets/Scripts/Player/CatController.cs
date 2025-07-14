using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
    public float movementSpeed, jumpForce;
    private float tmpMovementSpeed;
    public bool isFacingRight, isJumping;
    Rigidbody2D rb;
    Collider2D bc;
    public GameObject winScreenUI;

    //groundchecker
    public float radius;
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public LayerMask whatIsGround2;
    public LayerMask platformLayer;

    private float dropTime = 0.2f;

    //animation
    Animator anim;
    string walk_param = "walk";
    string jump_param = "jump";
    string idle_param = "idle";


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<Collider2D>();
    }

    void Update()
    {
        Jump();
        Down();
        Run();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    bool IsOnGround()
    {
        return isGrounded() || isGrounded2() || isGrounded3();
    }


    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * movementSpeed, rb.linearVelocity.y);


        if (move != 0)
        {
            anim.SetTrigger(walk_param);
        }
        else
        {
            anim.SetTrigger(idle_param);
        }

        if (move > 0 && !isFacingRight)
        {
            transform.eulerAngles = Vector2.zero;
            isFacingRight = true;
        }
        else if (move < 0 && isFacingRight)
        {
            transform.eulerAngles = Vector2.up * 180;
            isFacingRight = false;
        }
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            tmpMovementSpeed = movementSpeed;
            movementSpeed += 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = tmpMovementSpeed;
        }
    }

    void Down()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsOnGround())
        {
            DropDownThroughPlatform();
        }
    }

    void DropDownThroughPlatform()
    {
        Vector2 boxSize = new Vector2(0.8f, 2f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.5f); // Sedikit di bawah player

        Collider2D[] platforms = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (var platform in platforms)
        {
            Debug.Log("Menemukan platform: ");
            if (platform.CompareTag("OneWayPlatform"))
            {
                Debug.Log("S ditekan, mencoba menembus platform...");
                StartCoroutine(DisableCollision(platform));
            }
        }
    }

    IEnumerator DisableCollision(Collider2D platform)
    {
        // Disable collision dengan platform
        Physics2D.IgnoreCollision(bc, platform, true);

        // Tunggu waktu awal drop
        yield return new WaitForSeconds(dropTime);

        // Maksimal waktu tunggu tambahan agar platform tidak hilang selamanya
        float maxWait = 1f;
        float waited = 0f;

        float minSafeDistance = 0.5f;

        while (waited < maxWait)
        {
            Bounds platformBounds = platform.bounds;
            Bounds playerBounds = bc.bounds;

            float distance = platformBounds.min.y - playerBounds.max.y;

            if (distance >= minSafeDistance)
            {
                break; // kepala player sudah cukup jauh
            }

            waited += Time.deltaTime;
            yield return null;
        }

        // Aktifkan kembali collider platform
        Physics2D.IgnoreCollision(bc, platform, false);
        Debug.Log("Collision platform dikembalikan");
    }

    int LayerMaskToLayer(LayerMask layerMask)
    {
        int layer = 0;
        int layerMaskValue = layerMask.value;
        while (layerMaskValue > 1)
        {
            layerMaskValue = layerMaskValue >> 1;
            layer++;
        }
        return layer;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsOnGround())
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            soundmanager.Instance.PlaySound("CatJump");
        }

        if (!isJumping && !IsOnGround())
        {
            anim.SetTrigger(jump_param);
            isJumping = true;
        }
        else if (isJumping && IsOnGround())
        {
            isJumping = false;
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundChecker.position, radius, whatIsGround);
    }

    bool isGrounded2()
    {
        return Physics2D.OverlapCircle(groundChecker.position, radius, whatIsGround2);
    }

    bool isGrounded3()
    {
        return Physics2D.OverlapCircle(groundChecker.position, radius, platformLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxSize = new Vector2(0.8f, 2f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
} 
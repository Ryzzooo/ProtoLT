using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
    public float movementSpeed, jumpForce;
    public bool isFacingRight, isJumping;
    Rigidbody2D rb;
    public GameObject winScreenUI;

    //groundchecker
    public float radius;
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public LayerMask whatIsGround2;
   
    //animation
    Animator anim;
    string walk_param = "walk";
    string jump_param = "jump";
    string idle_param = "idle";


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    bool IsOnGround()
    {
        return isGrounded() || isGrounded2();
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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
} 
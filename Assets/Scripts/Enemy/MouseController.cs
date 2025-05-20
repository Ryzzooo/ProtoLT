using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    //movement
    public float movementSpeed;
    public bool isFacingRight;

    //checker
    public Transform groundChecker;
    public float groundCheckerRadius;
    public LayerMask whatIsGround;
    public Transform healthBarHUD;

    void Update()
    {
        transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
        if (!ThereIsGround())
        {
            if (isFacingRight)
            {
                healthBarHUD.localEulerAngles = Vector2.up * 180;
                transform.eulerAngles = Vector2.up * 180;
                isFacingRight = false;
            }
            else 
            {
                healthBarHUD.localEulerAngles = Vector2.zero;
                transform.eulerAngles = Vector2.zero;
                isFacingRight = true;
            }
        }

    }

    bool ThereIsGround()
    {
        return Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRadius);
    }
}

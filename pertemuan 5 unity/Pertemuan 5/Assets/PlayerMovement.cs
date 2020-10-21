using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_movementSmoothing;   // jumlah smoothing
    public int movementSpeed;           
    Vector3 m_velocity = Vector3.zero;  // velocity dari object di assign Vector3.zero biar defaultnya ke situ
    float HorizontalMove;               // value untuk Input.GetAxisRaw
    bool facingRight = true;
    Rigidbody2D m_rigidbody2D;          // untuk reference Rigidbody2D
    public float jumpForce;
    
    public bool isGrounded; 
    public Transform groundCheck;       // Transform dari gameobject yang kita pake untuk posisi checkGround
    public LayerMask groundLayer;       // Layer Ground
    public float groundCheckSize;       // radius untuk checkGround

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HorizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
         
        // bisa diganti Input.GetButtonDown() untuk virtual button yang ada di project settings -> input manager
        if (Input.GetKeyDown("space") && isGrounded)
        {
            jump(jumpForce);
        }
    }

    private void FixedUpdate()
    {
        move(HorizontalMove);

        // CheckGround
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
    }

    private void move(float movement)
    {
        Vector3 targetvelocity = new Vector2(movement, m_rigidbody2D.velocity.y);
        m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetvelocity, ref m_velocity, m_movementSmoothing);

        if(movement < 0 && facingRight)
        {
            facingRight = false;
            flip();
        }
        else if(movement > 0 && facingRight == false)
        {
            facingRight = true;
            flip();
        }

    }

    // localScale.x dari object di kali -1 biar nge flip
    void flip()
    {
        Vector3 currScale = transform.localScale;
        currScale.x *= -1;
        transform.localScale = currScale;
    }

    void jump(float jumpForce)
    {
        m_rigidbody2D.AddForce(new Vector2(0, jumpForce));
    }

    // Untuk liat circlecast checkGround
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float sprintingSpeed = 8f;
    [SerializeField] private float rjumpingPower = 16f;
    [SerializeField] private float maxStamina = 100f;
    private float stamina;
    [SerializeField] private float d_sprintingSpeed = 1f;
    [SerializeField] private float d_jumpingPower = 10f;
    [SerializeField] private float fallingMultiplier;
    [SerializeField] private float d_value;
    private bool isFacingRight = true;
    public bool canRun = true;
    private float jumpingPower;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        jumpingPower = rjumpingPower;
        stamina = maxStamina;
    }

    void Update()
    {
        Debug.Log($"{stamina}");
        if (!canRun)
        {
            horizontal = 0f;
            jumpingPower = 0f;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            jumpingPower = rjumpingPower;
        }
        increaseStamina();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && stamina > d_jumpingPower)
        {
            stamina -= d_jumpingPower;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (rb.velocity.y < 0f && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallingMultiplier);
        }

        Flip();
       //Debug.Log($"Stamina {stamina}");
        //Debug.Log($"Velocity {rb.velocity.y}");
    }

    private void FixedUpdate()
    {
        //if (canRun)
        //{
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                stamina -= d_sprintingSpeed * Time.deltaTime;
                rb.velocity = new Vector2(horizontal * sprintingSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        //}
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void increaseStamina()
    {
        if(rb.velocity.magnitude < 9 && stamina < maxStamina)
        {
            stamina += d_value * Time.deltaTime;
        }
    }
}
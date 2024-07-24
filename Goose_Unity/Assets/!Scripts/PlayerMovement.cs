using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float fallGravity;
    [SerializeField] float jumpGravity;
    [SerializeField] float floatGravity;
    Rigidbody rb;
    [SerializeField] bool isJumping;
    [SerializeField] bool isFloating;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void MovePlayer(Vector2 dir)
    {
        if (dir.x != 0)
        {
            float rotationDirection = dir.x > 0 ? 1f : -1f;
            float rotationAmount = rotationSpeed * rotationDirection * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
        if (dir.y != 0)
        {
            Vector3 moveDirection = transform.forward * dir.y * moveSpeed * Time.deltaTime;
            transform.Translate(moveDirection, Space.World);
        }
    }
    public void PlayerJump()
    {
        if (IsGrounded())
        {
            //isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        /*        if (IsGrounded())
                    isJumping = false;
                else if (rb.velocity.y > 0)
                    isJumping = true;*/
        HandleGravity();
    }
    void HandleGravity()
    {
        if (rb.velocity.y < 0)
        {
            float currentFallGravityScale = isFloating ? floatGravity : fallGravity;
            //Debug.LogError("Gravity: -------- " + currentFallGravityScale);
            rb.AddForce(Vector3.up * Physics.gravity.y * (currentFallGravityScale - 1), ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0)
            rb.AddForce(Vector3.up * Physics.gravity.y * (jumpGravity - 1), ForceMode.Acceleration);
    }
    bool IsGrounded()
    {
        Vector3 rayStart = transform.position;
        float rayLength = 1.1f;
        RaycastHit hit;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayLength))
        {
            // Check if the collider is not a trigger
            if (!hit.collider.isTrigger)
            {
                return true;
            }
        }

        return false;
    }
    public void SetFloatState(bool state)
    {
        isFloating = state;
    }
    public bool GetJumpStates()
    {
        return isJumping;
    }
}

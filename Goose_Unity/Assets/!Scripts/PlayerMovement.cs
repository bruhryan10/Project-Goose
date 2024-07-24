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
    bool isJumping;
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
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Update()
    {
        if (isJumping && rb.velocity.y < 0)
        {
            float currentFallGravityScale = isFloating ? floatGravity : fallGravity;
            rb.velocity += Vector3.up * Physics.gravity.y * (currentFallGravityScale - 1) * Time.deltaTime;
        }
        else if (isJumping && rb.velocity.y > 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpGravity - 1) * Time.deltaTime;
    }
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    public void SetFloatState(bool state)
    {
        isFloating = state;
    }
    public bool GetJumpState()
    {
        return isJumping;
    }
}

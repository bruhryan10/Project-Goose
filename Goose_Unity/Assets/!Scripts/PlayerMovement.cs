using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;
   
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
{
    // Calculate the attempted movement
    Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

    // Clamp to camera bounds
    Vector2 clampedPos = ClampToCameraBounds(newPos);

    // Apply movement
    rb.MovePosition(clampedPos);
}

Vector2 ClampToCameraBounds(Vector2 targetPos)
{
    Camera cam = Camera.main;

    // Camera size
    float camHeight = cam.orthographicSize;
    float camWidth = camHeight * cam.aspect;

    // Player sprite bounds (optional but recommended so the player doesn't clip into edges)
    float halfWidth = 0.3f;  // adjust depending on your sprite size
    float halfHeight = 0.3f;

    float minX = cam.transform.position.x - camWidth + halfWidth;
    float maxX = cam.transform.position.x + camWidth - halfWidth;

    float minY = cam.transform.position.y - camHeight + halfHeight;
    float maxY = cam.transform.position.y + camHeight - halfHeight;

    // Clamp to camera edges
    float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
    float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

    return new Vector2(clampedX, clampedY);
}


}

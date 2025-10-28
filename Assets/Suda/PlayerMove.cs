using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxStepDistance = 3f;

    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0f;

            float distance = Vector2.Distance(transform.position, mouseWorldPos);
            if (distance > maxStepDistance)
            {
                Vector2 dir = (mouseWorldPos - transform.position).normalized;
                targetPosition = (Vector2)transform.position + dir * maxStepDistance;
            }
            else
            {
                targetPosition = mouseWorldPos;
            }

            isMoving = true;
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
                isMoving = false;
        }
    }
}

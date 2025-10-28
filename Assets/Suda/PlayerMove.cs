using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMove : MonoBehaviour
{
    [Header("Ý’è")]
    public float moveSpeed = 5f;          
    public float maxStepDistance = 3f;   
    public float zPosition = -1f;        

    private Vector2 targetPosition;
    private bool isMoving = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = zPosition; 

            float distance = Vector2.Distance(transform.position, mouseWorldPos);
            if (distance > maxStepDistance)
            {
                Vector2 direction = (mouseWorldPos - transform.position).normalized;
                targetPosition = (Vector2)transform.position + direction * maxStepDistance;
            }
            else
            {
                targetPosition = mouseWorldPos;
            }

            targetPosition = ClampToCameraBounds(targetPosition);

            isMoving = true;
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }

    private Vector2 ClampToCameraBounds(Vector2 pos)
    {
        Vector3 bottomLeft = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);

        return pos;
    }
}

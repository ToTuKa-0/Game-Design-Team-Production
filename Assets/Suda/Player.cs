using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Ý’è")]
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

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
    }
}

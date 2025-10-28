using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("ê›íË")]
    public float moveSpeed = 5f;
    public float maxStepDistance = 3f;
    public float zPosition = -1f;
    public float moveDelay = 2f; 

    private Vector2 targetPosition;
    private bool isMoving = false;
    private bool canMove = true;
    private Camera mainCam;
    private Rigidbody2D rb;

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        targetPosition = transform.position;
    }

    void Update()
    {
        if (!canMove) return;

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
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                StartCoroutine(MoveCooldown()); 
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }

    private IEnumerator MoveCooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    private Vector2 ClampToCameraBounds(Vector2 pos)
    {
        Vector3 bottomLeft = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);

        return pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("PlayerÇ™ìGÇ…ìñÇΩÇ¡ÇƒéÄñS");
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGemChase : MonoBehaviour
{
    public Transform[] movePoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float reachDistance = 0.1f;

    private int currentIndex = 0;
    private bool isChasing = false;
    private GameObject player;
    private Rigidbody2D rb;

    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player = GameObject.FindWithTag("Player");
        PlayerMove1.OnGemPickup += StartChase;

        SetNextPatrolTarget();
    }

    void OnDestroy()
    {
        PlayerMove1.OnGemPickup -= StartChase;
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (isChasing)
            UpdateChaseTarget();
        else
            UpdatePatrolTarget();
    }

    void FixedUpdate()
    {
        if ((Vector2)rb.position == targetPosition) return;

        float speed = isChasing ? chaseSpeed : patrolSpeed;
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void UpdatePatrolTarget()
    {
        if (movePoints == null || movePoints.Length == 0) return;

        Vector2 currentTarget = (Vector2)movePoints[currentIndex].position;
        targetPosition = new Vector2(currentTarget.x, currentTarget.y);

        float dist = Vector2.Distance(transform.position, currentTarget);
        if (dist < reachDistance)
        {
            currentIndex++;
            if (currentIndex >= movePoints.Length)
                currentIndex = 0;

            SetNextPatrolTarget();
        }
    }

    private void SetNextPatrolTarget()
    {
        if (movePoints == null || movePoints.Length == 0) return;
        targetPosition = (Vector2)movePoints[currentIndex].position;
    }

    private void UpdateChaseTarget()
    {
        if (player == null) return;
        Vector3 p = player.transform.position;
        targetPosition = new Vector2(p.x, p.y);
    }

    void StartChase()
    {
        isChasing = true;
    }
}

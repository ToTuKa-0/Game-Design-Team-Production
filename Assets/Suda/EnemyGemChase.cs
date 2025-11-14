using UnityEngine;

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

    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player = GameObject.FindWithTag("Player");
        PlayerMove1.OnGemPickup += StartChase;
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
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        if (movePoints.Length == 0) return;

        Transform target = movePoints[currentIndex];
        moveDirection = (target.position - transform.position).normalized;

        rb.velocity = moveDirection * patrolSpeed;

        float dist = Vector2.Distance(transform.position, target.position);
        if (dist < reachDistance)
        {
            currentIndex++;
            if (currentIndex >= movePoints.Length)
                currentIndex = 0;
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        moveDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = moveDirection * chaseSpeed;
    }

    void StartChase()
    {
        isChasing = true;
    }
}

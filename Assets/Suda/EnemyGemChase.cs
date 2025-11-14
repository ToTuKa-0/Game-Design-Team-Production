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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

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
        Vector2 newPos = Vector2.MoveTowards(rb.position, target.position, patrolSpeed * Time.deltaTime);
        rb.MovePosition(newPos);

        float dist = Vector2.Distance(rb.position, target.position);
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

        Vector3 targetPos = player.transform.position;
        targetPos.z = transform.position.z; 
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, chaseSpeed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    void StartChase()
    {
        isChasing = true;
    }
}

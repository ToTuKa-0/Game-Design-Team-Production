using UnityEngine;

public class EnemyGemChase : MonoBehaviour
{
    [Header("巡回ポイント")]
    public Transform[] movePoints;

    [Header("移動設定")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("目的地への許容距離")]
    public float reachDistance = 0.1f;

    private int currentIndex = 0;
    private bool isChasing = false;

    private GameObject player;
    private PlayerMove1 playerScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerMove1>();
        }
    }

    void Update()
    {
        if (playerScript == null) return;

        isChasing = playerScript.hasAbility;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (movePoints.Length == 0) return;

        Transform target = movePoints[currentIndex];

        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

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
        Vector3 targetPos = player.transform.position;
        targetPos.z = transform.position.z; 
        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
    }
}

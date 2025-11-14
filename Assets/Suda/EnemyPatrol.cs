using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("巡回ポイント")]
    public Transform[] movePoints;

    [Header("移動速度")]
    public float speed = 2f;

    [Header("目的地への許容距離")]
    public float reachDistance = 0.1f;

    private int currentIndex = 0;

    void Update()
    {
        if (movePoints.Length == 0) return;

        Transform target = movePoints[currentIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist < reachDistance)
        {
            currentIndex++;

            if (currentIndex >= movePoints.Length)
            {
                currentIndex = 0;
            }
        }
    }
}

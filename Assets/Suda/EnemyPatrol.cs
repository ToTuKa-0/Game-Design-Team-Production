using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("巡回ポイント")]
    public Transform[] movePoints;

    [Header("移動速度")]
    public float speed = 3f;

    [Header("目的地への許容距離")]
    public float reachDistance = 0.1f;

    [Header("Z座標固定")]
    public float fixedZ = -1f; 

    private int currentIndex = 0;

    void Update()
    {
        if (movePoints.Length == 0) return;

        Transform target = movePoints[currentIndex];

        Vector3 newPos = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
        newPos.z = fixedZ; 
        transform.position = newPos;

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

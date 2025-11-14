using UnityEngine;

public class PointMove : MonoBehaviour
{
    public Transform[] points;  // 経路となるポイント（空のGameObjectなど）
    public float speed = 2f;    // 移動速度
    private int currentPoint = 0; // 現在の目的地インデックス

    void Update()
    {
        if (points.Length == 0) return;

        // 現在の目標地点を取得
        Transform target = points[currentPoint];

        // 現在位置から目標地点への方向を計算
        Vector2 direction = (target.position - transform.position).normalized;

        // 移動
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // 目標地点に十分近づいたら次のポイントへ
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= points.Length)
            {
                currentPoint = 0; // 経路をループ
            }
        }
    }
}


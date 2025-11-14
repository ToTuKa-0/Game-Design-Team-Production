using UnityEngine;

public class PlayerMoveW : MonoBehaviour
{
    public float Movespeed = 0.2f; // 小さいほど素早く追従
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;      // 2Dで落下しないように
        rb.freezeRotation = true; // 回転防止
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0f;
            targetPosition = mousePos;
        }
    }

    void FixedUpdate()
    {
        // Rigidbodyを使って滑らかに移動
        Vector3 newPos = Vector3.SmoothDamp(
            rb.position,
            targetPosition,
            ref velocity,
            Movespeed
        );

        rb.MovePosition(newPos);
    }

    // 他のオブジェクトに当たった時に呼ばれる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ぶつかった相手：" + collision.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("重なった相手：" + other.gameObject.name);
    }
}

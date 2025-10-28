using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    [Header("設定")]
    public float moveSpeed = 2f;
    public float stepDistance = 1f;
    public int moveSteps = 3;
    public float waitTime = 0.5f;
    public float zPosition = -1f;

    private Vector2 startPos;
    private bool isMoving = false;
    private bool isStunned = false;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            if (!isStunned)
            {
                yield return MoveTo(startPos + Vector2.left * stepDistance * moveSteps);
                yield return new WaitForSeconds(waitTime);
                yield return MoveTo(startPos);
                yield return new WaitForSeconds(waitTime);
                yield return MoveTo(startPos + Vector2.right * stepDistance * moveSteps);
                yield return new WaitForSeconds(waitTime);
                yield return MoveTo(startPos);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null; // スタン中は停止
            }
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        isMoving = true;
        while (!isStunned && Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
            yield return null;
        }
        isMoving = false;
    }

    // 🔹 スタン処理
    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        Debug.Log($"{gameObject.name} がスタンした！");
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        Debug.Log($"{gameObject.name} のスタン解除！");
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }
}

using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    [Header("Ý’è")]
    public float moveSpeed = 2f;        
    public float stepDistance = 1f;     
    public int moveSteps = 3;           
    public float waitTime = 0.5f;      
    public float zPosition = -1f;       

    private Vector2 startPos;           
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
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
    }

    IEnumerator MoveTo(Vector2 target)
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition); 
            yield return null;
        }
        isMoving = false;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }
}

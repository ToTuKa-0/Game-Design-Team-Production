using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public int maxSteps = 3;
    public float stepDistance = 1f;
    public float moveDelay = 2f;
    public float zPosition = -1f;

    [Header("アビリティ関連")]
    public bool hasAbility = false; 

    private bool canMove = true;
    private int stepsRemaining;
    private Vector3 targetPos;

    void Start()
    {
        stepsRemaining = maxSteps;
        targetPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = zPosition;

            float distance = Vector2.Distance(transform.position, mouseWorld);
            if (distance <= stepDistance * maxSteps)
            {
                StartCoroutine(MoveTo(mouseWorld));
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }

    System.Collections.IEnumerator MoveTo(Vector3 target)
    {
        canMove = false;
        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
            yield return null;
        }

        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hasAbility)
            {
                EnemyMove enemy = other.GetComponent<EnemyMove>();
                if (enemy != null) enemy.StunEnemy();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Cherry"))
        {
            hasAbility = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene("SUDA_stage02");
        }
    }
}

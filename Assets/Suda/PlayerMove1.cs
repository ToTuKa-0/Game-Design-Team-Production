using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove1 : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 10f;
    public int maxSteps = 3;
    public float stepDistance = 1f;
    public float moveDelay = 2f;
    public float zPosition = -1f;

    [Header("アビリティ関連")]
    public bool hasAbility = false;

    [Header("アニメーション設定")]
    public Animator animator;

    private bool canMove = true;
    private bool hitWall = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "SUDA_stage02")
        {
            GameObject spawn = GameObject.Find("spawn1");
            if (spawn != null)
            {
                transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, zPosition);
            }
        }
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

    IEnumerator MoveTo(Vector3 target)
    {
        canMove = false;
        hitWall = false;

        if (animator != null)
            animator.SetBool("Walk", true);

        while (!hitWall && Vector2.Distance(rb.position, target) > 0.05f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }

        if (animator != null)
            animator.SetBool("Walk", false);

        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            hitWall = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            if (hasAbility)
            {
                EnemyMove1 enemy = other.GetComponent<EnemyMove1>();
                if (enemy != null)
                {
                    StartCoroutine(EnemyStun(enemy, 3f));
                }
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
        else if (other.CompareTag("Goal1"))
        {
            SceneManager.LoadScene("SUDA_stage02");
        }
    }

    IEnumerator EnemyStun(EnemyMove1 enemy, float duration)
    {
        enemy.StunEnemy(); 
        yield return new WaitForSeconds(duration);
        enemy.RecoverEnemy(); 
    }

    public void ActivateCherryAbility()
    {
        hasAbility = true;
    }
}

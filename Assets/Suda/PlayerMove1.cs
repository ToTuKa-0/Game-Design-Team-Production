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
    private bool cherryUsed = false;
    private float originalSpeed;

    [Header("アニメーション設定")]
    public Animator animator;

    private bool canMove = true;
    private bool hitWall = false;
    private Rigidbody2D rb;

    public delegate void GemPickupHandler();
    public static event GemPickupHandler OnGemPickup;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (animator == null) animator = GetComponent<Animator>();

        originalSpeed = moveSpeed;

        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResetControlFlags();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        WarpToSpawn(scene.name);
        ResetControlFlags();
    }

    private void WarpToSpawn(string sceneName)
    {
        string spawnName = sceneName switch
        {
            "SUDA_stage02" => "Spawn1",
            "SUDA_stage03" => "Spawn2",
            "SUDA_stage04" => "Spawn3",
            "SUDA_stage05" => "Spawn4",
            _ => ""
        };

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawn = GameObject.Find(spawnName);
            if (spawn != null)
            {
                transform.position = new Vector3(
                    spawn.transform.position.x,
                    spawn.transform.position.y,
                    zPosition
                );
            }
            else
            {
                Debug.LogWarning("Spawn not found: " + spawnName);
            }
        }

        if (sceneName == "SUDA_stage04")
        {
            hasAbility = false;
            cherryUsed = false;
            moveSpeed = originalSpeed;
            OnGemPickup = null;
        }

    }

    private void ResetControlFlags()
    {
        canMove = true;
        hitWall = false;
        StopAllCoroutines();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = zPosition;

            if (Vector2.Distance(transform.position, mouseWorld) <= stepDistance * maxSteps)
                StartCoroutine(MoveTo(mouseWorld));
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }

    IEnumerator MoveTo(Vector3 target)
    {
        canMove = false;
        hitWall = false;

        if (animator != null) animator.SetBool("Walk", true);

        while (!hitWall && Vector2.Distance(rb.position, target) > 0.05f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }

        if (animator != null) animator.SetBool("Walk", false);
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Wall":
                hitWall = true;
                break;

            case "Enemy":
                EnemyMove1 enemy = other.GetComponent<EnemyMove1>();
                if (enemy != null)
                {
                    if (hasAbility && !cherryUsed)
                    {
                        StartCoroutine(EnemyStun(enemy, 3f));
                        cherryUsed = true;
                    }
                    else
                    {
                        SceneManager.LoadScene("SUDA_GameOver");
                    }
                }
                break;

            case "Cherry":
                hasAbility = true;
                cherryUsed = false;
                Destroy(other.gameObject);
                break;

            case "Mushroom":
                moveSpeed *= 2f;
                Destroy(other.gameObject);
                break;

            case "Gem":
                hasAbility = true;
                Destroy(other.gameObject);
                OnGemPickup?.Invoke();
                break;

            case "Goal1":
                SceneManager.LoadScene("SUDA_stage02");
                break;

            case "Goal2":
                moveSpeed = originalSpeed;
                SceneManager.LoadScene("SUDA_stage03");
                break;

            case "Goal3":
                moveSpeed = originalSpeed;
                hasAbility = false;
                cherryUsed = false;
                OnGemPickup = null;
                SceneManager.LoadScene("SUDA_stage04");
                break;

            case "Goal4":
                moveSpeed = originalSpeed;
                SceneManager.LoadScene("SUDA_stage05");
                break;
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
        cherryUsed = false;
    }
}

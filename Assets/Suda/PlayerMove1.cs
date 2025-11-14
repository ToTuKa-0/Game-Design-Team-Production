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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (animator == null)
            animator = GetComponent<Animator>();

        originalSpeed = moveSpeed;
    }

    void Start()
    {
        WarpToSpawn(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        WarpToSpawn(scene.name);
    }

    private void WarpToSpawn(string sceneName)
    {
        string spawnName = "";
        switch (sceneName)
        {
            case "SUDA_stage02": spawnName = "Spawn1"; break;
            case "SUDA_stage03": spawnName = "Spawn2"; break;
            case "SUDA_stage04": spawnName = "Spawn3"; break;
        }

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawn = GameObject.Find(spawnName);
            if (spawn != null)
                transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, zPosition);
        }

        if (sceneName == "SUDA_stage04")
        {
            hasAbility = false;
            cherryUsed = false;
            moveSpeed = originalSpeed;
            OnGemPickup = null;
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
                        Destroy(gameObject); 
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

using UnityEngine;

public class Cherry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🍒 Cherryを取得！アビリティ発動！");
            PlayerMove player = other.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.ActivateCherryAbility();
            }

            Destroy(gameObject); // 取ったら消す
        }
    }
}

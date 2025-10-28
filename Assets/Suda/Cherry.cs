using UnityEngine;

public class Cherry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.ActivateCherryAbility();
            }

            Destroy(gameObject); 
        }
    }
}

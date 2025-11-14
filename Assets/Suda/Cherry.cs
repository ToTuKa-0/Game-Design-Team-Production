using UnityEngine;

public class Cherry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove1 player = other.GetComponent<PlayerMove1>();
            if (player != null)
            {
                player.ActivateCherryAbility();
            }

            Destroy(gameObject); 
        }
    }
}

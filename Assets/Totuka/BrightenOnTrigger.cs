using UnityEngine;

public class BrightenOnTrigger : MonoBehaviour
{
    public PlayerZoomLight zoomScript;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            zoomScript.BrightenPlayerArea(); // ‘S‘Ì‚ð–¾‚é‚­

            Destroy(gameObject);
        }
    }
}

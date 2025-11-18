using UnityEngine;
using System.Collections;

public class OnOffWall : MonoBehaviour
{
    public float onTime = 0.5f;   // “–‚½‚è”»’è‚ª‚ ‚éŽžŠÔ
    public float offTime = 1.0f;  // “–‚½‚è”»’è‚ª–³‚¢ŽžŠÔ

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(SwitchCollider());
    }

    IEnumerator SwitchCollider()
    {
        while (true)
        {
            col.enabled = true;
            sr.enabled = true;
            yield return new WaitForSeconds(onTime);

            col.enabled = false;
            sr.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }
}

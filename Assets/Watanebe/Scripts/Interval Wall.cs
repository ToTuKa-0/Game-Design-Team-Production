using UnityEngine;
using System.Collections;

public class IntervalWall : MonoBehaviour
{
    public float interval = 1f; // Ø‚è‘Ö‚¦‚éŠÔŠui•bj

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(SwitchState());
    }

    IEnumerator SwitchState()
    {
        while (true)
        {
            bool newState = !col.enabled;

            col.enabled = newState;   // “–‚½‚è”»’è ON/OFF
            sr.enabled = newState;    // ‰æ‘œ ON/OFF

            yield return new WaitForSeconds(interval);
        }
    }
}

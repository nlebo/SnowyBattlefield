using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    SpriteRenderer P,M;
    // Start is called before the first frame update
    void Awake()
    {
        P = transform.parent.GetComponent<SpriteRenderer>();
        M = GetComponent<SpriteRenderer>();
    }

    public void Change()
    {
        StartCoroutine(_Change());
    }

    IEnumerator _Change()
    {
        yield return new WaitForSeconds(0.1f);

        if (gameObject.layer != transform.parent.gameObject.layer)
            gameObject.layer = transform.parent.gameObject.layer;
        M.color = new Color(M.color.r * 0.7f + P.color.r * 0.3f, M.color.g * 0.7f + P.color.g * 0.3f, M.color.b * 0.7f + P.color.g * 0.3f, P.color.a);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-2f, 0.1f);
        StartCoroutine(DestroyOffScreen());
    }

    private IEnumerator DestroyOffScreen()
    {
        while (true)
        {
            if (transform.position.y > 10) Destroy(gameObject);
            yield return new WaitForSeconds(8);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerFish : BaseEnemy
{

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        float p_mass = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().mass;
        random = Random.value;
        rb.mass = (1 + random) * p_mass;

        /* Set my size. */
        normalizedScale = (new Vector3(transform.localScale.x, transform.localScale.y, 0)).normalized;
        Scale();
    }

    void Update()
    {
        Linear();
        Bursts();
    }

    private void Linear()
    {
        // if facing right, swim right, else swim left
        float velocity = random * (dir ? 0.003f : -0.002f);

        rb.velocity += new Vector2(velocity, 0);
        if (transform.position.x < -10f || transform.position.x > 10f || transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    private void Bursts()
    {
        // if facing right, swim right, else swim left
        float velocity = random * (dir ? 1f : -1f);

        if (Random.value > 0.9975f)
        {
            rb.velocity += new Vector2(velocity, 0.14f);
        }

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!rb) return;
        Rigidbody2D c_rb = c.GetComponent<Rigidbody2D>();
        if (!c_rb) return;
        if (c_rb.mass > rb.mass)
        {
            if (c.transform.CompareTag("Player"))
            {
                c.GetComponent<PlayerController>().Grow(rb.mass * 0.4f);
                ScoreManager.singleton.IncreaseScore(m_Score);
            }
            Destroy(gameObject);
        }
        else
        {
            if (c.transform.CompareTag("Player"))
            {
                Destroy(c);
                Debug.Log("lost");
                GameObject gm = GameObject.FindWithTag("GameController");
                gm.GetComponent<GameManager>().LoseGame();
                //c.GetComponent<PlayerController>().Grow(-rb.mass);
            }
            rb.mass += c_rb.mass;
            Scale();
        }
    }

    private void Scale()
    {
        transform.localScale = 0.05f * normalizedScale * Mathf.Log(rb.mass, 2);
    }
}

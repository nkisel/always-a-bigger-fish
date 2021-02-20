using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemy : BaseEnemy
{

    //private bool dir; //used to determine which direction the fish should be swimming in

    //private Rigidbody2D rb;

    //private SpriteRenderer sr;

    //private BoxCollider2D bc;

    /* Unit transform.scale of this fish. ||normalizedScale|| == 1. */
    //private Vector3 normalizedScale;

    //private int mass;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // rb.velocity = new Vector2(-1, 0.2f);
        mass = 40;
        float p_mass = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().mass;
        random = Random.value;
        if (random > 0.8f)
        {
            rb.mass = p_mass * (random + 0.7f);
        }
        else
        {
            rb.mass = (p_mass + 4000) * random;
        }

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
        float velocity = (0.3f + random) * (dir ? 0.002f : -0.002f);

        rb.velocity += new Vector2(velocity, 0);
        if (transform.position.x < -10f || transform.position.x > 10f || transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    private void Bursts()
    {
        // if facing right, swim right, else swim left
        float velocity = (0.33f + random) * (dir ? 1f : -1f);

        if (Random.value > 0.998f)
        {
            rb.velocity += new Vector2(velocity, 0.1f);
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
                c.GetComponent<PlayerController>().Grow(rb.mass * 0.3f);
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

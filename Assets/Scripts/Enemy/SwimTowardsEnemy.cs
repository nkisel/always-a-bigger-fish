using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimTowardsEnemy : BaseEnemy
{

    #region Private Variables
    /* Player to track */
    private GameObject player;
    #endregion

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        float p_mass = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().mass;
        // rb.velocity = new Vector2(-1, 0.2f);
        mass = 10;
        random = Random.value;
        rb.mass = p_mass + (random * 20);

        /* Set my size. */
        normalizedScale = (new Vector3(transform.localScale.x, transform.localScale.y, 0)).normalized;
        Scale();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Track(player);
    }

    private void Track(GameObject obj)
    {

        if (!player) {
          return;
        }
        rb.velocity += (Vector2) (obj.transform.position - transform.position) * 0.01f * (random + 0.2f);
        if (rb.position.x < -10f || rb.position.x > 10f || rb.position.y < -8f)
        {
            Destroy(gameObject);
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
                c.GetComponent<PlayerController>().Grow(rb.mass * 0.35f);
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

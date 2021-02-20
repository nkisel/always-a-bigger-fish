using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How many points the score will increase when the player a fish")]
    protected int m_Score;
    #endregion

    protected bool dir; //used to determine which direction the fish should be swimming in

    protected Rigidbody2D rb;

    protected SpriteRenderer sr;

    protected BoxCollider2D bc;

    /* Unit transform.scale of this fish. ||normalizedScale|| == 1. */
    protected Vector3 normalizedScale;

    /* Random number between 0 and 1 for this fish, for
     * when regenerating random numbers is unnecessary. */
    protected float random;

    /* TODO: update mass & resize fish accordingly. */
    protected int mass;

    //Determine the method of movement for this instance
    private double move;

    private float zigZagTimer = 2f;

    private bool zigZagUp = true;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // rb.velocity = new Vector2(-1, 0.2f);
        float p_mass = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().mass;
        mass = 1;
        random = Random.value;
        rb.mass = mass + (random * 2) + (p_mass * 0.005f);

        /* Set my size. */
        normalizedScale = (new Vector3(transform.localScale.x, transform.localScale.y, 0)).normalized;
        Scale();

        move = random;
    }

    // Update is called once per frame
    void Update()
    {
        if (move < .66) {
          Linear();
        } else {
          ZigZag();
        }
    }

    private void Linear() {
        // if facing right, swim right, else swim left
        float velocity = dir ? 0.01f : -0.01f;

        rb.velocity += new Vector2(velocity, 0);
        if (transform.position.x < -10f || transform.position.x > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void ZigZag() {

        //if facing right, swim right, else swim left
        float velocity = dir ? 0.01f : -0.01f;

        if (zigZagTimer < 0) {
          zigZagTimer = 2f;
          zigZagUp = !zigZagUp;
            rb.velocity = new Vector2(rb.velocity.x + (random - 0.5f), 0);
        } else {
          if (zigZagUp) { //swim up diagonally
            rb.velocity += new Vector2(velocity, .01f);
          } else { //swim down diagonally
            rb.velocity += new Vector2(velocity, -.01f);
          }
          zigZagTimer -= Time.deltaTime; //decrease time spent heading in certain direction
        }
        if (transform.position.x < -10f || transform.position.x > 10f)
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
            if (c.transform.CompareTag("Player")) {
              c.GetComponent<PlayerController>().Grow(rb.mass * 0.5f);
              ScoreManager.singleton.IncreaseScore(m_Score);
            }
            Destroy(gameObject);
        } else
        {
            if (c.transform.CompareTag("Player"))
            {
              Debug.Log("lost");
              GameObject gm = GameObject.FindWithTag("GameController");
              gm.GetComponent<GameManager>().LoseGame();
            }
            rb.mass += c_rb.mass;
            Scale();
        }
    }

    private void Scale()
    {
        transform.localScale = normalizedScale * Mathf.Log(rb.mass, 3);
    }

    public void direction(bool d) {
      //true = facing right, false = facing left
      dir = d;
    }
}

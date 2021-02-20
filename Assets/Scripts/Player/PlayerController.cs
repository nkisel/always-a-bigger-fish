using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Private Variables
    private Rigidbody2D rb;

    private SpriteRenderer sr;

    private BoxCollider2D bc;

    /* TODO: update mass & resize fish accordingly. */
    private int mass;
    public int Mass
    {
        get
        {
            return mass;
        }
    }

    /* Unit transform.scale of this fish. ||normalizedScale|| == 1. */
    private Vector3 normalizedScale;
    #endregion

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        normalizedScale = (new Vector3(transform.localScale.x, transform.localScale.y, 0)).normalized;
        rb.velocity = new Vector2(-1, 0);
        mass = 12;
        rb.mass = mass;
        Scale();
        ScoreManager.singleton.IncreaseScore(0);
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.001f);

        Vector2 control = new Vector2(horizontal, vertical);

        /* Stopping & turning is much faster than speeding up */
        if (Vector2.Dot(rb.velocity, control) < 0)
        {
            rb.velocity += 0.5f * control;
        }
        else
        {
            rb.velocity += 0.02f * control * Mathf.Max(1.9f - transform.localScale.y, 1f);
        }

        /* Flip the sprite according to the current direction */
        sr.flipX = (rb.velocity.x < 0);
    }
    #endregion

    #region Player Functions
    public void Grow(float m) {
        rb.mass += m;
        Scale(); // modifies size of sprite which also increases the collider
    }

    public void Scale()
    {
        transform.localScale = 0.05f * normalizedScale * Mathf.Log(rb.mass, 2);
    }

    #endregion
}

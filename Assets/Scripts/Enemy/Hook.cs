using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody2D rb;

    private float lifeTimer;

    private GameObject player;

    private float random;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTimer = 4f;
        player = GameObject.FindGameObjectWithTag("Player");
        random = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
      if (lifeTimer < 0) {
        Retract();
        if (rb.position.y > 10f) {
          Destroy(gameObject);
        }
      } else {
        lifeTimer -= Time.deltaTime;
        if (Random.value > .25)
        Track(player);
      }
    }

    private void Track(GameObject obj)
    {

        if (!player) {
          return;
        }
        rb.velocity = (Vector2) (obj.transform.position - transform.position) * 0.45f;

    }

    private void Retract() {
      rb.velocity = transform.up * 2f;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Rigidbody2D c_rb = c.GetComponent<Rigidbody2D>();
        if (!c_rb) return;
        bool play = c.transform.CompareTag("Player");
        Destroy(c.gameObject);
        if (play)
        {
          Debug.Log("Lost");
          GameObject gm = GameObject.FindWithTag("GameController");
          gm.GetComponent<GameManager>().LoseGame();
        }
    }
}

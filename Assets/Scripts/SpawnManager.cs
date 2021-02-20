using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void SpawnMethType(GameObject enemy);

public class SpawnManager : MonoBehaviour
{

    #region Editor Variables
    [SerializeField]
    [Tooltip("Bubbles & miscellaneous floating objects to spawn")]
    private GameObject[] m_MiscObjects;

    [SerializeField]
    [Tooltip("Type of enemy to spawn")]
    private EnemySpawnInfo[] m_EnemySpawnInfo;

    [SerializeField]
    [Tooltip("Right end of the arena.")]
    private int rightSpawn;

    [SerializeField]
    [Tooltip("Left end of the arena.")]
    private int leftSpawn;

    [SerializeField]
    [Tooltip("Top number in ratio any enemy will spawn on the right")]
    private int rightChance;

    [SerializeField]
    [Tooltip("Top number in ratio any enemy will spawn on the left")]
    private int leftChance;

    [SerializeField]
    [Tooltip("Top number in ratio that will spawn wave")]
    private int waveChance;

    [SerializeField]
    [Tooltip("Total height of the arena.")]
    private float spawnHeight;

    [SerializeField]
    [Tooltip("Total height of the arena.")]
    private float spawnWidth;

    [SerializeField]
    [Tooltip("Time in between hook spawns")]
    private float hookTime;
    #endregion

    #region Private Variables

    private float time;
    private List<SpawnMethType> spawnMethods;
    private List<GameObject> enemySpawnType;
    private float hookTimer;
    private float difficultyFactor;
    #endregion

    #region Initialization

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        difficultyFactor = 0.5f;
        spawnHeight = spawnHeight * 2;

        spawnMethods = new List<SpawnMethType>();
        /* Set probability of which spawn method */
        for (int i = 0; i < rightChance; i++)
        {
            spawnMethods.Add(SpawnRight);
        }
        for (int i = 0; i < leftChance; i++)
        {
            spawnMethods.Add(SpawnLeft);
        }
        for (int i = 0; i < waveChance; i++)
        {
            spawnMethods.Add(SpawnWave);
        }

        enemySpawnType = new List<GameObject>();
        /* Set probabilities of enemy type to spawn */
        foreach (EnemySpawnInfo enemy in m_EnemySpawnInfo)
        {
            for (int i = 0; i < enemy.EnemyChance; i++)
            {
                enemySpawnType.Add(enemy.EnemyObj);
            }

        }
        hookTimer = hookTime;
    }

    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        float value = Random.value;
        if (Random.value > 0.98f - (difficultyFactor * 0.01f))
        {
            SpawnBubble(m_MiscObjects[Random.Range(0, m_MiscObjects.Length)]);
            GameObject enemy = enemySpawnType[Random.Range(1, enemySpawnType.Count)];
            spawnMethods[Random.Range(0, spawnMethods.Count)](enemy);
            time += Time.deltaTime;
        }
        hookTimer -= Time.deltaTime;
        if (hookTimer < 0) {
          SpawnHook(enemySpawnType[0]);
          hookTimer = hookTime;
        }
        difficultyFactor = 0.5f + Time.deltaTime;
    }

    #endregion

    public float getDifficulty()
    {
        return difficultyFactor;
    }

    #region Spawn Strategies

    void SpawnBubble(GameObject enemy)
    {
        /* Position the fish randomly along the left side of the screen */
        float x = (Random.value + 0.3f) * spawnWidth;
        GameObject obj = Instantiate(enemy);

        obj.transform.position = new Vector2(x, -6);
    }

    void SpawnLeft(GameObject enemy)
    {

        GameObject fish = Instantiate(enemy);
        //fish.GetComponent<BaseEnemy>().direction(false);
        fish.GetComponent<BaseEnemy>().direction(true);

        Rigidbody2D fish_rb = fish.GetComponent<Rigidbody2D>();

        /* Position the fish randomly along the left side of the screen */
        float y = (Random.value - 0.5f) * spawnHeight;
        fish.transform.position = new Vector2(leftSpawn, y);

        /* Start the fish swimming to the right */
        fish_rb.velocity = new Vector2(5, 0);
    }

    void SpawnRight(GameObject enemy)
    {
        GameObject fish = Instantiate(enemy);
        fish.GetComponent<BaseEnemy>().direction(false);
        Rigidbody2D fish_rb = fish.GetComponent<Rigidbody2D>();

        /* Position the fish randomly along the right side of the screen */
        float y = (Random.value - 0.5f) * spawnHeight;
        fish.transform.position = new Vector2(rightSpawn, y);

        /* Start the fish swimming to the left */
        fish_rb.velocity = new Vector2(-1, 0);
    }

    void SpawnWave(GameObject enemy)
    {
        GameObject[] allFish = new GameObject[Random.Range(2, (int) (4 + difficultyFactor))];
        GameObject fish;
        float position = Random.value - 0.55f;
        for (int i = 0; i < allFish.Length; i++)
        {
            fish = Instantiate(enemy);

            Rigidbody2D fish_rb = fish.GetComponent<Rigidbody2D>();

            /* Position the fish randomly along the right side of the screen */
            float y = (position) * spawnHeight;
            fish.transform.position = new Vector2(rightSpawn, y);

            /* Start the fish swimming to the left */
            fish_rb.velocity = new Vector2(-1, 0);
            fish.GetComponent<BaseEnemy>().direction(false);

            position += 0.04f;
        }

    }

    void SpawnHook(GameObject enemy) {
      GameObject hook = Instantiate(enemy);
      Rigidbody2D hook_rb = hook.GetComponent<Rigidbody2D>();

      /* Position the hook randomly along the top of the screen */
      float x = (Random.value - 0.5f) * 9;
      hook.transform.position = new Vector2(x, 13);

      hook_rb.velocity = new Vector2(0, -4);
    }

    #endregion

}

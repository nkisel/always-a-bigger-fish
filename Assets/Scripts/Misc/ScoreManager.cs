using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager singleton;

    #region Editor Variables
    [SerializeField]
    [Tooltip("The UI text element that will display the player's score")]
    private Text m_UIText;

    [SerializeField]
    [Tooltip("The UI text element that backs the scorecard")]
    private Text m_UITextBackground;

    [SerializeField]
    [Tooltip("The player game object")]
    private GameObject m_PlayerModel;

    [SerializeField]
    [Tooltip("The UI text element that will display the player's mass")]
    private Text m_MassText;

    [SerializeField]
    [Tooltip("Mass accent text")]
    private Text m_MassAccent1;

    [SerializeField]
    [Tooltip("Mass accent text")]
    private Text m_MassAccent2;

    [SerializeField]
    [Tooltip("Mass accent text")]
    private Text m_MassAccent3;

    [SerializeField]
    [Tooltip("Sound to play on mass increase.")]
    private AudioClip m_massSound;
    #endregion

    #region Private Variables
    /* Current score. */
    private int m_curScore;

    /* Fish's rigidbody. */
    private Rigidbody2D rb;

    /* Last recorded mass. */
    private float lastMass;

    /* Last random number generated on score update. */
    private float lastRandom;

    /* Pixel offset from the fish's transform. */
    private Vector3 offset;

    /* Pixel offset from the fish's transform. */
    private Vector3 accentOffset;

    /* Text transform time. */
    private float transformTime;

    /* Audio source. */
    private AudioSource src;

    /* Last running coroutine. */
    private int coroutineID;
    #endregion

    #region Initialization
    private void Awake()
    {
    	if (singleton == null)
    	{
    		singleton = this;
    	} else if (singleton != this)
    	{
    		Destroy(gameObject);
    	}
    	m_curScore = 0;
        IncreaseScore(0);

        accentOffset = new Vector3(0.04f, 0.04f);
        offset = new Vector3(0.04f, 0.04f);
        lastMass = 0;
        lastRandom = 0.1f;
        transformTime = 0.6f;
        coroutineID = 0;

    }

    // Reset the score
    private void Start()
    {
        src = GetComponent<AudioSource>();
        UpdateMass();
    }
    #endregion

    #region Score Methods
    public void IncreaseScore(int amount) 
    {
    	m_curScore += amount;
    	m_UIText.text = "" + m_curScore;
        m_UITextBackground.text = "" + m_curScore;
        UpdateMass();
    }

    public float GetMass()
    {
        return lastMass;
    }

    public void UpdateMass()
    {
        //float roundedMass = Mathf.Round(m_PlayerModel.GetComponent<Rigidbody2D>().mass * 10.0f) * 0.1f;
        //m_MassText.text = "" + roundedMass.ToString();
        //Vector3 screenPos = m_PlayerModel.transform.position;
        // m_MassText.transform.position = screenPos + offset;
        float upward = m_PlayerModel.transform.localScale.y;
        offset = new Vector3(upward, upward * 2.5f);
        StopAllCoroutines();
        StartCoroutine(SteadyUpdate());
    }

    private IEnumerator SteadyUpdate()
    {
        // int myID = coroutineID;
        m_MassAccent1.text = "" + lastMass.ToString();
        Vector3 screenPos = m_PlayerModel.transform.position;
        m_MassAccent1.transform.position = screenPos + (accentOffset * lastRandom);

        m_MassAccent2.text = "" + lastMass.ToString();
        m_MassAccent2.transform.position = screenPos - (accentOffset * lastRandom);

        float elapsed = 0;
        float newMass = m_PlayerModel.GetComponent<Rigidbody2D>().mass;

        m_MassAccent3.text = "" + lastMass.ToString();
        m_MassAccent3.transform.position = screenPos - (accentOffset * transformTime);

        while (elapsed < transformTime)
        {
            elapsed += Time.deltaTime;
            float lerpMass = Mathf.Lerp(lastMass, newMass, elapsed / transformTime);
            float roundedMass = Mathf.Round(lerpMass); //* 10.0f) * 0.1f;
            m_MassText.text = "" + roundedMass.ToString();
            screenPos = m_PlayerModel.transform.position;
            m_MassText.transform.position = screenPos + offset;

            // if (myID != coroutineID) yield break;

            if (lastMass != roundedMass)
            {
                src.volume = Mathf.Lerp(0.9f, 0.1f, elapsed / transformTime);
                src.pitch = Mathf.Lerp(0.8f, 1.75f, elapsed / transformTime);
                src.Play();
            }

            lastMass = roundedMass;

            yield return null;
        }
        lastRandom = Random.value;
    }

    #endregion
}

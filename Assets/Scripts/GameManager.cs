using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance = null;

    private bool inMenu;

    private float lastCutoff;

  #region Unity_functions
  private void Awake() {
      if (Instance == null) {
          Instance = this;
      } else if (Instance != null) {
          Destroy(this.gameObject);
      }
      DontDestroyOnLoad(gameObject);
      inMenu = false;
      //lastCutoff = 700f;
  }

  #endregion

  #region Scene_transitions
  public void StartGame() {
        inMenu = false;
        SceneManager.LoadScene("SampleScene");
        //AudioPlayerManager.instance.Muffle(lastCutoff);
  }

  public void LoseGame() {
        SceneManager.LoadScene("LoseScene");
        Debug.Log("Loaded");
        inMenu = true;
        //lastCutoff = AudioPlayerManager.instance.Muffle(400);
  }

    public static bool playing()
    {
        return !Instance.inMenu;
    }

  //public void WinGame() {
  //  SceneManager.LoadScene("WinScene");
  //}

  //public void MainMenu() {
  //  SceneManager.LoadScene("MainMenu");
  //}
  #endregion
}

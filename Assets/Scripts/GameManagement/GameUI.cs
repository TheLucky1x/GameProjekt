using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    // Start is called before the first frame update
    void Start() {
      Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
    }

    // Update is called once per frame
    void Update() {
      if (gameIsOver) /* (Should maybe not be in the GameUI script, but because it's a smaller project is it okay) */ {
        if (Input.GetKeyDown (KeyCode.Space)) /* Checkin if spacebar is clicked */ {
          SceneManager.LoadScene (0); // Reloading the scene
        }
      }
    }

    void ShowGameWinUI() {
      OnGameOver (gameWinUI); // Start the function called "OnGameOver" and use the "gameWinUI" as reference
    }

    void ShowGameLoseUI() {
      OnGameOver (gameLoseUI); // Start the function called "OnGameOver" and use the "gameLoseUI" as reference
    }

    void OnGameOver (GameObject gameOverUI) /* use the object that we get from above */ {
      gameOverUI.SetActive (true);
      gameIsOver = true;
      Guard.OnGuardHasSpottedPlayer -=ShowGameLoseUI; // Unsubscribe to avoid errors
    }
}

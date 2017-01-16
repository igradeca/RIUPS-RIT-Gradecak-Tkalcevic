using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public GameObject gameStopCanvas;
    public GameObject pauseButton;
    public TheStack stackHimself;

	// Use this for initialization
	void Start () {
        stackHimself = GameObject.Find("TheStack").GetComponent<TheStack>();	
	}

    void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            PauseButtonPress();
        }
        if (stackHimself.isGameOver && gameStopCanvas.activeSelf == false) {
            ShowGameOver();
        }
    }

    public void ShowGameOver() {
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.0f;

        gameStopCanvas.SetActive(true);

        int userId = GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID;
        int scoreCount = GameObject.Find("TheStack").GetComponent<TheStack>().scoreCount;
        if (GameObject.Find("Main Camera").GetComponent<HighscoreScript>().CheckHighscore(userId, scoreCount)) {
            GameObject.Find("GameStoppedText").GetComponent<Text>().text = "GAME OVER\nNEW HIGHSCORE!";
        } else {
            GameObject.Find("GameStoppedText").GetComponent<Text>().text = "GAME OVER";
        }   
             
        GameObject.Find("ResumeButton").SetActive(false);
    }
         
    public void PauseButtonPress() {
        if (!gameStopCanvas.activeSelf) {
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.0f;

            gameStopCanvas.SetActive(true);
            pauseButton.SetActive(false);
        } else {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;

            gameStopCanvas.SetActive(false);
            pauseButton.SetActive(true);
        }
    }

    public void RestartLevel() {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(2);
    }

    public void BackToMainMenu() {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(1);
    }

}

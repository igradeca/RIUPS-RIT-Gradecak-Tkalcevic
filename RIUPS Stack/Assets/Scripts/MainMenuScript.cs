using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject MainMenuCanvas;
    public GameObject AchievementsCanvas;
    public GameObject DontDestroyStuffObject;

    public GameObject HighscoreCanvas;
    public GameObject ShopCanvas;

    // Use this for initialization
    void Start () {
        
    }

    public void StartScene(int sceneNumber) {
        if (!GameObject.Find("UserStuff")) {
            Instantiate(DontDestroyStuffObject);
        }
        SceneManager.LoadScene(sceneNumber);
    }

    public void MenuButtons(int canvasNum) {

        if (MainMenuCanvas.activeSelf) {
            MainMenuCanvas.SetActive(false);

            switch (canvasNum) {
                case 1:
                    AchievementsCanvas.SetActive(true);
                    LoadAchievements();
                    break;
                case 2:
                    HighscoreCanvas.SetActive(true);
                    break;
                case 3:
                    ShopCanvas.SetActive(true);
                    break;
            }

        } else {
            MainMenuCanvas.SetActive(true);
            switch (canvasNum) {
                case 1:
                    AchievementsCanvas.SetActive(false);
                    break;
                case 2:
                    HighscoreCanvas.SetActive(false);
                    break;
                case 3:
                    ShopCanvas.SetActive(false);
                    break;
            }
        }

    }

    public void ExitGame() {
        Application.Quit();
    }

    void LoadAchievements() {
        if (!GameObject.Find("UserStuff")) {
            Instantiate(DontDestroyStuffObject);
            GameObject.Find("UserStuff").GetComponent<AchievementScript>().MainMenuAchievements();
        } else if (GameObject.Find("AchievementList").transform.childCount == 0) {
            GameObject.Find("UserStuff").GetComponent<AchievementScript>().MainMenuAchievements();
        }
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public GameObject MainMenuCanvas;
    public GameObject AchievementsCanvas;
    public GameObject DontDestroyStuffObject;

    public GameObject HighscoreCanvas;
    public GameObject ShopCanvas;
    public ShopScript ShopStuff;

    void Start() {
        if (!GameObject.Find("UserStuff")) {
            Instantiate(DontDestroyStuffObject);
        }
        if (GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID == 0) {
            GameObject.Find("AchievementsButton").SetActive(false);
            GameObject.Find("HighscoreButton").SetActive(false);
            GameObject.Find("ShopButton").SetActive(false);
        } else {
            GameObject.Find("UsernameText").GetComponent<Text>().text = GameObject.Find("UserStuff").GetComponent<UserInfoScript>().Username;
        }

    }

    public void StartScene(int sceneNumber) {
        
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
                    LoadShop();
                    ShopStuff.SetColoursToBuy();
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

    void LoadShop() {
        if (GameObject.Find("ShopPanel").transform.childCount == 0) {
            ShopStuff.ShowShopItems();
        }
    }

}

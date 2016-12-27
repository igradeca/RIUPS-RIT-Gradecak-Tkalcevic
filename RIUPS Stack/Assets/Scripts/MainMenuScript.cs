using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject MainMenuCanvas;
    public GameObject AchievementsCanvas;
    public GameObject AchievementObject;

    // Use this for initialization
    void Start () {
        
    }

    public void StartScene(int sceneNumber) {
        if (!GameObject.Find("Achievements(Clone)")) {
            Instantiate(AchievementObject);
        }
        SceneManager.LoadScene(sceneNumber);
    }

    public void AchievementsButton() {

        if (MainMenuCanvas.activeSelf) {
            MainMenuCanvas.SetActive(false);
            AchievementsCanvas.SetActive(true);
            LoadAchievements();
        } else {
            MainMenuCanvas.SetActive(true);
            AchievementsCanvas.SetActive(false);
        }

    }

    public void ExitGame() {
        Application.Quit();
    }

    void LoadAchievements() {
        if (!GameObject.Find("Achievements(Clone)")) {
            Instantiate(AchievementObject);
            GameObject.Find("Achievements(Clone)").GetComponent<AchievementScript>().MainMenuAchievements();
        } else if (GameObject.Find("AchievementList").transform.childCount == 0) {
            GameObject.Find("Achievements(Clone)").GetComponent<AchievementScript>().MainMenuAchievements();
        }
    }

}

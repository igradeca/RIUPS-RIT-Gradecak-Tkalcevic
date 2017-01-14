using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StackClone;
using System.Collections.Generic;

public class AchievementScript : MonoBehaviour {

    public GameObject MenuTab;
    private AchievementElementScript[] Achievements;

    // Use this for initialization
    void Awake () {

        DontDestroyOnLoad(transform.gameObject);

        Achievements = new AchievementElementScript[5];
        Achievements[0] = new AchievementElementScript();
        Achievements[0].title = "BEGINNER";
        Achievements[0].description = "REACH COUNT TO 10";

        Achievements[1] = new AchievementElementScript();
        Achievements[1].title = "HUNDERED";
        Achievements[1].description = "REACH COUNT TO 100";

        Achievements[2] = new AchievementElementScript();
        Achievements[2].title = "COMBO MASTER";
        Achievements[2].description = "COMBO 10x";

        Achievements[3] = new AchievementElementScript();
        Achievements[3].title = "MAYBE NEXT TIME";
        Achievements[3].description = "SCORE 0 - ZERO!";

        Achievements[4] = new AchievementElementScript();
        Achievements[4].title = "NOOB";
        Achievements[4].description = "MAKE YOUR FIRST COMBO";
    }

    void OnEnable() {
        List<Dosezek> AchievementsList = new List<Dosezek>();
        AchievementsList = Dosezek.Brskaj();

        foreach (var item in AchievementsList) {
            Debug.Log(item.Naziv + " " + item.Nagrada);
        }
    }

    public void MainMenuAchievements() {
        for (int i = 0; i < Achievements.Length; i++) {
            GameObject AchievementTab = Instantiate(MenuTab) as GameObject;
            AchievementTab.name = "Achievement" + i;
            AchievementTab.transform.SetParent(GameObject.Find("AchievementList").transform, false);
            AchievementTab.transform.FindChild("Title").GetComponent<Text>().text = Achievements[i].title;
            AchievementTab.transform.FindChild("Description").GetComponent<Text>().text = Achievements[i].description;
            if (Achievements[i].status) {
                AchievementTab.transform.FindChild("StatusImage").gameObject.SetActive(true);
            }
            AchievementTab.transform.localPosition = new Vector3(0, 220 - (i * 110), 0);
        }
    }

    public void GameplayAchievements(int count, int combo) {
        switch (count) {
            case 0:
                if (!Achievements[3].status) {
                    Achievements[3].status = true;
                    UnlockShow(Achievements[3]);
                }
                break;
            case 10:
                if (!Achievements[0].status) {
                    Achievements[0].status = true;
                    UnlockShow(Achievements[0]);
                }
                break;
            case 100:
                if (!Achievements[1].status) {
                    Achievements[1].status = true;
                    UnlockShow(Achievements[1]);
                }
                break;
        }

        switch (combo) {
            case 10:
                if (!Achievements[2].status) {
                    Achievements[2].status = true;
                    UnlockShow(Achievements[2]);
                }
                break;
            case 5:
                if (!Achievements[4].status) {
                    Achievements[4].status = true;
                    UnlockShow(Achievements[4]);
                }
                break;
        }
    }

    void UnlockShow(AchievementElementScript unlockedAchievement) {

        GameObject unlockTab = GameObject.Find("AchievementUnlocked");

        unlockTab.transform.FindChild("Title").GetComponent<Text>().text = unlockedAchievement.title;
        unlockTab.GetComponent<Animator>().Play("AchievementUnlockAnim");
    }

}

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

        

        //Achievements = new AchievementElementScript[5];
    }

    public void MainMenuAchievements() {

        List<Dosezek> AchievementsList = new List<Dosezek>();
        AchievementsList = Dosezek.Brskaj("", -1, -1, Manager.Current.User.Id);

        // ili ovak
        List<int> AchievementIDs = new List<int>();
        AchievementIDs = Dosezek.UporabnikMaDosezek( Manager.Current.User.Id );

        int counter = 0;
        foreach (var item in AchievementsList) {
            GameObject AchievementTab = Instantiate(MenuTab) as GameObject;
            AchievementTab.name = "Achievement" + counter;
            AchievementTab.transform.SetParent(GameObject.Find("AchievementList").transform, false);
            AchievementTab.transform.FindChild("Title").GetComponent<Text>().text = item.Naziv;
            AchievementTab.transform.FindChild("Description").GetComponent<Text>().text = item.Opis;
            /*
            if (Achievements[counter].status) {
                AchievementTab.transform.FindChild("StatusImage").gameObject.SetActive(true);
            }
            */
            AchievementTab.transform.localPosition = new Vector3(0, 220 - (counter * 110), 0);
            counter++;
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

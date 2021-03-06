using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StackClone;
using System.Collections.Generic;

public class AchievementScript : MonoBehaviour {

    public GameObject MenuTab;
    private AchievementElementScript[] Achievements;
    private List<int> AchievementIDs = new List<int>();

    // Use this for initialization
    void Awake () {
                
        //AchievementIDs = Dosezek.UporabnikMaDosezek(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID);

        //Achievements = new AchievementElementScript[5];
    }

    public void GetAchievementsForUser() {
        if (GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID != 0) {
            AchievementIDs = Dosezek.UporabnikMaDosezek(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID);
            foreach (var item in AchievementIDs) {
                Debug.Log(AchievementIDs);
            }
        }        
    }

    public void MainMenuAchievements() {

        List<Dosezek> AchievementsList = new List<Dosezek>();
        AchievementsList = Dosezek.Brskaj();

        AchievementIDs = Dosezek.UporabnikMaDosezek(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID);

        int counter = 0;
        foreach (var item in AchievementsList) {
            GameObject AchievementTab = Instantiate(MenuTab) as GameObject;
            AchievementTab.name = "Achievement" + counter;
            AchievementTab.transform.SetParent(GameObject.Find("AchievementList").transform, false);
            AchievementTab.transform.FindChild("Title").GetComponent<Text>().text = item.Naziv;
            AchievementTab.transform.FindChild("Description").GetComponent<Text>().text = item.Opis;
            
            if (AchievementIDs.Count != 0 && AchievementIDs.Contains(item.Id)) {
                AchievementTab.transform.FindChild("StatusImage").gameObject.SetActive(true);
            }
            
            AchievementTab.transform.localPosition = new Vector3(0, 220 - (counter * 110), 0);
            counter++;
        }
    }
    // od 4 do 8
    public void GameplayAchievements(int count, int combo) {   

        switch (count) {
            case 0:
                if (!AchievementIDs.Contains(7)) {
                    Dosezek.DodajDosezekUporabnika(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID, 7);
                    List<Dosezek> unlock = Dosezek.Brskaj("", 7);
                    UnlockShow(unlock.Find(x => x.Id == 7));
                }
                break;
            case 10:
                if (!AchievementIDs.Contains(4)) {
                    Dosezek.DodajDosezekUporabnika(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID, 4);
                    List<Dosezek> unlock = Dosezek.Brskaj("", 4);
                    UnlockShow(unlock.Find(x => x.Id == 4));
                }
                break;
            case 100:
                if (!AchievementIDs.Contains(5)) {
                    Dosezek.DodajDosezekUporabnika(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID, 5);
                    List<Dosezek> unlock = Dosezek.Brskaj("", 5);
                    UnlockShow(unlock.Find(x => x.Id == 5));
                }
                break;
        }

        switch (combo) {
            case 10:
                if (!AchievementIDs.Contains(6)) {
                    Dosezek.DodajDosezekUporabnika(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID, 6);
                    List<Dosezek> unlock = Dosezek.Brskaj("", 6);
                    UnlockShow(unlock.Find(x => x.Id == 6));
                }
                break;
            case 5:
                if (!AchievementIDs.Contains(8)) {
                    Dosezek.DodajDosezekUporabnika(GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID, 8);
                    List<Dosezek> unlock = Dosezek.Brskaj("", 8);
                    UnlockShow(unlock.Find(x => x.Id == 8));
                }
                break;
        }
    }

    void UnlockShow(Dosezek unlocked) {

        GameObject unlockTab = GameObject.Find("AchievementUnlocked");

        unlockTab.transform.FindChild("Title").GetComponent<Text>().text = unlocked.Naziv;
        unlockTab.GetComponent<Animator>().Play("AchievementUnlockAnim");
    }

}

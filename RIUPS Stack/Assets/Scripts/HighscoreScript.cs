using UnityEngine;
using System.Collections;
using StackClone;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighscoreScript : MonoBehaviour {

    // poziva se u main menu-u na button click "HighscoreButton"
    public void PopulateHighscore() {

        Text userHighscoreList = GameObject.Find("UserList Text").GetComponent<Text>();
        Text scoreHighscoreList = GameObject.Find("ScoreList Text").GetComponent<Text>();
        List<TopRezultat> playersList = new List<TopRezultat>();
        playersList = TopRezultat.GetTopRezultati(10);
        
        int counter = 1;
        userHighscoreList.text = "";
        scoreHighscoreList.text = "";
        foreach (var player in playersList) {
            userHighscoreList.text += counter + ". " + player.Uporabnik.Uporabnisko + "\n";
            scoreHighscoreList.text += player.Rezultat + "\n";
            counter++;
        }
    }

    public bool CheckHighscore(int userId, int score) {

        List<TopRezultat> playersList = new List<TopRezultat>();
        playersList = TopRezultat.GetTopRezultati(10);

        if (score > playersList[playersList.Count - 1].Rezultat) {
            TopRezultat.Dodaj(userId, score);
            TopRezultat.BrisanjeSlabih(10);
            return true;
        }
        return false;
    }

}

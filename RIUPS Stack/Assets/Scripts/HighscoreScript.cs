using UnityEngine;
using System.Collections;
using StackClone;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighscoreScript : MonoBehaviour {

    public Text HighscoreList;

    void OnEnable() {
        PopulateHighscore();
    }

    public void PopulateHighscore() {

        List<TopRezultat> PlayersList = new List<TopRezultat>();
        PlayersList = TopRezultat.GetTopRezultati(10);
        
        int counter = 1;
        HighscoreList.text = "";
        foreach (var player in PlayersList) {
            string row = counter + ". " + player.Uporabnik.Uporabnisko + "   " + player.Rezultat;
            HighscoreList.text += row + "\n";
            counter++;
        }
    }

}

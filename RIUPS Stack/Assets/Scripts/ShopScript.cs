using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StackClone;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    public void ShowShopItems() {
        UsersCoins();
        List<ShopItem> shopItems = new List<ShopItem>();
        shopItems = ShopItem.Brskaj();

        foreach (var item in shopItems) {
            Debug.Log(item.Id + " " + item.Naziv);
        }
    }

    private void UsersCoins() {
        int coins = GameObject.Find("UserStuff").GetComponent<UserInfoScript>().coins;
        GameObject.Find("CoinsText").GetComponent<Text>().text = "COINS: " + coins.ToString();
    }

    public void UpdateCoins(int coins) {

        int id = GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID;
        List<Uporabnik> user = new List<Uporabnik>();
        user = Uporabnik.Brskaj(id);
        user[0].Kovanc = coins;

        Uporabnik.Update(user[0]);
    }

}

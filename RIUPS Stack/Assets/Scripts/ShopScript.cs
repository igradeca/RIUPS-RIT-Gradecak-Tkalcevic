using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StackClone;
using UnityEngine.UI;
using System.Linq;

public class ShopScript : MonoBehaviour {

    public GameObject ColoursToBuy;

	// Use this for initialization
	void Start () {

    }

    public void SetColoursToBuy() {
        if (!GameObject.Find("ColoursToBuy")) {
            GameObject colours = Instantiate(ColoursToBuy) as GameObject;
            colours.name = "ColoursToBuy";
        }
        
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

    public void dodajArtikle() {

        List<string> existing = ShopItem.Brskaj().Select( x => x.Naziv ).ToList();

        ShopItem item1 = new ShopItem();
        ShopItem item2 = new ShopItem();
        ShopItem item3 = new ShopItem();

        item1.Naziv = "Le Classic";
        item2.Naziv = "Red";
        item3.Naziv = "Grasshopper";

        if (!existing.Contains(item1.Naziv))
            ShopItem.Dodaj(item1);
        if ( !existing.Contains( item2.Naziv ) )
            ShopItem.Dodaj(item2);
        if ( !existing.Contains( item3.Naziv ) )
            ShopItem.Dodaj(item3);

        List<ShopItem> items = new List<ShopItem>();
        items = ShopItem.Brskaj();

        foreach (var item in items) {
            Debug.Log(item.Id + " " + item.Naziv);
        }
    }

}

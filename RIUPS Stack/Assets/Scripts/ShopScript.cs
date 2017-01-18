using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StackClone;
using UnityEngine.UI;
using System.Linq;

public class ShopScript : MonoBehaviour {

    public GameObject ColoursToBuy;
    public GameObject ShopItemPrefab;
    public Color32 bojabogati;

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
        Instantiate(ColoursToBuy);

        int counter = 0;
        foreach (var item in shopItems) {
            GameObject shopItemInst = Instantiate(ShopItemPrefab) as GameObject;     
            shopItemInst.name = item.Naziv;
            shopItemInst.transform.SetParent(GameObject.Find("ShopPanel").transform, false);
            shopItemInst.transform.FindChild("ShopItemText").GetComponent<Text>().text = item.Naziv;
            Color32 mojaColor = GameObject.Find("ColoursToBuy(Clone)").transform.FindChild(item.Naziv).GetComponent<ColourElementScript>().coloursPallette[0];
            Debug.Log(mojaColor);
            shopItemInst.GetComponent<RawImage>().color = mojaColor;
            shopItemInst.transform.localPosition -= new Vector3(0.0f, (90 * counter), 0.0f);

            counter++;
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

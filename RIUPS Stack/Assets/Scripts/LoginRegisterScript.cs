using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StackClone;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoginRegisterScript : MonoBehaviour {

    public GameObject loginCanvas;
    public GameObject registerCanvas;
    public GameObject initialCanvas;
    public GameObject messageBox;
    public GameObject loginUserInfo;

    public Text username;
    public Text password;
    public Text regName;
    public Text regLastName;
    public Text regEmail;
    public Text regUsername;
    public Text regPassword;
    public Text errorText;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoginPanelBtnClick()
    {
        initialCanvas.SetActive( false );
        loginCanvas.SetActive( true );
    }

    public void RegisterPanelBtnClick()
    {
        initialCanvas.SetActive( false );
        registerCanvas.SetActive( true );
    }

    public void LoginBtnClick()
    {
        if ( username.text == "" || password.text == "" )
        {
            errorText.text = "Wrong input!\nYou left one of the fields empty!";
            messageBox.SetActive( true );
            return;
        }
        else
        {
            List<Uporabnik> uporabnikList = Uporabnik.Brskaj( -1, username.text );
            if (uporabnikList == null)
            {
                errorText.text = "Error!\nFailed to reach database.";
                messageBox.SetActive( true );
                return;
            }
            else if (uporabnikList.Count == 0)
            {
                errorText.text = "Error!\nUsername not found.";
                messageBox.SetActive( true );
                return;
            }
            Uporabnik uporabnik = uporabnikList.Count == 1 ? uporabnikList[0] : new Uporabnik();
            bool? tmpPassValid = Uporabnik.PotrdiGeslo( uporabnik.Id, password.text );
            bool passwordValid = (tmpPassValid == null || tmpPassValid == false) ? false : true;
            if ( !passwordValid )
            {
                errorText.text = "Error!\nWrong password.";
                messageBox.SetActive( true );
                return;
            }
            else if ( passwordValid )
            {
                Instantiate(loginUserInfo);
                GameObject.Find("UserStuff").GetComponent<UserInfoScript>().Username = uporabnik.Uporabnisko;
                GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID = uporabnik.Id;

                SceneManager.LoadScene( "main menu" );
            }
        }
    }

    public void RegisterBtnClick()
    {
        if ( regUsername.text == "" || regPassword.text == "" )
        {
            errorText.text = "Error!\nYou left username and/or password field empty.";
            messageBox.SetActive( true );
            return;
        }
        else
        {
            List<Uporabnik> uporabnikList = Uporabnik.Brskaj( -1, regUsername.text );
            if ( uporabnikList == null )
            {
                errorText.text = "Error!\nFailed to reach database.";
                messageBox.SetActive( true );
                return;
            }
            else if ( uporabnikList.Count > 0 )
            {
                errorText.text = "Error!\nUsername already in use.";
                messageBox.SetActive( true );
                return;
            }
            Uporabnik novi = new Uporabnik( -1, regName.text, regLastName.text,
            regEmail.text, regUsername.text, Uporabnik.TipUporabnika.Uporabnik, 0 );
            int id = Uporabnik.Dodaj( novi, regPassword.text );
            if (id == -1)
            {
                errorText.text = "Error!\nFailed to reach database.";
                messageBox.SetActive( true );
                return;
            }
            else
            {
                SceneManager.LoadScene("Login");
            }
            
        }
    }

    public void ErrorBtnClick()
    {
        messageBox.SetActive( false );
    }

    public void BackBtnClick ()
    {
        initialCanvas.SetActive( true );
        registerCanvas.SetActive( false );
        loginCanvas.SetActive( false );
    }

}

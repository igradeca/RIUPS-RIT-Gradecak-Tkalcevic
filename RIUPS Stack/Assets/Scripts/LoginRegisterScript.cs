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
    public InputField password;
    public Text regName;
    public Text regLastName;
    public Text regEmail;
    public Text regUsername;
    public InputField regPassword;
    public Text errorText;

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
            Debug.Log(password.text);
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
                FillUserStuff(uporabnik.Id, uporabnik.Uporabnisko, uporabnik.Kovanc);
                SceneManager.LoadScene( "main menu" );
            }
        }
    }

    private void FillUserStuff(int id, string username, int coins) {
        GameObject.Find("UserStuff").GetComponent<UserInfoScript>().userID = id;
        GameObject.Find("UserStuff").GetComponent<UserInfoScript>().Username = username;
        GameObject.Find("UserStuff").GetComponent<UserInfoScript>().coins = coins;
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

    public void OfflineModeClick() {
        SceneManager.LoadScene("main menu");
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


/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Current
    {
        get
        {
            if ( applicationIsQuitting )
            {
                Debug.LogWarning( "[Singleton] Instance '" + typeof( T ) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null." );
                return null;
            }

            lock ( _lock )
            {
                if ( _instance == null )
                {
                    _instance = (T)FindObjectOfType( typeof( T ) );

                    if ( FindObjectsOfType( typeof( T ) ).Length > 1 )
                    {
                        Debug.LogError( "[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it." );
                        return _instance;
                    }

                    if ( _instance == null )
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof( T ).ToString();

                        DontDestroyOnLoad( singleton );

                        Debug.Log( "[Singleton] An instance of " + typeof( T ) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad." );
                    }
                    else
                    {
                        Debug.Log( "[Singleton] Using instance already created: " +
                            _instance.gameObject.name );
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}

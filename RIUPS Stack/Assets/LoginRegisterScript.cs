using UnityEngine;
using System.Collections;

public class LoginRegisterScript : MonoBehaviour {

    public GameObject loginCanvas;
    public GameObject registerCanvas;
    public GameObject initialCanvas;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoginBtnClick()
    {
        initialCanvas.SetActive( false );
        loginCanvas.SetActive( true );
    }

    public void RegisterBtnClick()
    {
        initialCanvas.SetActive( false );
        registerCanvas.SetActive( true );
    }
}

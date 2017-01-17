using UnityEngine;
using System.Collections;
using StackClone;
using System.Collections.Generic;

public class UserInfoScript : MonoBehaviour {

    public string Username;
    public int userID;
    public int coins;

	// Use this for initialization
	void Awake () {

        gameObject.name = "UserStuff";
        DontDestroyOnLoad(transform.gameObject);

    }

}

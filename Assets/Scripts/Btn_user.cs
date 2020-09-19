using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataBank;

public class Btn_user : MonoBehaviour {

	[SerializeField] Text activeUser;
	// Use this for initialization
	void Start () {
		UserDb user = new UserDb ();
		activeUser.text = "Hai, " + user.getActiveUser ()._name;
		user.close ();
	}

}

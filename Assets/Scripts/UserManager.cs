using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DataBank;

public class UserManager : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;
	public InputField inputName;

	private HistoryDb historyDb;
	private UserDb userDb;
	private int changeCounter = 0;
	private int lastChangeCounter;

	// Use this for initialization
	void Start () {
		userDb = new UserDb ();
		historyDb = new HistoryDb ();
		lastChangeCounter = changeCounter;
		getData ();
	}

	// Update is called once per frame
	void Update () {

		if(changeCounter == lastChangeCounter) {
			// No change since last update!
			return;
		}

		lastChangeCounter = changeCounter;
		getData ();
	}

	void getData(){

		while(this.transform.childCount > 0) {
			Transform c = this.transform.GetChild(0);
			c.SetParent(null);  // Become Batman
			Destroy (c.gameObject);
		}

		//UserEntity activeUser = userDb.getActiveUser();
		List<UserEntity> users = userDb.getAllUser();

		foreach(UserEntity user in users) {
			GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
			go.transform.SetParent(this.transform);
			go.transform.Find ("Username").GetComponent<Text>().text = user._name;
			go.transform.Find ("Status").GetComponent<Text>().text = user._status;
			go.transform.Find ("Activate").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=>ActivateUser(user._id));
			go.transform.Find ("Delete").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=>DeleteUser(user._id));
			if (user._status == "active") {
				go.transform.Find ("Activate").gameObject.SetActive (false);
				go.transform.Find ("Delete").gameObject.SetActive (false);
			}
		}
	}

	public void AddUser() {
		if (inputName == null || inputName.text == "") 
			return;
		
		userDb.addData (new UserEntity (inputName.text));
		changeCounter++;
	}

	public void ActivateUser(string id){
		userDb.nonActivateAllUsers ();
		userDb.activateUser (id);
		changeCounter++;
	}

	public void DeleteUser(string id){
		historyDb.deleteDataByUserId (id);
		userDb.deleteDataById (id);
		changeCounter++;
	}

	public void CloseDB(){
		userDb.close ();
		historyDb.close ();
	}
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DataBank;

public class HistoryManager : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;
	private HistoryDb historyDb;
	private UserDb userDb;

	private int changeCounter = 0;
	private int lastChangeCounter;

	//ScoreManager scoreManager;

	// Use this for initialization
	void Start () {
		historyDb = new HistoryDb ();
		userDb = new UserDb ();
		//scoreManager = GameObject.FindObjectOfType<ScoreManager>();
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

		UserEntity user = userDb.getActiveUser();
		List<HistoryEntity> histories = historyDb.getDataByUserId(user._id);

		foreach(HistoryEntity history in histories) {
			GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
			go.transform.SetParent(this.transform);
			go.transform.Find ("Username").GetComponent<Text>().text = history._userId;
			go.transform.Find ("Type").GetComponent<Text>().text = history._type;
			go.transform.Find ("Datetime").GetComponent<Text>().text = history._datetime;
			go.transform.Find ("Score").GetComponent<Text>().text = history._score;
		}
	}

	public void AddHistory() {
		UserEntity user = userDb.getActiveUser();
		historyDb.addData(new HistoryEntity(user._id, "angka", "100"));
		changeCounter++;
	}

	public void CloseDB(){
		userDb.close ();
		historyDb.close ();
	}
}

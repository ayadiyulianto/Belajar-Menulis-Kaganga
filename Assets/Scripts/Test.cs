using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataBank;

public class Test : MonoBehaviour {

	public GameObject[] allQuestions;
	public GameObject scorePanel;
	public Text scoreText;
	public GameObject responsePanel;
	public Text response;
	public Text rightAnswer;
	private GameObject[] _10questions;
	private int score;
	private int number;
	public string typeSoal;

	// Use this for initialization
	void Start () {
		ResetTest ();
	}

	private void Get10Questions(){
		GameObject[] tempquestions = allQuestions;

		//Shuffle array
		for (int i = 0; i < tempquestions.Length; i++) {
			int random = Random.Range (0, tempquestions.Length);
			GameObject tgo = tempquestions [random];
			tempquestions [random] = tempquestions [i];
			tempquestions [i] = tgo;
		}

		for (int i = 0; i < _10questions.Length; i++) {
			_10questions [i] = tempquestions [i];
		}
	}
	
	private void ShowQuestion(){

		if (_10questions == null) {
			return;
		}
		
		foreach (GameObject question in allQuestions) {
			if (question != null)
				question.SetActive (false);
		}

		_10questions [number].SetActive (true);
	}

	public void CheckAnswer(bool answer){

		_10questions [number].SetActive (false);
		responsePanel.SetActive (true);
		if (answer) {
			score += 10;
			response.text = "BENAR";
		} else {
			response.text = "SALAH";
		}
		rightAnswer.text = "Jawaban Benar = " + _10questions [number].GetComponent<Question>().rightAnswer;

	}

	public void NextQuestion(){
		
		number += 1;
		responsePanel.SetActive (false);
		if (number < _10questions.Length) {
			ShowQuestion ();
		} else {
			ShowResult ();
		}
	}

	private void ShowResult(){

		UserDb userDb = new UserDb ();
		HistoryDb historyDb = new HistoryDb ();
		UserEntity user = userDb.getActiveUser();
		if (user != null || user._id != "") {
			historyDb.addData (new HistoryEntity (user._id, typeSoal, "" + score));
		}
		userDb.close ();
		historyDb.close ();
		scorePanel.SetActive (true);
		scoreText.text = ""+score;
	}

	public void ResetTest(){
		scorePanel.SetActive (false);
		responsePanel.SetActive (false);

		_10questions = new GameObject[3];
		score = 0;
		number = 0;
		Get10Questions ();
		ShowQuestion ();
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour {

	public void LoadScene(string scene){
		SceneManager.LoadScene (scene);
	}
	//Load the current letter
	public void LoadLetter (int i)
	{
		if (i == null) {
			return;
		}
		WritingHandler.currentLetterIndex = i;
		SceneManager.LoadScene ("MenulisHurufPokok");
	}
	//Load the current letter

	public void LoadNgimbang (int i)
	{
		if (i == null) {
			return;
		}
		WritingHandler.currentLetterIndex = i;
		SceneManager.LoadScene ("MenulisHurufNgimbang");
	}

	public void LoadSandangan (int i)
	{
		if (i == null) {
			return;
		}
		WritingHandler.currentLetterIndex = i;
		SceneManager.LoadScene ("MenulisSandangan");
	}

	public void LoadKata (int i)
	{
		if (i == null) {
			return;
		}
		WritingHandler.currentLetterIndex = i;
		SceneManager.LoadScene ("MenulisKata");
	}

	public void LoadAngka (int i)
	{
		if (i == null) {
			return;
		}
		WritingHandler.currentLetterIndex = i;
		SceneManager.LoadScene ("MenulisAngka");
	}

}
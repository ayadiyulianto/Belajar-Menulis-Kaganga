using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// Implement your game events in this script
public class Events : MonoBehaviour
{
	private WritingHandler writingHandler;
	private General general;

	void Start ()
	{
			//Setting up the writingHandler reference
			GameObject letters = HierrachyManager.FindActiveGameObjectWithName ("Letters");
			if (letters != null)
					writingHandler = letters.GetComponent<WritingHandler> ();
			GameObject gen = HierrachyManager.FindActiveGameObjectWithName ("General");
			if (gen != null)
				general = gen.GetComponent<General> ();
	}

	//Load the next letter
	public void LoadTheNextLetter (object ob)
	{
			if (writingHandler == null) {
					return;
			}
			writingHandler.LoadNextLetter ();
	}

	//Load the previous letter
	public void LoadThePreviousLetter (object ob)
	{
			if (writingHandler == null) {
					return;
			}
			writingHandler.LoadPreviousLetter ();

	}

	//Load the current letter
	public void LoadLetter (Object ob)
	{
			if (ob == null) {
					return;
			}
			WritingHandler.currentLetterIndex = int.Parse (ob.name.Split ('-') [1]);
			SceneManager.LoadScene ("MenulisHuruf");
	}

	//Erase the current letter
	public void EraseLetter (Object ob)
	{
			if (writingHandler == null) {
					return;
			}
			writingHandler.RefreshProcess ();

	}

	public void LoadSoundLetter(Object ob)
	{
		if (writingHandler == null) {
			return;
		}
		writingHandler.PlaySoundLetter ();
	}

	public void BackSoundOnOff(Object ob)
	{
		if (general == null) {
			return;
		}
		general.BackSoundOnOff ();
	}

	public void ReadLetter(Object ob)
	{
		if (writingHandler == null) {
			return;
		}
		writingHandler.LoadLetter ();
	}

	public void WriteLetter(Object ob)
	{
		if (writingHandler == null) {
			return;
		}
		writingHandler.WriteLetter ();
	}

	//Load alphabet menu
	public void LoadMenuHuruf(Object ob){
		SceneManager.LoadScene ("MenuHuruf");
	}

	public void LoadMenuSandangan(Object ob){
		SceneManager.LoadScene ("MenuSandangan");
	}

	public void LoadMenuKata(Object ob){
		SceneManager.LoadScene ("MenuKata");
	}
	public void LoadMenuAngka(Object ob){
		SceneManager.LoadScene ("MenuAngka");
	}
}
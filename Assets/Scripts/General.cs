using UnityEngine;
using System.Collections;

//General class
public class General : MonoBehaviour
{
	private static General instance;
	public bool backSoundOn = true;

	// Use this for initialization
	void Start ()
	{
			if (instance == null) {
					instance = this;
					DontDestroyOnLoad (gameObject);
			} else {
					Destroy (gameObject);
			}
	}
		
	public void BackSoundOnOff()
	{
		if (backSoundOn) {
			gameObject.GetComponent<AudioSource> ().Pause ();
			backSoundOn = false;
		} else {
			gameObject.GetComponent<AudioSource> ().Play ();
			backSoundOn = true;
		}

	}

}

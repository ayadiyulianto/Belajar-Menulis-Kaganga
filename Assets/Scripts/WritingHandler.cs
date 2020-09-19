using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic; 

public class WritingHandler : MonoBehaviour
{
		public GameObject[] letters;//the letters list from A-Z
		public GameObject[] readLetters;
		public static int currentLetterIndex;//the index of the current letter
		private bool clickBeganOrMovedOutOfLetterArea;//does the click began or moved out of letter area
		private int previousTracingPointIndex;//the index of the previous letter
		private ArrayList currentTracingPoints;//holds the indexes of the tracing points
		private Vector3 previousPosition, currentPosition = Vector3.zero;//thre click previous position
		public GameObject lineRendererPrefab;//the line renderer prefab
		public GameObject circlePointPrefab;//the circle point prefab
		private GameObject currentLineRender = null;//current line renderer gameobject
		public Material drawingMaterial;
		private bool letterDone = false;
		private bool setRandomColor = true;
		private bool clickStarted;//uses with mouse input drawings,when drawing clickStarted
		public Transform hand;
		public bool showCursor;
		public AudioClip cheeringSound;
		public AudioClip positiveSound;
		public AudioClip wrongSound;
		public Text text;
		private Vector3 tmpMousePosition;
		private IEnumerator coroutine;
		public GameObject eraseButton;
		public GameObject PanelStars;
		public GameObject[] stars;
		private int trial = 0;
		private float timeStart = 0.0f;
		public GameObject gen;

		IEnumerator Start ()
		{   
				Cursor.visible = showCursor;//show curosr or hide
				currentTracingPoints = new ArrayList ();//initiate the current tracing points
				LoadLetter ();
				yield return 0;
		}


		//Executes Every Single Frame
		void Update ()
		{
				if (letterDone) {//if the letter is done then skip the next
						return;
				}

				if (Input.GetKeyDown (KeyCode.Escape)) {//on escape pressed
						BackToMenu ();//back to menu 
				}

				RaycastHit2D hit2d = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);//raycast of touch

				if (hit2d.collider != null) {//ketika tangan menyentuh collider
						if (Input.GetMouseButtonDown (0)) {
								TouchLetterHandle (hit2d.collider.gameObject, true, Camera.main.ScreenToWorldPoint (Input.mousePosition));//touch collide with letter (drawing begin);
								clickStarted = true;
						} else if (clickStarted) {
								TouchLetterHandle (hit2d.collider.gameObject, false, Camera.main.ScreenToWorldPoint (Input.mousePosition));//touch collide with letter (drawing move);
						}  
				}
				if (Input.GetMouseButtonUp (0)) {

						if (clickStarted) {
								EndTouchLetterHandle ();
								clickStarted = false;
								clickBeganOrMovedOutOfLetterArea = false;
						}
				}

				if (hand != null) {
					if (tmpMousePosition != Input.mousePosition) {
						if (coroutine != null) {
							StopCoroutine (coroutine);
						}
						//drag the hand on screen
						Vector3 clickPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						clickPosition.z = -6;
						hand.position = clickPosition;
					}
								
				}
		}


	//Letter touch hanlder
	private void TouchLetterHandle (GameObject ob, bool isTouchBegan, Vector3 touchPos)
	{
		string obTag = ob.tag;// name of button that ray hit it
		bool flag1 = (obTag == "TracingPoint");
		bool flag2 = (obTag == "Letter" || obTag == "TracingPoint" || obTag == "Background") && currentLineRender != null;

		if (flag1 && isTouchBegan) {//Touch Began
			TracingPoint tracingPoint = ob.GetComponent<TracingPoint> ();//get the tracing point
			TracingPart [] tracingParts = letters [currentLetterIndex].GetComponents<TracingPart> ();//get the tracing parts of the current letter
			foreach (TracingPart part in tracingParts) {//check apakah tracing parts sesuai prioritas
				if (!part.succeded) {
					if (PreviousLettersPartsSucceeded (part, tracingParts)) { //check apakah tracing point mulai dr 0 
						if (tracingPoint.index != part.order [0]) {
							return;
						}
					}
				}
			}

			int currentindex = tracingPoint.index;//get the tracing point index
			if (currentindex != previousTracingPointIndex) {
				currentTracingPoints.Add (currentindex);//add the current tracing point to the list
				previousTracingPointIndex = currentindex;//set the previous tracing point
				if (timeStart == 0.0f) {
					timeStart = Time.time;
				}
				if (currentLineRender == null) {
					currentLineRender = (GameObject)Instantiate (lineRendererPrefab);//instaiate new line
					if (setRandomColor) {
						currentLineRender.GetComponent<LineRendererAttributes> ().SetRandomColor ();//set a random color for the line
						setRandomColor = false;
					}
				}

				Vector3 currentPosition = touchPos;//ge the current touch position
				currentPosition.z = -5;
				previousPosition = currentPosition;//set the previous position

				if (tracingPoint.single_touch) {
					InstaitaeCirclePoint (currentPosition, currentLineRender.transform);//create circle point
				} else {
					InstaitaeCirclePoint (currentPosition, currentLineRender.transform);//create circle point

					//add the current point to the current line
					LineRenderer ln = currentLineRender.GetComponent<LineRenderer> ();
					LineRendererAttributes line_attributes = currentLineRender.GetComponent<LineRendererAttributes> ();
					int numberOfPoints = line_attributes.NumberOfPoints;
					numberOfPoints++;
					if (line_attributes.Points == null) {
						line_attributes.Points = new List<Vector3> ();
					}

					line_attributes.Points.Add (currentPosition);
					line_attributes.NumberOfPoints = numberOfPoints;
					ln.positionCount = (numberOfPoints);
					ln.SetPosition (numberOfPoints - 1, currentPosition);
				}
			}
		} else if (flag2 && !isTouchBegan) {//Touch Moved
			if (obTag == "TracingPoint") {
				TracingPoint tracingPoint = ob.GetComponent<TracingPoint> ();//get the current tracing point
				int currentindex = tracingPoint.index;//get the tracing point index
				if (tracingPoint.single_touch) {//skip if the touch is single
					return;
				}

				if (currentindex != previousTracingPointIndex) {
					currentTracingPoints.Add (currentindex);//add the current tracing point to the list
					previousTracingPointIndex = currentindex;//set the previous tracing point
				}
			} else if (obTag == "Background") {
				clickBeganOrMovedOutOfLetterArea = true;
				EndTouchLetterHandle ();
				clickStarted = false;
				return;
			}

			currentPosition = touchPos;
			currentPosition.z = -5.0f;
			float distance = Mathf.Abs (Vector3.Distance (currentPosition, new Vector3 (previousPosition.x, previousPosition.y, currentPosition.z)));//the distance between the current touch and the previous touch
			if (distance <= 0.1f) {//0.1 is distance offset
				return;	
			}

			previousPosition = currentPosition;//set the previous position

			//InstaitaeCirclePoint (currentPosition, currentLineRender.transform);//create circle point

			//add the current point to the current line
			LineRenderer ln = currentLineRender.GetComponent<LineRenderer> ();
			LineRendererAttributes line_attributes = currentLineRender.GetComponent<LineRendererAttributes> ();
			int numberOfPoints = line_attributes.NumberOfPoints;
			numberOfPoints++;
			line_attributes.Points.Add (currentPosition);
			line_attributes.NumberOfPoints = numberOfPoints;
			ln.positionCount = (numberOfPoints);
			ln.SetPosition (numberOfPoints - 1, currentPosition);
		}
	}

	//On tocuh released
	private void EndTouchLetterHandle ()
	{

		if (currentLineRender == null || currentTracingPoints.Count == 0) {
			return;//skip the next
		}

		TracingPart [] tracingParts = letters [currentLetterIndex].GetComponents<TracingPart> ();//get the tracing parts of the current letter
		bool equivfound = false;//whether a matching or equivalent tracing part found
		if (!clickBeganOrMovedOutOfLetterArea) {

			foreach (TracingPart part in tracingParts) {//check tracing parts
				if (currentTracingPoints.Count == part.order.Length && !part.succeded) {
					if (PreviousLettersPartsSucceeded (part, tracingParts)) {//check whether the previous tracing parts are succeeded
						equivfound = true;//assume true
						for (int i =0; i < currentTracingPoints.Count; i++) {
							int index = (int)currentTracingPoints [i];
							if (index != part.order [i]) {
								equivfound = false;
								break;
							}
						}
					}
				}
				if (equivfound) {//if equivalent found 
					part.succeded = true;//then the tracing part is succeed (written as wanted)
					break;
				}
			}
		}

		if (equivfound) {//if equivalent found 

			if (currentTracingPoints.Count != 1) {
				StartCoroutine ("SmoothCurrentLine");//make the current line smoother
			} else {
				currentLineRender = null;
			}
			PlayPositiveSound ();//play positive sound effect
		} else {
			trial += 1;
			PlayWrongSound ();//play negative or wrong answer sound effect
			Destroy (currentLineRender);//destroy the current line
			currentLineRender = null;//release the current line
		}

		previousPosition = Vector2.zero;//reset previous position
		currentTracingPoints.Clear ();//clear record of indexed
		previousTracingPointIndex = 0;//reset previous selected Index(index as point id)
		CheckLetterDone ();//check if the entier letter is written successfully or done
		if (letterDone) {//if the current letter done or wirrten successfully
			showStars();
			if (cheeringSound != null)
				AudioSource.PlayClipAtPoint (cheeringSound, Vector3.zero, 0.8f);//play the cheering sound effect
			hand.GetComponent<SpriteRenderer> ().enabled = false;//hide the hand
		}
	}

	private void showStars(){
		RefreshProcess ();
		trial += 1;
		float time = Time.time - timeStart;
		Debug.Log ("Trials : " + trial + " Time : " + time);
		int star = 0;
		if (trial <= 3 && time <= 10.0f) {
			star = 3;
		} else if (trial <= 5 && time <= 12.0f) {
			star = 2;
		} else {
			star = 1;
		}
		PanelStars.SetActive (true);

		for (int i = 0; i < 3; i++) {
			stars [i].SetActive (false);
		}
		for (int i = 0; i < star; i++) {
			stars [i].SetActive (true);
		}
	}

		//Check letter done or not
		private void CheckLetterDone ()
		{
				bool success = true;//letter success or done flag
				TracingPart [] tracingParts = letters [currentLetterIndex].GetComponents<TracingPart> ();//get the tracing parts of the current letter
				foreach (TracingPart part in tracingParts) {
						if (!part.succeded) {
								success = false;
								break;
						}
				}
		
				if (success) {
						letterDone = true;//letter done flag
						Debug.Log ("You done the " + letters [currentLetterIndex].name);
				}
		}

		//Back To Menu
		private void BackToMenu ()
		{
			SceneManager.LoadScene ("MenuHuruf");
		}

		//Refresh the lines and reset the tracing parts
		public void RefreshProcess ()
		{
				RefreshLines ();
				TracingPart [] tracingParts = letters [currentLetterIndex].GetComponents<TracingPart> ();
				foreach (TracingPart part in tracingParts) {
						part.succeded = false;
				}
				if (hand != null)
						hand.GetComponent<SpriteRenderer> ().enabled = true;
				letterDone = false;
		}

		//Refreesh the lines
		private void RefreshLines ()
		{
				StopCoroutine ("SmoothCurrentLine");
				GameObject [] gameobjs = HierrachyManager.FindActiveGameObjectsWithTag ("LineRenderer");
				if (gameobjs == null) {
						return;
				}
				foreach (GameObject gob in gameobjs) {
						Destroy (gob);	
				}
		}

		//Make the current lime more smoother
		private IEnumerator SmoothCurrentLine ()
		{
				LineRendererAttributes line_attributes = currentLineRender.GetComponent<LineRendererAttributes> ();
				LineRenderer ln = currentLineRender.GetComponent<LineRenderer> ();
				Vector3[] vectors = SmoothCurve.MakeSmoothCurve (line_attributes.Points.ToArray (), 10);

				int childscount = currentLineRender.transform.childCount;
				for (int i = 0; i < childscount; i++) {
						Destroy (currentLineRender.transform.GetChild (i).gameObject);
				}
		
				line_attributes.Points.Clear ();
				for (int i = 0; i <vectors.Length; i++) {
						if (i == 0 || i == vectors.Length - 1)
								InstaitaeCirclePoint (vectors [i], currentLineRender.transform);
						line_attributes.NumberOfPoints = i + 1;
						line_attributes.Points.Add (vectors [i]);
						ln.positionCount = (i + 1);
						ln.SetPosition (i, vectors [i]);
				}
				currentLineRender = null;
				yield return new WaitForSeconds (0);
		}

		//Check If User Passed The Previous Parts Before The Give Letter Part
		public static bool PreviousLettersPartsSucceeded (TracingPart currentpart, TracingPart[] lparts)
		{
				int p = currentpart.priority;

				if (p == 1) {
						return true;
				}

				bool prevsucceded = true;
				foreach (TracingPart part in lparts) {
						if (part.priority < p) {
							if (!part.succeded ) {//&& part.order.Length != 1 } //make single point TracingParts have no priority
										prevsucceded = false;
										break;
								}
						}
				}

				return prevsucceded;
		}

		//Play a positive or correct sound effect
		private void PlayPositiveSound ()
		{
				if (positiveSound != null)
						AudioSource.PlayClipAtPoint (positiveSound, Vector3.zero, 0.8f);//play the cheering sound effect
		}

		//Play wrong or opps sound effect
		private void PlayWrongSound ()
		{
				if (wrongSound != null)
						AudioSource.PlayClipAtPoint (wrongSound, Vector3.zero, 0.8f);//play the cheering sound effect
		}

		//Load the next letter
		public void LoadNextLetter ()
		{

				if (currentLetterIndex >= 0 && currentLetterIndex < letters.Length - 1) {
						currentLetterIndex++;
						LoadLetter ();
				}
		}

		//Load the previous letter
		public void LoadPreviousLetter ()
		{

				if (currentLetterIndex > 0 && currentLetterIndex < letters.Length) {
						currentLetterIndex--;
						LoadLetter ();
				}

		}

		//Load the current letter
		public void LoadLetter ()
		{
				if (letters == null) {
						return;
				}

				if (!(currentLetterIndex >= 0 && currentLetterIndex < letters.Length)) {
						return;
				}

				if (letters [currentLetterIndex] == null) {
						return;
				}

				letterDone = false;
				RefreshProcess ();
				HideLetters ();
				PanelStars.SetActive (false);
				hand.gameObject.SetActive (false);
				eraseButton.SetActive (false);
				readLetters [currentLetterIndex].SetActive (true);
				text.gameObject.SetActive (true);
				text.text = readLetters [currentLetterIndex].GetComponent<Letter>().letter;

				setRandomColor = true;
	}

	//Load the current letter
	public void WriteLetter ()
	{
		if (readLetters == null) {
			return;
		}

		if (!(currentLetterIndex >= 0 && currentLetterIndex < letters.Length)) {
			return;
		}

		if (letters [currentLetterIndex] == null) {
			return;
		}

		letterDone = false;
		trial = 0;
		timeStart = 0.0f;
		RefreshProcess ();
		HideLetters ();
		PanelStars.SetActive (false);
		hand.gameObject.SetActive (true);
		eraseButton.SetActive (true);
		letters [currentLetterIndex].SetActive (true);
		text.gameObject.SetActive (true);
		tmpMousePosition = Input.mousePosition;
		coroutine = StartTutor ();
		StartCoroutine (coroutine);
	}

	IEnumerator StartTutor(){
		while(true){
			yield return new WaitForSeconds(1f);
			List<TracingPart> tracingParts = new List<TracingPart> (letters [currentLetterIndex].GetComponents<TracingPart> ());
			tracingParts = tracingParts.OrderBy (x => x.priority).ToList();
			List<TracingPoint> tracingPoints = new List<TracingPoint> (letters [currentLetterIndex].GetComponentsInChildren<TracingPoint> ());
			foreach (TracingPart part in tracingParts) {
				if (part.order.Length == 1) {
					Vector3 pos = tracingPoints.Find (x => x.index == part.order[0]).transform.position;
					pos.z = -6;
					hand.position = pos;
					iTween.MoveTo (hand.gameObject, hand.position, 3f);
					yield return new WaitForSeconds (3f);
				} else {
					int elm = 0;
					foreach (int i in part.order) {
						if (elm == 0) {
							Vector3 pos = tracingPoints.Find (x => x.index == i).transform.position;
							pos.z = -6;
							hand.position = pos;
						} else {
							Vector3 nextPos = tracingPoints.Find (x => x.index == i).transform.position;
							nextPos.z = -6;
							iTween.MoveTo (hand.gameObject, nextPos, 3f);
							yield return new WaitForSeconds (3f);
						}
						elm++;
					}
				}
			}
		}
	}

	public void PlaySoundLetter()
	{
		if (readLetters == null) {
			return;
		}

		if (!(currentLetterIndex >= 0 && currentLetterIndex < letters.Length)) {
			return;
		}

		if (readLetters [currentLetterIndex] == null) {
			return;
		}

		AudioClip sound = readLetters [currentLetterIndex].GetComponent<Letter> ().sound;
		Vector3 pos = Vector3.zero;
		pos.z = Camera.main.transform.position.z;
		if (sound != null) {
			StartCoroutine (PlayOneTime(sound, pos));
		}
		Debug.Log ("Played " + readLetters [currentLetterIndex].GetComponent<Letter> ().letter);
		
	}

	IEnumerator PlayOneTime(AudioClip sound, Vector3 pos){
		gen.GetComponent<AudioSource> ().Pause ();
		AudioSource.PlayClipAtPoint (sound, pos);
		yield return new WaitForSeconds(sound.length);
		if (gen.GetComponent<General>().backSoundOn) {
			gen.GetComponent<AudioSource> ().Play ();
		}
	}

		//Hide the letters
		private void HideLetters ()
		{
				if (letters == null || readLetters == null) {
						return;
				}

				foreach (GameObject letter in letters) {
						if (letter != null)
								letter.SetActive (false);
				}

				foreach (GameObject readLetter in readLetters) {
						if (readLetter != null)
							readLetter.SetActive (false);
				}
		}
	
		//Create Cicle at given Point
		private void InstaitaeCirclePoint (Vector3 position, Transform parent)
		{
				GameObject currentcicrle = (GameObject)Instantiate (circlePointPrefab);//instaiate object
				currentcicrle.transform.parent = parent;
				currentcicrle.GetComponent<Renderer>().material = currentLineRender.GetComponent<LineRendererAttributes> ().material;
				currentcicrle.transform.position = position;
		}
}

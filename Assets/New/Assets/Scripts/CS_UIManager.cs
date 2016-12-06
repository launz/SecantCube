using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CS_UIManager : MonoBehaviour {
	[SerializeField] GameObject myMenu;
	[SerializeField] Transform myHighlight;
	[SerializeField] float myHighlightSpeed;
	[SerializeField] Transform[] myOptions;
	private int myOptionCurrent = 1;
	private Vector3 myOptionTargetPosition;

	private int myOptionSign = 1;
	private float myOptionTimer = 1;
	private float myOptionMaxTime = 1;
	// Use this for initialization
	void Start () {
		myMenu.SetActive (false);
		myOptionTargetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		//update Highlight position
		myHighlight.position = Vector3.Lerp (myHighlight.position, myOptions [myOptionCurrent].position, Time.fixedDeltaTime * myHighlightSpeed);


		if (myMenu.activeSelf == false) {
			//if menu is not open
			if (Input.GetButtonDown ("Pause")) {
				Pause ();
			}
		} else {
			//if menu is open
			if (Input.GetButtonDown ("Pause")) {
				Continue ();
			}


			if (Input.GetAxisRaw ("Horizontal") == 0)
				myOptionTimer = 0;
			
			if (myOptionTimer <= 0) {
				if (Input.GetAxisRaw ("Horizontal") > 0) {
					myOptionCurrent++;
					if (myOptionCurrent >= myOptions.Length)
						myOptionCurrent -= myOptions.Length;
					myOptionTimer = myOptionMaxTime;
				} else if (Input.GetAxisRaw ("Horizontal") < 0) {
					myOptionCurrent--;
					if (myOptionCurrent < 0)
						myOptionCurrent += myOptions.Length;
					myOptionTimer = myOptionMaxTime;
				}
				//Debug.Log (myOptionCurrent);
			} else {
				myOptionTimer -= Time.deltaTime;
			}


			if (Input.GetButtonDown ("Jump")) {
				switch (myOptionCurrent) {
				case 0: 
					Restart ();
					break;
				case 1:
					Continue ();
					break;
				case 2:
					Quit ();
					break;
				default:
					break;
				}
			}

		}
	}

	private void Pause () {
		Time.timeScale = 0;
		myMenu.SetActive (true);
	}

	private void Restart () {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
		CS_AudioManager.Instance.ResetSnapshot ();
	}

	private void Continue () {
		Time.timeScale = 1;
		myMenu.SetActive (false);
	}

	private void Quit () {
		Application.Quit ();
	}
}

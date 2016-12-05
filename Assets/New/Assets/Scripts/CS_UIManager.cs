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
				
			if (Input.GetButtonDown ("Horizontal")) {
				//Debug.Log (t_Horizontal);
				//float t_Horizontal = Input.GetAxisRaw ("Horizontal");
				if (Input.GetAxisRaw ("Horizontal") > 0) {
					myOptionCurrent++;
					if (myOptionCurrent >= myOptions.Length)
						myOptionCurrent -= myOptions.Length;
				} else {
					myOptionCurrent--;
					if (myOptionCurrent < 0)
						myOptionCurrent += myOptions.Length;
				}
					
				Debug.Log (myOptionCurrent);

				//myHighlight.position = Vector3.Lerp (myHighlight.position, myOptions [myOptionCurrent].position, 0.1f);
				//myOptions [myOptionCurrent].position;
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
	}

	private void Continue () {
		Time.timeScale = 1;
		myMenu.SetActive (false);
	}

	private void Quit () {
		Application.Quit ();
	}
}

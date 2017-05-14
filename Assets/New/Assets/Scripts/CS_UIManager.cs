using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using JellyJoystick;

public class CS_UIManager : MonoBehaviour {
	[SerializeField] GameObject myMenu;
	[SerializeField] Transform myHighlight;
	[SerializeField] float myHighlightSpeed;
	[SerializeField] Transform[] myOptions;
	private int myOptionCurrent = 1;
	private Vector3 myOptionTargetPosition;

	private int myOptionSign = 1;
	private float myOptionTimer = 1;
	[SerializeField] float myOptionMaxTime = 1;
	// Use this for initialization
	void Start () {
		myMenu.SetActive (false);
		myOptionTargetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		//update Highlight position
		myHighlight.position = Vector3.Lerp (myHighlight.position, myOptions [myOptionCurrent].position, Time.unscaledDeltaTime * myHighlightSpeed);


		if (myMenu.activeSelf == false) {
			//if menu is not open
			if (JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.START)) {
				Pause ();
			}
		} else {
			//if menu is open
			if (JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.START)) {
				Continue ();
			}


			if (JellyJoystickManager.Instance.GetAxis (AxisMethodName.Raw, 1, JoystickAxis.LS_X) == 0)
				myOptionTimer = 0;
			
			if (myOptionTimer <= 0) {
				if (JellyJoystickManager.Instance.GetAxis (AxisMethodName.Raw, 1, JoystickAxis.LS_X) > 0) {
					myOptionCurrent++;
					if (myOptionCurrent >= myOptions.Length)
						myOptionCurrent -= myOptions.Length;
					myOptionTimer = myOptionMaxTime;
				} else if (JellyJoystickManager.Instance.GetAxis (AxisMethodName.Raw, 1, JoystickAxis.LS_X) < 0) {
					myOptionCurrent--;
					if (myOptionCurrent < 0)
						myOptionCurrent += myOptions.Length;
					myOptionTimer = myOptionMaxTime;
				}
				//Debug.Log (myOptionCurrent);
			} else {
				myOptionTimer -= Time.unscaledDeltaTime;
			}


			if (JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.A) ||
			    JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.B) ||
			    JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.X) ||
			    JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 1, JoystickButton.Y)) {
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

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager.
/// Script for main Game States
/// </summary>

public class GameManager : MonoBehaviour {

	public GameObject playerObject;
	public GameObject drumObject;
	public GameObject synthObject;

	public int pickupScore;

	public int maxScore;

	public AudioMixer audioMixer;

	public AudioMixerSnapshot[] mixerSnapshots;





	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

		//USE FOR DEBUGGING ONLY!!!
//		if (Input.GetKeyDown(KeyCode.T)) {
//			AddPickup();
//		}
	
	}

	public void AddPickup() {

		pickupScore++;
		//increase Camera Shake, increase motion blur
		//change music track
		//if numPickups >= 10, unparent spindles from cube

		playerObject.SendMessage("IncreaseScore", 1f/(float)maxScore);

		Camera.main.GetComponent<MotionBlur> ().blurAmount = (float)pickupScore * (1f / (float)maxScore);

		//Music cue conditionals
		if (pickupScore == 1) {
			//bring in synth
			mixerSnapshots [1].TransitionTo (3f);

		} else if (pickupScore == 2) {
			mixerSnapshots [2].TransitionTo (3f);
			//switch to other drums
			drumObject.SendMessage ("NextClip");

		} else if (pickupScore == 3) {
			//bring in synth bass
			mixerSnapshots [3].TransitionTo (3f);
		} else if (pickupScore == 4) {
			//switch to other synth
			//bring in synth 2
			synthObject.SendMessage ("NextClip");
			mixerSnapshots [4].TransitionTo (3f);
		} else if (pickupScore == 5) {
			
			mixerSnapshots [5].TransitionTo (3f);
		} else if (pickupScore == 6) {
			
		} else if (pickupScore == 7) {
			//final snapshot
			mixerSnapshots [6].TransitionTo (3f);
		}


		if (pickupScore >= maxScore) {
			playerObject.SendMessage ("Explode");
		}

		//Mixing audio between the beginning state (blend between positional audio,
		//SFX, and 2D audio) and ending state (distorted, no sfx, all 2D audio)
	


	}
}

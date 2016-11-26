using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {


	public float magnitude;

	public float prevMagnitude;

	Vector3 moveForce;

	float horizontalMag;
	float verticalMag;
	public float jumpMag;
	public float dragAmt;

	public float shakeIntensity;

	bool _startedAccel;

	public float wobbleThreshold;

	//Reference to child noise object
	public GameObject playerNoise;

	//Reference to directional light (used for explosion)
	public GameObject dirLight;



	//reference to GO's rigidBody
	Rigidbody rigid;

	//Reference to the camera dummy
	public GameObject cameraDummy;

	//specific reference to the camera dummy's y Euler rotation
	public float cameraDummyRot;



	// Use this for initialization
	void Start () {

		rigid = gameObject.GetComponent<Rigidbody>();
		moveForce = new Vector3 (0f, 0f, 0f);
		_startedAccel = false;
		shakeIntensity = 0.05f;
	
	}
	
	// Update is called once per frame
	void Update() {
		if (moveForce.magnitude > prevMagnitude && !_startedAccel) {
			StartCoroutine(CameraShake.Shake(Camera.main.transform, shakeIntensity));
			//_startedAccel = true;
		}

		if (rigid.velocity.magnitude > wobbleThreshold) {
			StartCoroutine(CameraShake.Shake(Camera.main.transform, 0.02f));
		}


	}


	void FixedUpdate () {

		//

		horizontalMag = Input.GetAxis ("Horizontal");
		verticalMag = Input.GetAxis ("Vertical");

	
		if (Input.GetButton("Jump")) {
			rigid.AddForce(new Vector3(0,jumpMag,0), ForceMode.Impulse);
		}

		//cameraDummyRot is returning the proper angle - the camera dummy object is placed at the same location as our ball,
		//but rotates relative to the camera (controlled by the mouse/right joystick)
		cameraDummyRot = cameraDummy.transform.eulerAngles.y;


		prevMagnitude = moveForce.magnitude;
		moveForce = new Vector3 (horizontalMag, 0f, verticalMag);

		//create a rotation quaternion which, when multiplied by a Vector3, rotates that vector "cameraDummyRot" degrees
		// about the "Vector3.up" axis
		Quaternion forceRotation = Quaternion.Euler(0,cameraDummyRot,0);
		//Debug.Log (forceRotation);

		Vector3 rotatedForce = forceRotation * moveForce;

		rigid.AddForce (rotatedForce * magnitude);


		//applying drag to body (x-z plane only)

		//normalized opposite of velocity
		Vector3 dragDirection = -rigid.velocity.normalized;
		if (Mathf.Abs (horizontalMag) < 0.1f && Mathf.Abs (verticalMag) < 0.1f) {

			_startedAccel = false;

			//rigid.AddForce (new Vector3(dragDirection.x * dragAmt, 0, dragDirection.z * dragAmt));

		}

	}

	void Explode() {
		foreach (Transform child in transform) {
			GameObject spindle = child.gameObject;
			spindle.AddComponent<Rigidbody> ();

			//spindle.transform.parent = null;


		}
		StartCoroutine(CameraShake.Shake(Camera.main.transform, 1.0f));
		dirLight.GetComponent<Light> ().intensity = 5.0f;
		dirLight.GetComponent<Light> ().color = Color.magenta;
		gameObject.GetComponent<AudioSource>().Play();


		//gameObject.GetComponent<AudioSource> ().Play();
	}

	void IncreaseScore(float volAmount) {

		playerNoise.GetComponent<AudioSource> ().volume = volAmount;
		shakeIntensity += 0.01f;

	}

	

	
		
}

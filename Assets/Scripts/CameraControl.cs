using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject target;
	public GameObject player;
	public Vector3 offset;
	public float rotateSpeed = 5;

	public float FOVmin = 50f;
	public float FOVscaling = 100f;

	bool _controllerConnected = false;


	// Use this for initialization
	void Start () {
		//offset = target.transform.position - transform.position;
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetAxis("RightJoyY") != 0f) {
			_controllerConnected = true;
		}
			
		LookInput ();
		AdjustFOV ();
		
	}

	void LateUpdate() {


	}



	void LookInput() {

		float horizontal;
		float vertical;
		
		if (_controllerConnected) {
			//eventually change this to joystick input
			horizontal = Input.GetAxis ("RightJoyX") * rotateSpeed;

			Debug.Log (horizontal);
			vertical = -Input.GetAxis ("RightJoyY") * rotateSpeed;
		} else {

			horizontal = Input.GetAxis ("Mouse X") * rotateSpeed;

			vertical = -Input.GetAxis ("Mouse Y") * rotateSpeed;
		
		}

		//vertical = map(vertical, -1.0f, 1.0f, -0.5f, 1.5f);
		target.transform.Rotate (vertical, horizontal, 0);
		//transform.Rotate (vertical, horizontal, 0);


		float desiredAngleY = target.transform.eulerAngles.y ;
		float desiredAngleX = target.transform.eulerAngles.x ;
		Quaternion rotation = Quaternion.Euler (desiredAngleX, desiredAngleY, 0);
		transform.position = target.transform.position - (rotation * offset); 
		transform.LookAt (target.transform);

	}

	void AdjustFOV(){

		Camera.main.fieldOfView = Mathf.Clamp(FOVmin + player.GetComponent<Rigidbody>().velocity.magnitude/FOVscaling, 
			FOVmin, 150f); 

	}

	//helper for mapping value 's' from a1-a2 to b1-b2
	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}

using UnityEngine;
using System.Collections;

public class CS_Portal : MonoBehaviour {
	[SerializeField] Color myStartColor;
	[SerializeField] Color myMidColor;
	[SerializeField] Color myEndColor;
	[SerializeField] GameObject myExit;
	[SerializeField] int myExitLevelNumber;
	private bool isOn = true;

	[SerializeField] GameObject myShowLevel;
	[SerializeField] GameObject myHideLevel;

	private Quaternion myRotation;

//	private GameObject myCopy;

	void Start () {
		//SetIsOn (true);
//		myCopy = new GameObject ();
//		myCopy.transform.position = this.transform.position;
//		myCopy.transform.rotation = this.transform.rotation;

		myRotation = this.transform.rotation;
	}

	void Update () {
		//SetIsOn (true);
		//		myCopy = new GameObject ();
		//		myCopy.transform.position = this.transform.position;
		//		myCopy.transform.rotation = this.transform.rotation;

		this.transform.rotation = myRotation;
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && isOn == true) {

			myExit.GetComponent<CS_Portal> ().SetIsOn (false);

			if (myShowLevel != null) {
				if (myShowLevel.activeSelf == false) {
					myShowLevel.SetActive (true);
					Debug.Log ("Show:" + myShowLevel.name);
				}
			}

			if (myHideLevel != null) {
				if (myHideLevel.activeSelf == true) {
					myHideLevel.SetActive (false);
					Debug.Log ("Hide:" + myHideLevel.name);
				}
			}
//			Debug.LogError ("!!!!");
			GameObject t_camera = other.GetComponent<CS_PlayerControl> ().GetMyCamera ();

			Vector3 t_deltaPosition = other.transform.position - this.transform.position;
			t_deltaPosition = RotateVecter (t_deltaPosition, this.transform, myExit.transform);
			other.transform.position = myExit.transform.position + t_deltaPosition;

			t_deltaPosition = t_camera.transform.position - this.transform.position;
			t_deltaPosition = RotateVecter (t_deltaPosition, this.transform, myExit.transform);
			t_camera.transform.position = myExit.transform.position + t_deltaPosition;

			Vector3 t_deltaRotation = other.transform.rotation.eulerAngles - this.transform.rotation.eulerAngles;
			t_deltaRotation = RotateVecter (t_deltaRotation, this.transform, myExit.transform);
			other.transform.rotation = Quaternion.Euler (myExit.transform.rotation.eulerAngles + t_deltaRotation);

			t_deltaRotation = t_camera.transform.rotation.eulerAngles - this.transform.rotation.eulerAngles;
			t_deltaRotation = RotateVecter (t_deltaRotation, this.transform, myExit.transform);
			t_camera.transform.rotation = Quaternion.Euler (myExit.transform.rotation.eulerAngles + t_deltaRotation);


			other.GetComponent<Rigidbody> ().velocity = 
				RotateVecter (other.GetComponent<Rigidbody> ().velocity, this.transform, myExit.transform);
			
			other.GetComponent<CS_PlayerControl> ().SetMyCameraCenterDelta (
				RotateVecter (other.GetComponent<CS_PlayerControl> ().GetMyCameraCenterDelta (), this.transform, myExit.transform)
			);

			other.GetComponent<CS_PlayerControl> ().SetLaunchpad (
				RotateVecter (other.GetComponent<CS_PlayerControl> ().GetLaunchpad (), this.transform, myExit.transform)
			);


			t_camera.GetComponent<ReplacementShaderEffect> ().ChangeColors (myStartColor, myMidColor, myEndColor);

			CS_AudioManager.Instance.PlaySnapshotAdd (myExitLevelNumber);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			SetIsOn (true);
		}
	}

	private Vector3 RotateVecter (Vector3 g_original,Transform g_from, Transform g_to) {
		int t_sign = 1;

		if (Vector3.Angle (g_original, g_from.up) > 90)
			t_sign = -1;
		else
			t_sign = 1;
		float t_up = Vector3.Project (g_original, g_from.up).magnitude * t_sign;

		if (Vector3.Angle (g_original, g_from.right) > 90)
			t_sign = -1;
		else
			t_sign = 1;
		float t_right = Vector3.Project (g_original, g_from.right).magnitude * t_sign;

		if (Vector3.Angle (g_original, g_from.forward) > 90)
			t_sign = -1;
		else
			t_sign = 1;
		float t_forward = Vector3.Project (g_original, g_from.forward).magnitude * t_sign;

		Vector3 t_final = t_up * g_to.up + t_right * g_to.right + t_forward * g_to.forward;
		return t_final;
	}

	public void SetIsOn (bool t_isOn) {
		isOn = t_isOn;
	}

}

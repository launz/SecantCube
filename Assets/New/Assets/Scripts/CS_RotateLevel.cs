using UnityEngine;
using System.Collections;

public class CS_RotateLevel : MonoBehaviour {

	[SerializeField] float myRotationSpeed = 1;
	[SerializeField] float myRotationStopAngle = 5;

	private Quaternion myTargetRotation;
	private Transform myRotationCenter;
	private Vector3 myRotationCenterPosition;

	private Camera myCamera;

	[Header("Shake")]
	[SerializeField] float myShake_Max = 1;
	[SerializeField] float myShake_Min = 0;
	[SerializeField] float myShake_Ratio = 120;
	[SerializeField] float myShake_Speed = 10;
	private float myShake_Intensity = 0;
	// Use this for initialization
	void Start () {
		myTargetRotation = this.transform.rotation;
		myCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (myRotationCenter == null)
			return;

		if (this.transform.rotation == myTargetRotation)
			return;

		if (Quaternion.Angle(this.transform.rotation, myTargetRotation) < myRotationStopAngle) {
			Debug.Log (Quaternion.Angle(this.transform.rotation, myTargetRotation));
			this.transform.rotation = myTargetRotation;
			this.transform.position = myRotationCenterPosition - myRotationCenter.position + this.transform.position;
			return;
		}

		UpdateCameraShake ();
		Debug.Log("rotate!!!!!!" + myTargetRotation.eulerAngles);
//		Vector3.Lerp (this.transform.rotation.eulerAngles, myTargetRotation.eulerAngles, Time.deltaTime * myRotationSpeed);
//		this.transform.rotation = myTargetRotation;
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, myTargetRotation, Time.deltaTime * myRotationSpeed);
		this.transform.position = myRotationCenterPosition - myRotationCenter.position + this.transform.position;

	}

	public void SetRotation (Quaternion g_rotation, Transform g_center) {
		myTargetRotation = g_rotation;
		myRotationCenter = g_center;
		myRotationCenterPosition = g_center.position;
	}

	private void UpdateCameraShake () {
		float t_shakeIntensity;

		t_shakeIntensity = myShake_Max -
			myShake_Ratio / (
				Quaternion.Angle (this.transform.rotation, myTargetRotation) + (
					myShake_Ratio / (
						myShake_Max - myShake_Min
					)));

		myShake_Intensity = Mathf.Lerp (
			myShake_Intensity, 
			t_shakeIntensity, 
			Time.deltaTime * myShake_Speed
		);

		myCamera.transform.position = myCamera.transform.position + Random.insideUnitSphere * myShake_Intensity;
		myCamera.transform.Rotate (
			Random.Range (-myShake_Intensity, myShake_Intensity), 
			Random.Range (-myShake_Intensity, myShake_Intensity), 
			Random.Range (-myShake_Intensity, myShake_Intensity)
		);

		//		Debug.Log (myShake_Intensity + "myShake_Intensity");
	}
}

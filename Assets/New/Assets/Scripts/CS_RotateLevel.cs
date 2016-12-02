using UnityEngine;
using System.Collections;

public class CS_RotateLevel : MonoBehaviour {

	[SerializeField] float myRotationSpeed = 1;

	private Quaternion myTargetRotation;
	private Transform myRotationCenter;
	private Vector3 myRotationCenterPosition;
	// Use this for initialization
	void Start () {
		myTargetRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (myRotationCenter == null)
			return;

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


}

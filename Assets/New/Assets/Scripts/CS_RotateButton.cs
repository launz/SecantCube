using UnityEngine;
using System.Collections;

public class CS_RotateButton : MonoBehaviour {

	[SerializeField] GameObject myRotateCenter;
	[SerializeField] GameObject myRotateTarget;

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Player") {
			myRotateTarget.GetComponent<CS_RotateLevel> ().SetRotation (Quaternion.Euler(this.transform.localRotation.eulerAngles * -1), myRotateCenter.transform);
		}
	}
}

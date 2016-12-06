using UnityEngine;
using System.Collections;

public class CS_PlayerCamera : MonoBehaviour {
	[SerializeField] GameObject myWater;
	// Use this for initialization
//	void Start () {
//		myWater.SetActive (false);
//	}
//	
//	// Update is called once per frame
	void Update () {
		if (this.transform.position.y > 0) {
			HideWater ();
		} else {
			ShowWater ();
		}
	}

	public void ShowWater () {
		if (myWater.activeSelf == false)
			myWater.SetActive (true);
	}

	public void HideWater () {
		if (myWater.activeSelf == true)
			myWater.SetActive (false);
	}
}

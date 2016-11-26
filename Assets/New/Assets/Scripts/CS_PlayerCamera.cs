using UnityEngine;
using System.Collections;

public class CS_PlayerCamera : MonoBehaviour {
	private GameObject myPlayer;
	[SerializeField] float myDistance;
	[SerializeField] float mySpeed;
	// Use this for initialization
	void Start () {
		myPlayer = GameObject.Find (CS_Global.NAME_PLAYER) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 t_targetPosition = myPlayer.transform.position - this.transform.position;
		t_targetPosition.Normalize ();
		//Debug.Log (t_targetPosition);
		t_targetPosition = myPlayer.transform.position - t_targetPosition * myDistance;
		//Debug.Log (t_targetPosition);
		//Debug.Log (Vector3.Lerp (this.transform.position, t_targetPosition, Time.deltaTime * mySpeed));
		this.transform.position = Vector3.Lerp (this.transform.position, t_targetPosition, Time.deltaTime * mySpeed);

		this.transform.LookAt (myPlayer.transform);

	}

}

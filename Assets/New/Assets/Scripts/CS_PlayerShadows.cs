using UnityEngine;
using System.Collections;

public class CS_PlayerShadows : MonoBehaviour {

	Ray forwardRay;
	Ray backRay;
	Ray leftRay;
	Ray rightRay;
	Ray upRay;
	Ray downRay;

	[SerializeField] GameObject shadow;
	GameObject forwardShadow;
	GameObject backShadow;
	GameObject leftShadow;
	GameObject rightShadow;
	GameObject upShadow;
	GameObject downShadow;

	private int layerMask = 768;




	void Start () {
		forwardShadow = Instantiate (shadow) as GameObject;
		backShadow = Instantiate (shadow) as GameObject;
		leftShadow = Instantiate (shadow) as GameObject;
		rightShadow = Instantiate (shadow) as GameObject;
		upShadow = Instantiate (shadow) as GameObject;
		downShadow = Instantiate (shadow) as GameObject;
	}
	

	void Update () {
	
		MoveShadow (forwardShadow, Vector3.forward);
		MoveShadow (backShadow, Vector3.back);
		MoveShadow (upShadow, Vector3.up);
		MoveShadow (downShadow, Vector3.down);
		MoveShadow (leftShadow, Vector3.left);
		MoveShadow (rightShadow, Vector3.right);


//		Vector3 forward = transform.TransformDirection(Vector3.forward);
//		rayForward = Physics.Raycast (transform.position, Vector3.forward, 10000);
//		RaycastHit hitForward;
//		forwardRay = new Ray (transform.position, Vector3.forward);


//
//		Debug.DrawRay(transform.position, Vector3.forward * 1000, Color.red);
//		Debug.DrawRay(transform.position, Vector3.back * 1000, Color.red);
//		Debug.DrawRay(transform.position, Vector3.left * 1000, Color.red);
//		Debug.DrawRay(transform.position, Vector3.right * 1000, Color.red);
//		Debug.DrawRay(transform.position, Vector3.up * 1000, Color.red);
//		Debug.DrawRay(transform.position, Vector3.down * 1000, Color.red);

	}

	void MoveShadow (GameObject g_shadow, Vector3 g_direction) {

		RaycastHit t_hit;
		Ray t_ray = new Ray (transform.position, g_direction);



		if (Physics.Raycast (t_ray, out t_hit,500f,1)) {

			g_shadow.transform.position = t_hit.point;

//			Debug.Log (t_hit.transform.name + " " +t_hit.transform.position);

		}

	}
}



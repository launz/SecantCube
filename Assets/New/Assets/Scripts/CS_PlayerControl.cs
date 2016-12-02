using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CS_PlayerControl : MonoBehaviour {
	[Header("Movement")]
	private Rigidbody myRigidbody;
	[SerializeField] float myNormalVelocity = 5;
	[SerializeField] float myNormalForce = 1;
	[SerializeField] float myVerticalForce = 10;
	[SerializeField] float myHorizontalForce = 50;
	[SerializeField] float myVelocityRatio = 10;
	[SerializeField] float myAirResistance = 0.1f;

	[Header("Camera")]
	[SerializeField] GameObject myCamera;
	[SerializeField] Transform myCameraCollide;

	[SerializeField] float myCameraDistance = 10;
	[SerializeField] float myCameraSpeed = 4;
	[SerializeField] float myVerticalLimitRatio = 0.8f;
	private Vector3 myCameraCenterDelta;
	[SerializeField] float myCameraCenterDistanceRatio = 0.5f;
	[SerializeField] float myCameraCenterSpeed = 4;

	[Header("Shake")]
	[SerializeField] float myShake_Max = 1;
	[SerializeField] float myShake_Min = 0;
	[SerializeField] float myShake_Ratio = 120;
	[SerializeField] float myShake_Speed = 10;
	private float myShake_Intensity = 0;

	[Header("Blur")]
	[SerializeField] float myBlur_Max = 0.5f;
	[SerializeField] float myBlur_Min = 0.01f;
	[SerializeField] float myBlur_Ratio = 120;
	[SerializeField] float myBlur_Speed = 1;

	[Header("FieldOfView")]
	[SerializeField] float myFieldOfView_Max = 100;
	[SerializeField] float myFieldOfView_Min = 60;
	[SerializeField] float myFieldOfView_Ratio = 120;
	[SerializeField] float myFieldOfView_Speed = 1;

	[Header("Boost")]
	[SerializeField] float myBoost_Force = 2000;
	[SerializeField] float myBoost_EnergyMax = 100;
	private float myBoost_EnergyCurrent;
	private float myBoost_IsOnBoostSurface;
	[SerializeField] bool myBoost_IsRechargedByPercentage = true;
	[SerializeField] float myBoost_RechargePerSecond = 0.2f;
	[SerializeField] bool myBoost_IsUsedByPercentage = true;
	[SerializeField] float myBoost_UsePerSecond = 0.1f;

	[Header("BoostDisplay")]
	[SerializeField] GameObject myBoostDisplay;
	[SerializeField] Color myBoostDisplay_ColorMax;
	[SerializeField] Color myBoostDisplay_ColorMin;
	[SerializeField] float myBoostDisplay_SizeMax;
	[SerializeField] float myBoostDisplay_SizeMin;
	// Use this for initialization
	void Start () {
		if (myCamera == null) {
			myCamera = Camera.main.gameObject;
		}

		myCameraCenterDelta = Vector3.up;
		myRigidbody = this.GetComponent<Rigidbody> ();

		myBoost_EnergyCurrent = myBoost_EnergyMax;

	}
	
	// Update is called once per frame
	void Update () {

		float translationVertical = Input.GetAxis("Vertical") * myVerticalForce * Time.deltaTime;
		float translationHorizontal = Input.GetAxis("Horizontal") * myHorizontalForce * Time.deltaTime;

		float t_nextAngle = Vector3.Angle (
			(myCamera.transform.position + myCamera.transform.up * translationVertical - this.transform.position), 
			Vector3.up);
//		Debug.Log (t_nextAngle);

		//camera limit
//		if (t_nextAngle < 180 - myVerticalLimit && t_nextAngle > myVerticalLimit)
//			myCamera.transform.position += myCamera.transform.up * translationVertical;
//		myCamera.transform.position -= myCamera.transform.right * translationHorizontal;

		if ((Mathf.Abs (myCamera.transform.position.y - (this.transform.position + myCameraCenterDelta).y) > Vector3.Distance (myCamera.transform.position, (this.transform.position + myCameraCenterDelta)) * myVerticalLimitRatio) &&
		    ((myCamera.transform.position.y > (this.transform.position + myCameraCenterDelta).y && translationVertical > 0) ||
		    (myCamera.transform.position.y < (this.transform.position + myCameraCenterDelta).y && translationVertical < 0))) {
			//camera movement
			myCamera.transform.position = myCamera.transform.position - 
				myCamera.transform.right * translationHorizontal;
			Debug.Log ("Limit");
//			myCamera.GetComponent<Rigidbody> ().MovePosition (
//				myCamera.transform.position -
//				myCamera.transform.right * translationHorizontal
//			);

			//player movement
			myRigidbody.velocity = (
			    myRigidbody.velocity.normalized + (
			        myCamera.transform.right * translationHorizontal
			    ) * Time.deltaTime * myVelocityRatio
			).normalized * myRigidbody.velocity.magnitude;
		} else {
			//camera movement
			myCamera.transform.position = myCamera.transform.position + 
				myCamera.transform.up * translationVertical - 
				myCamera.transform.right * translationHorizontal;
			
//			myCamera.GetComponent<Rigidbody> ().MovePosition (
//				myCamera.transform.position +
//				myCamera.transform.up * translationVertical -
//				myCamera.transform.right * translationHorizontal
//			);

			//player movement
			myRigidbody.velocity = (
				myRigidbody.velocity.normalized + (
					myCamera.transform.right * translationHorizontal - myCamera.transform.up * translationVertical
				) * Time.deltaTime * myVelocityRatio
			).normalized * myRigidbody.velocity.magnitude;
		}



		//airResistance
		myRigidbody.AddForce(-myRigidbody.velocity.normalized * myRigidbody.velocity.magnitude * myRigidbody.velocity.magnitude * myAirResistance);

		UpdateBoost ();
		UpdateCameraBlur ();
		UpdateCameraFieldOfView ();
		UpdateCamera ();
		UpdateCameraShake ();
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		float translationHorizontal = Input.GetAxis("Horizontal") * myHorizontalForce * Time.deltaTime;

		Gizmos.DrawRay (transform.position, myCamera.transform.right * translationHorizontal);
		Gizmos.DrawSphere (transform.position + myCamera.transform.up * 2f,0.3f);
	}

	private void UpdateCamera () {
		Vector3 t_targetPosition = this.transform.position - myCamera.transform.position;

		t_targetPosition.Normalize ();

		t_targetPosition = this.transform.position - t_targetPosition * myCameraDistance;

		//Move Camera
		myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, t_targetPosition, Time.deltaTime * myCameraSpeed);
//		Vector3 newPosition = Vector3.Lerp(myCamera.transform.position, t_targetPosition, Time.deltaTime * myCameraSpeed);
//		myCamera.GetComponent<Rigidbody>().MovePosition( newPosition );


		Vector3 t_lookAt = - Input.GetAxis ("Vertical") * myCamera.transform.up + Input.GetAxis ("Horizontal") * myCamera.transform.right;
//		Vector3 t_lookAt = Input.GetAxis ("Horizontal") * myCamera.transform.right;
//		Vector3 t_lookAt = new Vector3();
		t_lookAt = t_lookAt - Vector3.Project (t_lookAt, myCamera.transform.position - this.transform.position);
		t_lookAt.Normalize ();
		if (t_lookAt.magnitude != 0) {
			t_lookAt = t_lookAt * myCameraCenterDistanceRatio * (myCamera.transform.position - this.transform.position).magnitude;
			//ease
			myCameraCenterDelta = Vector3.Lerp (myCameraCenterDelta, t_lookAt, Time.deltaTime * myCameraCenterSpeed);
		}
//		Debug.Log (this.transform.position + " " + myCameraCenterDelta);

		//Update Camera collide
		myCameraCollide.position = myCameraCenterDelta + this.transform.position;
		myCameraCollide.GetComponent<Rigidbody> ().MovePosition (myCameraCenterDelta + this.transform.position);

		myCamera.transform.LookAt (this.transform.position + myCameraCenterDelta);

		myCamera.GetComponent<Rigidbody> ().velocity = Vector3.zero;

	}

	private void UpdateBoost () {
		if (Input.GetButton ("Jump")) {
			//Debug.Log ("myBoost_EnergyCurrent: " + myBoost_EnergyCurrent);		
			//if has energy
			if (myBoost_EnergyCurrent > 0) {

				//use boost energy
				if (myBoost_IsUsedByPercentage)
					myBoost_EnergyCurrent -= myBoost_UsePerSecond * myBoost_EnergyMax * Time.deltaTime;
				else
					myBoost_EnergyCurrent -= myBoost_UsePerSecond * Time.deltaTime;

				//check if the boost energy < 0
				if (myBoost_EnergyCurrent < 0)
					myBoost_EnergyCurrent = 0;
				
				//boost
				Vector3 t_forward = this.transform.position - myCamera.transform.position;
				t_forward.Normalize ();
				myRigidbody.AddForce (t_forward * myBoost_Force);
			} else if (myRigidbody.velocity.magnitude >= myNormalVelocity) {
//				Debug.Log (myRigidbody.velocity.magnitude);
				return;
			} else {
				Vector3 t_forward = this.transform.position - myCamera.transform.position;
				t_forward.Normalize ();
				myRigidbody.AddForce (t_forward * myNormalForce);
				if (myRigidbody.velocity.magnitude >= myNormalVelocity) {
					myRigidbody.velocity = myRigidbody.velocity.normalized * myNormalVelocity;
				}
			}


		}

		//show boost amount
		myBoostDisplay.GetComponent<Renderer> ().material.color = 
			myBoost_EnergyCurrent / myBoost_EnergyMax * (myBoostDisplay_ColorMax - myBoostDisplay_ColorMin) + myBoostDisplay_ColorMin;
		myBoostDisplay.transform.localScale = Vector3.one *
			(myBoost_EnergyCurrent / myBoost_EnergyMax * (myBoostDisplay_SizeMax - myBoostDisplay_SizeMin) +
				myBoostDisplay_SizeMin);
	}

	private void UpdateCameraBlur () {
		float t_blur = myBlur_Max -
			myBlur_Ratio / (
				myRigidbody.velocity.magnitude + (
					myBlur_Ratio / (
						myBlur_Max - myBlur_Min
					)));

		myCamera.GetComponent<MotionBlur> ().blurAmount = Mathf.Lerp (
			myCamera.GetComponent<MotionBlur> ().blurAmount, 
			t_blur, 
			Time.deltaTime * myBlur_Speed
		);
	}

	private void UpdateCameraShake () {
		float t_shakeIntensity;
		if (Input.GetButton ("Jump"))
			t_shakeIntensity = myShake_Max -
				myShake_Ratio / (
					myRigidbody.velocity.magnitude + (
						myShake_Ratio / (
							myShake_Max - myShake_Min
						)));
		else
			t_shakeIntensity = 0;

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

	private void UpdateCameraFieldOfView () {
		float t_fieldOfView = myFieldOfView_Max -
			myFieldOfView_Ratio / (
				myRigidbody.velocity.magnitude + (
					myFieldOfView_Ratio / (
						myFieldOfView_Max - myFieldOfView_Min
					)));

		myCamera.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (
			myCamera.GetComponent<Camera> ().fieldOfView, 
			t_fieldOfView, 
			Time.deltaTime * myFieldOfView_Speed
		);
	}

	void OnCollisionStay (Collision g_collision) {
		if (g_collision.transform.tag == CS_Global.TAG_BOOST) {
			//recharge boost energy
			if(myBoost_IsRechargedByPercentage)
				myBoost_EnergyCurrent += myBoost_EnergyMax * myBoost_RechargePerSecond * Time.deltaTime;
			else
				myBoost_EnergyCurrent +=  myBoost_RechargePerSecond * Time.deltaTime;
			//check if the boost energy > max
			if (myBoost_EnergyCurrent > myBoost_EnergyMax)
				myBoost_EnergyCurrent = myBoost_EnergyMax;
		}
	}

	public GameObject GetMyCamera () {
		return myCamera;
	}

	public Vector3 GetMyCameraCenterDelta () {
		return myCameraCenterDelta;
	}

	public void SetMyCameraCenterDelta (Vector3 g_myCameraCenterDelta) {
		myCameraCenterDelta = g_myCameraCenterDelta;
	}

}

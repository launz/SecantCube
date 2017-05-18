using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using JellyJoystick;

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
	[SerializeField] GameObject myCameraReal;
	[SerializeField] Transform myCameraCollide;

	[SerializeField] float myCameraDistance = 10;
	[SerializeField] float myCameraSpeed = 4;
	[SerializeField] float myVerticalLimitRatio = 0.8f;
	private Vector3 myCameraCenterDelta;
	[SerializeField] float myCameraCenterDistanceRatio = 0.5f;
	[SerializeField] float myCameraCenterSpeed = 4;

	[SerializeField] float myCameraRealDistance = 3;
	[SerializeField] float myCameraRealDistance_Speed = 10;

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
	[SerializeField] bool myBoost_Forever = false;
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

	[Header("Launchpad")]
	private bool myLaunchpad_IsOn;
	private Vector3 myLaunchpad_Velocity;
	[SerializeField] float myLaunchpad_MinTime = 1;
	private float myLaunchpad_Timer;

	[Header("MoveDisplay")]
	[SerializeField] TrailRenderer[] myTrail_Array;
	[SerializeField] float myTrail_MaxWidth = 0.5f;
	[SerializeField] Color myTrail_MaxColor = new Color (1, 1, 1, 0.5f);
	[SerializeField] float myTrail_Ratio = 120;
	[SerializeField] float myTrail_Speed = 1;
	private float myTrail_Percentage = 0;
	private Color myTrail_CurrentColor = Color.white;


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
	void FixedUpdate () {

//		float t_Vertical = Input.GetAxis ("Vertical");
//		float t_Horizontal = Input.GetAxis ("Horizontal");

		UpdateMove ();
		UpdateBoost ();
		UpdateCameraBlur ();
		UpdateCameraFieldOfView ();
		UpdateCamera ();
		UpdateCameraShake ();
		UpdateLaunchpad ();
		UpdateTrail ();
	}

//	void OnDrawGizmos(){
//		Gizmos.color = Color.red;
//		float translationHorizontal = Input.GetAxis("Horizontal") * myHorizontalForce * Time.fixedDeltaTime;
//
//		Gizmos.DrawRay (transform.position, myCamera.transform.right * translationHorizontal);
//		Gizmos.DrawSphere (transform.position + myCamera.transform.up * 2f,0.3f);
//	}

	private void UpdateMove () {
		if (myLaunchpad_IsOn)
			return;
		
		float t_Vertical = JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_Y);
		float t_Horizontal = JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_X);

		if (t_Horizontal > 0.1f || t_Vertical > 0.1f) {
			Vector2 t_direction = new Vector2 (t_Horizontal, t_Vertical).normalized * myCameraRealDistance;
			t_direction = Vector3.Lerp (myCameraReal.transform.localPosition, t_direction, Time.fixedDeltaTime * myCameraRealDistance_Speed).normalized;
			myCameraReal.transform.localPosition = t_direction * myCameraRealDistance;
		}

		float translationVertical = t_Vertical * myVerticalForce * Time.fixedDeltaTime;
		float translationHorizontal = t_Horizontal * myHorizontalForce * Time.fixedDeltaTime;

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
			//Debug.Log ("Limit");
			//			myCamera.GetComponent<Rigidbody> ().MovePosition (
			//				myCamera.transform.position -
			//				myCamera.transform.right * translationHorizontal
			//			);

			//player movement
			myRigidbody.velocity = (
				myRigidbody.velocity.normalized + (
					myCamera.transform.right * translationHorizontal
				) * Time.fixedDeltaTime * myVelocityRatio
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
				) * Time.fixedDeltaTime * myVelocityRatio
			).normalized * myRigidbody.velocity.magnitude;
		}



		//airResistance
		myRigidbody.AddForce(-myRigidbody.velocity.normalized * myRigidbody.velocity.magnitude * myRigidbody.velocity.magnitude * myAirResistance);

	}

	private void UpdateCamera () {
		Vector3 t_targetPosition = this.transform.position - myCamera.transform.position;

		t_targetPosition.Normalize ();

		t_targetPosition = this.transform.position - t_targetPosition * myCameraDistance;

		//Move Camera
		myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, t_targetPosition, Time.fixedDeltaTime * myCameraSpeed);
//		Vector3 newPosition = Vector3.Lerp(myCamera.transform.position, t_targetPosition, Time.fixedDeltaTime * myCameraSpeed);
//		myCamera.GetComponent<Rigidbody>().MovePosition( newPosition );


		Vector3 t_lookAt = -JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_Y) * myCamera.transform.up +
		                   JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_X) * myCamera.transform.right;
//		Vector3 t_lookAt = Input.GetAxis ("Horizontal") * myCamera.transform.right;
//		Vector3 t_lookAt = new Vector3();
		t_lookAt = t_lookAt - Vector3.Project (t_lookAt, myCamera.transform.position - this.transform.position);
		t_lookAt.Normalize ();
		if (t_lookAt.magnitude != 0) {
			t_lookAt = t_lookAt * myCameraCenterDistanceRatio * (myCamera.transform.position - this.transform.position).magnitude;
			//ease
			myCameraCenterDelta = Vector3.Lerp (myCameraCenterDelta, t_lookAt, Time.fixedDeltaTime * myCameraCenterSpeed);
		}
//		Debug.Log (this.transform.position + " " + myCameraCenterDelta);

		//Update Camera collide
		myCameraCollide.position = myCameraCenterDelta + this.transform.position;
		myCameraCollide.GetComponent<Rigidbody> ().MovePosition (myCameraCenterDelta + this.transform.position);

		myCamera.transform.LookAt (this.transform.position + myCameraCenterDelta);

		myCamera.GetComponent<Rigidbody> ().velocity = Vector3.zero;



	}

	public void BoostForever () {
		myBoost_Forever = true;
		myBoost_EnergyCurrent = myBoost_EnergyMax;
		myBoost_UsePerSecond = 0;
		myRigidbody.useGravity = false;
	}

	private void UpdateBoost () {
		if (InputGetButtonBoost () || myLaunchpad_IsOn) {
//			Debug.Log ("Boost");
			//Debug.Log ("myBoost_EnergyCurrent: " + myBoost_EnergyCurrent);		
			//if has energy
			if (myBoost_EnergyCurrent > 0) {

				//use boost energy
				if (myBoost_Forever == false) {
					if (myBoost_IsUsedByPercentage)
						myBoost_EnergyCurrent -= myBoost_UsePerSecond * myBoost_EnergyMax * Time.fixedDeltaTime;
					else
						myBoost_EnergyCurrent -= myBoost_UsePerSecond * Time.fixedDeltaTime;
				}

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
		if (myBoostDisplay == null)
			return;
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

		myCameraReal.GetComponent<MotionBlur> ().blurAmount = Mathf.Lerp (
			myCameraReal.GetComponent<MotionBlur> ().blurAmount, 
			t_blur, 
			Time.fixedDeltaTime * myBlur_Speed
		);
	}

	private void UpdateCameraShake () {
		float t_shakeIntensity;
		if (InputGetButtonBoost ())
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
			Time.fixedDeltaTime * myShake_Speed
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

		myCameraReal.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (
			myCameraReal.GetComponent<Camera> ().fieldOfView, 
			t_fieldOfView, 
			Time.fixedDeltaTime * myFieldOfView_Speed
		);
	}

	private void UpdateTrail () {
		//set trail display percentage
		float t_percentage = 1 - myTrail_Ratio / (myRigidbody.velocity.magnitude + myTrail_Ratio);
		//drag
		myTrail_Percentage = Mathf.Lerp (myTrail_Percentage, t_percentage, Time.fixedDeltaTime * myBlur_Speed);
		myTrail_CurrentColor = new Color (myTrail_MaxColor.r, myTrail_MaxColor.g, myTrail_MaxColor.b, myTrail_MaxColor.a * myTrail_Percentage);
		foreach (TrailRenderer t_trail in myTrail_Array) {
			t_trail.startWidth = myTrail_Percentage * myTrail_MaxWidth;
			t_trail.material.color = myTrail_CurrentColor;
		}
	}

	public void ClearTrail () {
		this.GetComponent<TrailRenderer> ().Clear ();
		foreach (TrailRenderer t_trail in myTrail_Array) {
			t_trail.Clear ();
		}
	}

	private void UpdateLaunchpad () {
		if (myLaunchpad_IsOn == false)
			return;

		if (myLaunchpad_Timer > 0) {
			myLaunchpad_Timer -= Time.fixedDeltaTime;
		} 
//		else if (JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_X) != 0 ||
//		           JellyJoystickManager.Instance.GetAxis (AxisMethodName.Normal, 1, JoystickAxis.LS_Y) != 0 ||
//		           InputGetButtonBoost ()) {
//			myLaunchpad_IsOn = false;
//			//myLaunchpad_Velocity = Vector3.zero;
//			return;
//		}
//			
//		Debug.Log ("Launchpad");
		myRigidbody.velocity = myLaunchpad_Velocity;
	}

	public void StartLaunchpad (Vector3 g_velocity) {
		myLaunchpad_Timer = myLaunchpad_MinTime;
		SetLaunchpad (g_velocity);
		myLaunchpad_IsOn = true;
		Debug.Log ("StartLaunchpad");
	}

	public void SetLaunchpad (Vector3 g_velocity) {
		myLaunchpad_Velocity = g_velocity;
	}

	public Vector3 GetLaunchpad () {
		return myLaunchpad_Velocity;
	}

	void OnCollisionStay (Collision g_collision) {
		BoostRecover (g_collision.transform);
		BoostLose (g_collision.transform);
		//Debug.Log ("Hit" + g_collision.transform.name);
		StopLaunchpad (g_collision.transform);
	}

	void OnTriggerStay(Collider g_other) {
		BoostRecover (g_other.transform);
		BoostLose (g_other.transform);
		//Debug.Log ("Hit" + g_other.transform.name);
		StopLaunchpad (g_other.transform);
	}

	private void StopLaunchpad (Transform g_collide) {
		if (myLaunchpad_Timer > 0)
			return;

//		Debug.Log ("StopLaunchpad");
		
		if (g_collide.tag != CS_Global.TAG_LAUNCHPAD && 
			g_collide.tag != CS_Global.TAG_BOOST && 
			g_collide.tag != CS_Global.TAG_BOOSTLOSE && 
			g_collide.tag != CS_Global.TAG_PORTAL)
			myLaunchpad_IsOn = false;
	}

	private void BoostRecover (Transform g_Transform) {
		if (g_Transform.tag != CS_Global.TAG_BOOST)
			return;

		//recharge boost energy
		if(myBoost_IsRechargedByPercentage)
			myBoost_EnergyCurrent += myBoost_EnergyMax * myBoost_RechargePerSecond * Time.fixedDeltaTime;
		else
			myBoost_EnergyCurrent +=  myBoost_RechargePerSecond * Time.fixedDeltaTime;
		//check if the boost energy > max
		if (myBoost_EnergyCurrent > myBoost_EnergyMax)
			myBoost_EnergyCurrent = myBoost_EnergyMax;
	}

	private void BoostLose (Transform g_Transform) {
		if (g_Transform.tag != CS_Global.TAG_BOOSTLOSE)
			return;

		if (myBoost_Forever == false)
			myBoost_EnergyCurrent = 0;
	}

	public GameObject GetMyCamera () {
		return myCamera;
	}

	public GameObject GetMyCameraReal () {
		return myCameraReal;
	}

	public Vector3 GetMyCameraCenterDelta () {
		return myCameraCenterDelta;
	}

	public void SetMyCameraCenterDelta (Vector3 g_myCameraCenterDelta) {
		myCameraCenterDelta = g_myCameraCenterDelta;
	}

	private bool InputGetButtonBoost () {
		if (JellyJoystickManager.Instance.GetButton (ButtonMethodName.Hold, 1, JoystickButton.A) ||
		    JellyJoystickManager.Instance.GetButton (ButtonMethodName.Hold, 1, JoystickButton.B) ||
		    JellyJoystickManager.Instance.GetButton (ButtonMethodName.Hold, 1, JoystickButton.X) ||
			JellyJoystickManager.Instance.GetButton (ButtonMethodName.Hold, 1, JoystickButton.Y)) {
			return true;
		} else
			return false;
	}
}

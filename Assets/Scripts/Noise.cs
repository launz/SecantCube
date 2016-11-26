using UnityEngine;
using System;  // Needed for Random

public class Noise : MonoBehaviour
{
	// un-optimized version of a noise generator
	private System.Random RandomNumber = new System.Random();
	public float offset = 0;

	public GameObject player;

	public Rigidbody playerBody;

	public bool _isPlayerNoise;

	public double frequency = 440;
	public double frequency2 = 660;
	public double gain = 0.05;

	private double increment;
	private double increment2;
	private double phase;
	private double phase2;
	private double sampling_frequency = 44100;

	void Start() {

		player = GameObject.FindGameObjectWithTag ("Player");
		playerBody = player.GetComponent<Rigidbody> ();

	}

	void FixedUpdate() {

		//want the playerNoise object to behave differently - dependent on 

		if (!_isPlayerNoise) {
			frequency = (double)(Mathf.Clamp (Mathf.Abs (playerBody.velocity.y * 2f), 100f, 4000f));
			frequency2 = (double)(playerBody.velocity.magnitude);
		}

	}

	void OnAudioFilterRead(float[] data, int channels)
	{

		for (int i = 0; i < data.Length; i=i+channels) {

			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] =  (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;

		}

		for (int i = 0; i < data.Length; i=i+channels) {

			phase2 = phase2 + increment2;
			// this is where we copy audio data to make them “available” to Unity
			data[i] *=  (float)(gain*Math.Sin(phase2)) + 1;
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;

		}

		for (int i = 0; i < data.Length; i++)
		{
			data[i] *=  offset + (float)RandomNumber.NextDouble()*2.0f;
			
		}




		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		increment2 = frequency2 * 2 * Math.PI / sampling_frequency;

		//add a sine am?

	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ReplacementShaderEffect : MonoBehaviour
{
    public Shader ReplacementShader;
    public Color startColor;
    public Color midColor;
    public Color endColor;
	Camera mainCam;

	// [Range(0.0f,1.0f)]
	// public float BlueMultColor;
	// [Range(0.0f,1.0f)]
	// public float PinkMultColor;
	// [Range(0.0f,1.0f)]
	// public float YellowMultColor;

	// [Range(-2.0f,2.0f)]
	// public float BluePlusColor;
	// [Range(-2.0f,2.0f)]
	// public float PinkPlusColor;
	// [Range(-2.0f,2.0f)]
	// public float YellowPlusColor;


	void Start() {	

		mainCam = GetComponent<Camera> ();
		Shader.SetGlobalColor("_StartColor", startColor);
		Shader.SetGlobalColor("_MidColor", midColor);
		Shader.SetGlobalColor("_EndColor", endColor);

	}

	void Update() {		//	make background color always the same as end color in shader
		
		mainCam.backgroundColor = endColor;
	}

    void OnValidate()
    {
    	Shader.SetGlobalColor("_StartColor", startColor);
    	Shader.SetGlobalColor("_MidColor", midColor);
    	Shader.SetGlobalColor("_EndColor", endColor);
  //       Shader.SetGlobalColor("_OverDrawColor", OverDrawColor);
		// Shader.SetGlobalColor("_OverDrawColorPlus", OverDrawColorPlus);
		// Shader.SetGlobalFloat("_BlueMultColor", BlueMultColor);
		// Shader.SetGlobalFloat("_PinkMultColor", PinkMultColor);
		// Shader.SetGlobalFloat("_YellowMultColor", YellowMultColor);
		// Shader.SetGlobalFloat("_BluePlusColor", BluePlusColor);
		// Shader.SetGlobalFloat("_PinkPlusColor", PinkPlusColor);
		// Shader.SetGlobalFloat("_YellowPlusColor", YellowPlusColor);

    }

    void OnEnable()
    {
        if (ReplacementShader != null)
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
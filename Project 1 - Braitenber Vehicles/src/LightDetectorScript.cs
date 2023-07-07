using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class LightDetectorScript : MonoBehaviour
{

	public float angle = 360; // Angle of detection.
	public bool ApplyThresholds, ApplyLimits; // Whether limits and thresholds should be applied or not to Gaussian Function.
	public bool inverse; // Whether the output to the wheels should be inverted or not.
	public float min_x, max_x, min_y, max_y, mean, stdDev; // Thresholds, limits, mean and standard deviation pharameters to be given to Gaussian Function.
	private bool useAngle = true; // Whether get all lights or only visible lights.

	public float output; // Final outputed value to activation functions.
	public int numObjects; // Number of Lights with proper TAG detected.

	// Start of function.
	void Start(){
		output = 0;
		numObjects = 0;

		if (angle > 360){
			useAngle = false;
		}
	}

	// Regularly update the output that should be emitted by the sensor.
	void Update(){
		GameObject[] lights;

		if (useAngle)
			lights = GetVisibleLights();
		else
			lights = GetAllLights();

		output = 0;
		numObjects = lights.Length;

		// For each light in visible range, compute the output transmitted by the combination of them.
		foreach (GameObject light in lights){
			float r = light.GetComponent<Light>().range;
			output += 1.0f / ((transform.position - light.transform.position).sqrMagnitude / r + 1);
		}
	}

	public virtual float GetOutput() { throw new NotImplementedException(); }

	// Returns all "Light" tagged objects. The sensor angle is not taken into account.
	GameObject[] GetAllLights(){
		return GameObject.FindGameObjectsWithTag("Light");
	}

	// Returns all "Light" tagged objects that are within the view angle of the Sensor. 
	// Only considers the angle over the y axis. Does not consider objects blocking the view.
	GameObject[] GetVisibleLights(){
		ArrayList visibleLights = new ArrayList();
		float halfAngle = angle / 2.0f;

		GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

		foreach (GameObject light in lights){
			Vector3 toVector = (light.transform.position - transform.position);
			Vector3 forward = transform.forward;
			toVector.y = 0;
			forward.y = 0;
			float angleToTarget = Vector3.Angle(forward, toVector);

			if (angleToTarget <= halfAngle)
				visibleLights.Add(light);
		}
		return (GameObject[])visibleLights.ToArray(typeof(GameObject));
	}


}

using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CarDetectorScript : MonoBehaviour {

    public float angle = 360; // Angle of detection.
    public bool ApplyThresholds, ApplyLimits; // Whether limits and thresholds should be applied or not to Gaussian Function.
    public bool inverse; // Whether the output to the wheels should be inverted or not.
    public float min_x, max_x, min_y, max_y, mean, stdDev; // Thresholds, limits, mean and standard deviation pharameters to be given to Gaussian Function.
    private bool useAngle = true; // Whether get all cars or only visible cars.

    public float output; // Final outputed value to activation functions.
    public int numObjects; // Number of Cars with proper TAG detected.

    // Start of function.
    void Start(){
        output = 0;
        numObjects = 0;

        if (angle > 360)
            useAngle = false;
    }

    // Regularly update the output that should be emitted by the sensor.
    void Update(){
        var MIN = Mathf.Infinity;
        GameObject[] cars = GetVisibleCars();
        GameObject closestCar = null;

        // From all visible cars use only the one closest to the sensor.
        foreach (GameObject car in cars){
            var dist = Vector3.Distance(transform.position, car.transform.position);
            if (dist < MIN){
                MIN = dist;
                closestCar = car;
            }
        }

        // Output using given formula. If car not detected the output will be 0.
        if (MIN < Mathf.Infinity)
            output = 1.0f / ((transform.position - closestCar.transform.position).sqrMagnitude + 1);
        else
            output = 0;
    }

    public virtual float GetOutput() { throw new NotImplementedException(); }

    // Returns all "CarToFollow" tagged objects. The sensor angle is not taken into account.
    GameObject[] GetAllCars(){
        return GameObject.FindGameObjectsWithTag("CarToFollow");
    }

    // Returns all "CarToFollow" tagged objects that are within the view angle of the Sensor. 
    // Only considers the angle over the y axis. Does not consider objects blocking the view.
    GameObject[] GetVisibleCars(){
        ArrayList visibleCars = new ArrayList();
        float halfAngle = angle / 2.0f;

        GameObject[] cars = GameObject.FindGameObjectsWithTag("CarToFollow");

        foreach (GameObject car in cars){
            Vector3 toVector = (car.transform.position - transform.position);
            Vector3 forward = transform.forward;
            toVector.y = 0;
            forward.y = 0;
            float angleToTarget = Vector3.Angle(forward, toVector);

            if (angleToTarget <= halfAngle)
                visibleCars.Add(car);
        }
        return (GameObject[])visibleCars.ToArray(typeof(GameObject));
    }
}

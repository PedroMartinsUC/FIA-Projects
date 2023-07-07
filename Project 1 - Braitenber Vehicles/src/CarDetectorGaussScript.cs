using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CarDetectorGaussScript : CarDetectorScript
{

    // Get gaussian output value, including inverse option
    public override float GetOutput(){

        if (inverse)
            return 1 - GaussianOutput(min_x, max_x, min_y, max_y, mean, stdDev);
        else
            return GaussianOutput(min_x, max_x, min_y, max_y, mean, stdDev);

    }

    // Computes gaussian function with mean and stdDev parameters
    private float GaussianFunction(float mean, float stdDev){
        // Only calculated if stdDev != 0 (otherwise math impossibility)
        if (stdDev != 0)
            return (float)(Math.Pow(Math.E, -0.5 * Math.Pow((output - mean) / (stdDev), 2)) / (stdDev * Math.Sqrt(2 * Math.PI)));
        else
            return 0;
    }

    // Gaussian output with ranges for the x and y axis
    public float GaussianOutput(float min_x, float max_x, float min_y, float max_y, float mean, float stdDev){
        // Output to be provided as a result of gaussian computation. Starts with the minimum y value set
        float strength = 0;

        // Apply limts and thresholds
        if (ApplyThresholds && ApplyLimits){
            if ((output <= max_x) && (min_x <= output)){
                strength = GaussianFunction(mean, stdDev);
                if (strength <= min_y)
                    strength = min_y;
                else if (strength >= max_y)
                    strength = max_y;
            }
        }
        // Only apply thresholds
        else if (ApplyThresholds && !ApplyLimits){
            if ((output <= max_x) && (min_x <= output))
                strength = GaussianFunction(mean, stdDev);
        }
        // Only apply limits
        else if (!ApplyThresholds && ApplyLimits){
            strength = GaussianFunction(mean, stdDev);
            if (strength <= min_y)
                strength = min_y;
            else if (strength >= max_y)
                strength = max_y;
        }
        // Do not apply limits nor thresholds
        else if (!ApplyThresholds && !ApplyLimits)
            strength = GaussianFunction(mean, stdDev);

        //Value given to the wheels (could still be inverted).
        return strength;
    }
}
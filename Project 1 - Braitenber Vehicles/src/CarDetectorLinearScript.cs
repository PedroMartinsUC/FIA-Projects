using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CarDetectorLinearScript : CarDetectorScript {

	// Returns the output to the wheels, which could be inverted or not.
	public override float GetOutput(){
		if (inverse)
			return 1 - output;
		else
			return output;
	}
}

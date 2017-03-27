using UnityEngine;
using System.Collections;

public class RotateByTime : MonoBehaviour {
    public float degreesPerSecond = 90.0f;
    public Vector3 rotateAxis = Vector3.up;

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotateAxis, degreesPerSecond * Time.deltaTime);
	}
}

using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
    public float forwardSpeedMetersPerSecond = 2.0f;
    public float rotationDegreesPerSecond = 90.0f;
	void Update () {
        transform.position += transform.forward * Input.GetAxis("Vertical") * forwardSpeedMetersPerSecond * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationDegreesPerSecond * Time.deltaTime, 0);
    }
}

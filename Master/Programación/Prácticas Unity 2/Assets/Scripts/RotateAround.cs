using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
    public Transform target;
    public float degreesPerSecond = 90.0f;
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(target.position, Vector3.up, degreesPerSecond * Time.deltaTime);
	}
}

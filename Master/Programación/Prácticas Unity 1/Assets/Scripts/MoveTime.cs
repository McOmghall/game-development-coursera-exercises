using UnityEngine;
using System.Collections;

public class MoveTime : MonoBehaviour {
    public float movementSpeedPerSecond = 10f;
	void Update () {
        transform.position = transform.position + Vector3.Normalize(transform.forward) * movementSpeedPerSecond * Time.deltaTime;
	}
}

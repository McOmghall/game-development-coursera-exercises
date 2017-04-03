using UnityEngine;
using System.Collections;

public class MovementPistolController : MonoBehaviour {
    public float maximumDeviationFromZeroHeight = 10.0f;
    public float speedInMetersPerSecond = 2.0f;

	void Update () {
        transform.position += Vector3.up * Input.GetAxis("Vertical") * speedInMetersPerSecond * Time.deltaTime;
        if (Mathf.Abs(transform.position.y) >= maximumDeviationFromZeroHeight) {
            transform.position = new Vector3(transform.position.x, Mathf.Sign(transform.position.y) * maximumDeviationFromZeroHeight, transform.position.z);
        }
    }
}

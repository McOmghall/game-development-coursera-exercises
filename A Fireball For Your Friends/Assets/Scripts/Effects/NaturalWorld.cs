using UnityEngine;
using System.Collections;

public class NaturalWorld : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
        foreach(Light l in GetComponentsInChildren<Light>()) {
            l.transform.RotateAround(GameObject.FindGameObjectWithTag("Player").transform.position, new Vector3(1, 0, 0), 30 * Mathf.Sin(Time.deltaTime) * Time.deltaTime);
            l.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }
}

using UnityEngine;
using System.Collections;
using Editor;

public class Shooting : MonoBehaviour {
    public GameObject bullet;
    public float secondsBetweenShots = 0.5f;
    public float initialVelocity = 100.0f;
    [ReadOnly] public Barrel barrel;
    [ReadOnly] public float timeOfNextShot = float.MinValue;
	
    void Start () {
        barrel = GetComponentInChildren<Barrel>();
    }

	void Update () {
	    if (Time.time > timeOfNextShot && Input.GetAxis("Fire1") > 0) {
            timeOfNextShot = Time.time + secondsBetweenShots;

            // Shoot
            GameObject firedBullet = Instantiate(bullet, barrel.transform.position, Quaternion.identity, null) as GameObject;
            firedBullet.GetComponent<Rigidbody>().AddForce(Vector3.right * initialVelocity, ForceMode.Impulse);
        }
	}
}

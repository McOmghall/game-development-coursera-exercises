using UnityEngine;
using System.Collections;

public class HealthDamageable : MonoBehaviour, IDamageable {
    public float currentHealth = 20.0f;
	
    public void damage (float damage) {
        currentHealth -= damage;
        if (currentHealth < 0) {
            Debug.Log("Destroyed HealthDamageable " + gameObject.name);
            Destroy(gameObject);
        }
    }

	void Update () {
	
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamagingStuff : MonoBehaviour {
  public float damageAmount = 10f;

  private Collider thisCollider;

	void Start () {
    thisCollider = GetComponent<Collider>();
	}

  protected void OnCollisionEnter(Collision collision) {
    Health impact = collision.gameObject.GetComponent<Health>();
    if (impact != null) {
      impact.damage(damageAmount);
    }
  }

  void OnTriggerEnter (Collider collider) {
    Health impact = collider.gameObject.GetComponent<Health>();
    if (impact != null) {
      impact.damage(damageAmount);
    }
  }
}

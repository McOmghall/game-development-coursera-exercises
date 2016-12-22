using UnityEngine;
using System.Collections;
using System;

public class NormalFireball : ShootableFireball {
  private Vector3 originInternal;
  private float distanceToTargetOriginal;

  public float timeToDestructionAfterImpact = 1;

  internal override void fireTo(Vector3 target) {
    transform.LookAt(target);
    originInternal = transform.position;
    setStatus(FireballStates.SHOT);

    Vector3 vectorToTarget = target - originInternal;
    distanceToTargetOriginal = vectorToTarget.magnitude;
    GetComponent<Rigidbody>().velocity = vectorToTarget.normalized * (distanceToTargetOriginal / shootTime);
  }

  void FixedUpdate() {
    if (status.Equals(FireballStates.SHOT)) {
      float distanceToOriginSquared = (originInternal - transform.position).sqrMagnitude;
      if (distanceToTargetOriginal * distanceToTargetOriginal < distanceToOriginSquared) {
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
      }
    }
  }

  internal override float timeToDestruction() {
    return timeToDestructionAfterImpact;
  }
}
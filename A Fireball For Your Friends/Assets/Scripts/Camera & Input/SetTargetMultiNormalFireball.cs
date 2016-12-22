using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetMultiNormalFireball : MonoBehaviour {
  public Vector3 target = Vector3.zero;

	void Start () {
    transform.GetComponent<ChargeableMultiNormalFireball>().setTarget(target);
	}
}

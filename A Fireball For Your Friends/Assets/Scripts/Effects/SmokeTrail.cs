using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SmokeTrail : MonoBehaviour {
	public void Play() {
        GetComponent<ParticleSystem>().Play();
    }
}

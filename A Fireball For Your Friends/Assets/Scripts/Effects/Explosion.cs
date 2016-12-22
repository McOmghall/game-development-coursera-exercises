using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Explosion : MonoBehaviour {
    public void Play() {
        GetComponent<ParticleSystem>().Play();
    }
}

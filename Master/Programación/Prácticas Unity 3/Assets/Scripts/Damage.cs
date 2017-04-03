using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Damage : MonoBehaviour {
    public float damageAmount = 10.0f;

    void OnCollisionEnter(Collision collision) {
        IDamageable damage = collision.gameObject.GetComponent<IDamageable>();

        if (damage != null) {
            damage.damage(damageAmount);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class KillAfterSeconds : MonoBehaviour {
    public float secondsToDie = 5.0f;

    void Start () {
        Destroy(gameObject, secondsToDie);
    }
}

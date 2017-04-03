using UnityEngine;
using Editor;

public class Spawner : MonoBehaviour {
    public GameObject spawnThisEnemy;
    public float secondsBetweenSpawnsMinimum = 1.0f;
    public float secondsBetweenSpawnsExtraRandomFactor = 0.5f;
    [ReadOnly] public float timeOfNextSpawn = float.MinValue;

	void Update () {
	    if (Time.time >= timeOfNextSpawn) {
            timeOfNextSpawn = Time.time + secondsBetweenSpawnsMinimum + Random.Range(0, secondsBetweenSpawnsExtraRandomFactor);
            Instantiate(spawnThisEnemy, transform.position, Quaternion.identity, null);
        }
	}
}

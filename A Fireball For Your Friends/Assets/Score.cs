using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
  public float scoreToPlayer = 10;

  void OnDestroy () {
    if (GameObject.FindGameObjectWithTag("Player") != null) {
      GameObject.FindGameObjectWithTag("Player").GetComponent<InputControl>().addScore(scoreToPlayer);
    }
  }
}

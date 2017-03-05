using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class EnemyIntelligence : MonoBehaviour {
  public float distanceToAttack = 10;

  enum States {
    ADVANCING,
    ATTACKING,
    FLEEING
  }

  private Health health;
  private Animator animator;
  private Transform player;
  private States state;

  void Start () {
    health = GetComponent<Health>();
    animator = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }
	
	// Update is called once per frame
	void Update () {
    if (Vector3.Distance(transform.position, player.position) < distanceToAttack) {
      state = States.ATTACKING;
    } else if (health.healthPercentage() <= 0.3) {
      state = States.FLEEING;
    } else {
      state = States.ADVANCING;
    }
 
    switch (state) {
      case States.ATTACKING:
        attack();
        break;
      case States.ADVANCING:
        advance();
        break;
      case States.FLEEING:
        flee();
        break;
      default:
        break;
    }
	}

  private void attack() {
    animator.SetTrigger("Attack1Trigger");
    animator.SetBool("Moving", false);
    animator.SetBool("Running", false);

    transform.LookAt(Vector3.Scale(player.position, new Vector3(1, 0 ,1)));
  }

  private void advance() {
    animator.SetBool("Moving", true);
    animator.SetBool("Running", true);

    transform.LookAt(Vector3.Scale(player.position, new Vector3(1, 0, 1)));
  }

  private void flee() {
    animator.SetBool("Moving", true);
    animator.SetBool("Running", true);

    // Lookaway
    transform.LookAt(2 * transform.position - Vector3.Scale(player.position, new Vector3(1, 0, 1)));
  }
}

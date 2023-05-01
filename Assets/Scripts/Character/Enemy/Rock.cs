using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour {
    public enum RockStates { 
        HitPlayer, HitEnemy, HitNothing 
    }
    private Rigidbody rb;

    [HideInInspector]
    public RockStates rockStates;
    
    [HideInInspector]
    public GameObject target;

    [Header("Basic Settings")]
    public float force;
    public int damage;
    private Vector3 direction;
    public GameObject breakEffect;

    #region 生命周期函数
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate() {
        if (rb.velocity.sqrMagnitude < 1f) {
            rockStates = RockStates.HitNothing;
        }
    }

    void OnCollisionEnter(Collision other) {
        switch(rockStates) {
            // 反击石头人
            case RockStates.HitEnemy: {
                if (other.gameObject.CompareTag("Enemy")) {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);
                    other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject, 0.2f);
                }
                break;
            }
            // 砸向玩家
            case RockStates.HitPlayer: {
                if (other.gameObject.CompareTag("Player")) {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());
                    rockStates = RockStates.HitNothing;
                }
                break;
            }
            case RockStates.HitNothing: {
                break;
            }
        }
    }

#endregion


    public void FlyToTarget() {
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}

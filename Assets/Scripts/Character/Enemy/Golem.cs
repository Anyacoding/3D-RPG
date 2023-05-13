using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController  {
    [Header("Skill")]
    public float kickForce = 10;
    public GameObject rockPrefab;
    public Transform handPos;

#region 动画事件
    public void kickOff() {
        if (attackTarget != null && transform.isFacingTarget(attackTarget.transform)) {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }

    public void ThrowRock() {
        var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
        
        if (attackTarget != null) {
            rock.GetComponent<Rock>().target = attackTarget;
        }
        if (attackTarget == null) {
            rock.GetComponent<Rock>().target = FindObjectOfType<PlayerController>().gameObject;
        }
    }

#endregion

}

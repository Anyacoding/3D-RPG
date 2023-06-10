using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject attackTarget;
    private float lastAttackTime = 0.5f;
    private CharacterStats characterStats;
    private bool isDead;

#region 生命周期函数
    // Awake is called before the first frame update
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        UnityEngine.Debug.Log("Awake()");
    }

    void OnEnable() {
        // 向事件注册函数
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        // 向GameManager注册自身为玩家
        GameManager.Instance.RigisterPlayer(characterStats);
        UnityEngine.Debug.Log("OnEnable()");
    }

    void Start() {
        // 加载存档
        SaveManager.Instance.LoadPlayerData();
    }

    
    // Update is called once per frame
    void Update() {
        if (characterStats.CurrentHealth == 0) {
            isDead = true;
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        // 如果死亡就注销事件
        if (isDead) {
            MouseManager.Instance.OnMouseClicked -= MoveToTarget;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
        }
    }

    void OnDisable() {
        if (MouseManager.IsInitialized == false) return;
        // 取消订阅
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

#endregion


#region 切换函数
    private void SwitchAnimation() {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("isDead", isDead);
    }
#endregion


#region 事件函数
    public void MoveToTarget(Vector3 target) {
        StopAllCoroutines();
        
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target) {
        if (target != null) {
            attackTarget = target;
            characterStats.attackData.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    void Hit() {
        if (attackTarget != null) {
            if (attackTarget.CompareTag("Attackable")) {
                if (attackTarget.GetComponent<Rock>()) {
                    attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                    attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                    attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                }
            }
            else {
                var targetState = attackTarget.GetComponent<CharacterStats>();
                targetState.TakeDamage(characterStats, targetState);
            }
        }
    }

#endregion


#region 辅助函数
    // 协程
    IEnumerator MoveToAttackTarget() {
        // Run to Enemy
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);
        
        while (attackTarget != null && Vector3.Distance(transform.position, attackTarget.transform.position) > characterStats.attackData.attackRange) {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        // Attack
        agent.isStopped = true;

        if (lastAttackTime < 0 && attackTarget != null) {
            animator.SetBool("Critical", characterStats.attackData.isCritical);
            animator.SetTrigger("Attack");
            // 重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
        
    }
#endregion

}

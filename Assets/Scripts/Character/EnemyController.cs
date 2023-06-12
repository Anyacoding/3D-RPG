using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { GUARD, PATROL, CHASE, DEAD }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver {
    private NavMeshAgent agent;
    private EnemyState enemyState;
    private Animator animator;
    private CharacterStats characterStats;
    private Collider coll;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float lookAtTime;
    private float speed;
    protected GameObject attackTarget;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;

    private Quaternion guardRotation;

    // 配合动画
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

#region 生命周期函数
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    void Start() {
        if (isGuard) {
            enemyState = EnemyState.GUARD;
        }
        else {
            enemyState = EnemyState.PATROL;
            wayPoint = GetNewWayPoint();
        }
    }

    void OnEnable() {
        GameManager.Instance.AddObserver(this);
    }

    void OnDisable() {
        if (GameManager.IsInitialized) {
            GameManager.Instance.RemoveObserver(this);
        }
        // 死亡掉落物体
        if (GetComponent<LootSpawner>() && isDead) {
            GetComponent<LootSpawner>().SpawnLoot();
        }
    }

    void Update() {
        if (characterStats.CurrentHealth == 0) {
            isDead = true;
        }
        // 玩家存活时才进行状态切换
        if (!playerDead) {
            SwitchStates();
            lastAttackTime -= Time.deltaTime;
        }
        SwitchAnimation();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
#endregion

#region 切换函数
    private void SwitchAnimation() {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Critical", characterStats.attackData.isCritical);
        animator.SetBool("isDead", isDead);
    }

    void SwitchStates() {
        if (isDead) {
            enemyState = EnemyState.DEAD;
        }
        else if (FoundPlayer()) {
            enemyState = EnemyState.CHASE;
        }

        switch(enemyState) {
            case EnemyState.GUARD: {
                Guard();
                break;
            }
            case EnemyState.PATROL: {
                Patrol();
                break;
            }
            case EnemyState.CHASE: {
                Chase();
                break;
            }
            case EnemyState.DEAD: {
                Dead();
                break;
            }   
        }
    }
#endregion
    
#region 状态函数
    
    void Chase() {
        agent.speed = speed;
        isWalk = false;
        isChase = true;

        if (!FoundPlayer()) {
            isFollow = false;
            isChase = false;
            agent.destination = transform.position;
            
            if (isGuard)
                enemyState = EnemyState.GUARD;
            else 
                enemyState = EnemyState.PATROL;
        }
        else {
            isFollow = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
            agent.speed = speed;
        }

        if(TargetInAttackRange() || TargetInSkillRange()) {
            isFollow = false;
            // 攻击时不能平移
            agent.isStopped = true;

            if (lastAttackTime < 0) {
                // 重置冷却时间
                lastAttackTime = characterStats.attackData.coolDown;
                // 暴击判断
                characterStats.attackData.isCritical = Random.value < characterStats.attackData.criticalChance;
                // 执行攻击
                Attack();
            }
        }
    }

    void Patrol() {
        isChase = false;
        agent.speed = speed * 0.5f;

        // 判断是否到了随机巡逻点
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance) {
            isWalk = false;
            // 看会风景
            if (remainLookAtTime > 0) {
                remainLookAtTime -= Time.deltaTime;
            }
            else {
                remainLookAtTime = lookAtTime;
                wayPoint = GetNewWayPoint();
            }
        }
        else {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    void Guard() {
        isChase = false;
        if (transform.position != guardPos) {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;

            // 回到守卫的位置，并用线性插值平缓进行旋转
            if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance) {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
            }
        }
    }

    void Dead() {
        coll.enabled = false;
        agent.enabled = false;
        Destroy(gameObject, 2f);
    }

    void Attack() {
        transform.LookAt(attackTarget.transform);
        
        // 近战攻击动画
        if (TargetInAttackRange()) {
            animator.SetTrigger("Attack");
        }
        if (TargetInSkillRange()) {
            animator.SetTrigger("Skill");
        }
    }

#endregion

#region 事件函数
    void Hit() {
        if (attackTarget != null && transform.isFacingTarget(attackTarget.transform)) {
            var targetState = attackTarget.GetComponent<CharacterStats>();
            targetState.TakeDamage(characterStats, targetState);
        }
    }

#endregion


#region 辅助函数
    bool FoundPlayer() {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach(var target in colliders) {
            if (target.CompareTag("Player")) {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    Vector3 GetNewWayPoint() {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        randomPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;

        return randomPoint;
    }

    bool TargetInAttackRange() {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else 
            return false;
    }

    bool TargetInSkillRange() {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else 
            return false;
    }

#endregion

#region 实现观察者接口
    public void EndNotify() {
        isChase = false;
        isWalk = false;
        isFollow = false;
        attackTarget = null;
        playerDead = true;
        animator.SetBool("Win", true);
    }

#endregion

}

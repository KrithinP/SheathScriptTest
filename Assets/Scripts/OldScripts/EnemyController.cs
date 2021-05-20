using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10.0f;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;
    CharacterStats myStats;

    AnimController animController;
    Vector3 MoveDirection = Vector3.zero;
    public float horizVel =0;
    public float VerticalVel =0;
    public float ZVel =0;
    float Speed = 4;
    Animator enemyAnimator;
    const int ENEMYIDLE = 0;
    const int ENEMYWALK = 1;
    const int ENEMYATTACK = 2;
    const int ENEMYDEATH = 3;

    public AnimController AnimController
    {
        get => default;
        set
        {
        }
    }

    public CharacterCombat CharacterCombat
    {
        get => default;
        set
        {
        }
    }

    // Enemy enemy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = AnimController.instance.MyHumanoid.transform;
        combat = GetComponent<CharacterCombat>();
        // agent.Warp(target.position);
        enemyAnimator = GetComponentInChildren<Animator>(); //UD_demo_character
        myStats = GetComponent<CharacterStats>();
        //enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myStats.currentHealth>0&&!myStats.died)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if(distance <= lookRadius) 
            {
                agent.SetDestination(target.position);
                enemyAnimator.SetInteger("EnemyCondition", ENEMYWALK);
                if(distance<=agent.stoppingDistance)
                {
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    if(targetStats != null)
                    {
                        combat.Attack(targetStats);
                    }

                    FaceTarget();
                    if(targetStats.currentHealth>0)
                    {
                        enemyAnimator.SetInteger("EnemyCondition", ENEMYATTACK);
                        ZVel = 0;
                        VerticalVel = 0;
                        horizVel = 0;
                        StartCoroutine (idle());
                        MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                        MoveDirection *= Speed;
                        MoveDirection = transform.TransformDirection(MoveDirection);
                    }
                    else
                    {
                        enemyAnimator.SetInteger("EnemyCondition", ENEMYIDLE);
                        ZVel = 0;
                        VerticalVel = 0;
                        horizVel = 0;
                        MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                        MoveDirection *= Speed;
                        MoveDirection = transform.TransformDirection(MoveDirection);
                    }

                }
            
            }
        }
        else
        {
            Die();
        }
        

    }
    void Die()
    {
        myStats.died=true;
        StartCoroutine (conditionChanger());
        enemyAnimator.SetInteger("EnemyCondition", ENEMYDEATH);
        ZVel = 0;
        VerticalVel = 0;
        horizVel = 0;
        MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
        MoveDirection *= Speed;
        MoveDirection = transform.TransformDirection(MoveDirection);
    }
    void FaceTarget()
    {
        Vector3 direction =  (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,Time.deltaTime * 5f);
    }
    
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    IEnumerator idle()
    {
        yield return new WaitForSeconds(2.0f);
        enemyAnimator.SetInteger("EnemyCondition", ENEMYIDLE);
        ZVel = 0;
        VerticalVel = 0;
        horizVel = 0;
        MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
        MoveDirection *= Speed;
        MoveDirection = transform.TransformDirection(MoveDirection);
    }
    IEnumerator conditionChanger()
    {
         yield  return new WaitForSeconds(0.12f);
         enemyAnimator.SetInteger("EnemyCondition", -1); //  condition 0 is running
        // anim.SetBool("Death", false);
    }
}

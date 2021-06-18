using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;

public class PetScript : MonoBehaviour
{
    CharacterController controller;
    public float knightSearchRadius = 10.0f;
    Transform knight;
    NavMeshAgent agent;
    CharacterStats myStats;
    AnimController animController;
    Vector3 MoveDirection = Vector3.zero;
    public float horizVel = 0;
    public float VerticalVel = 0;
    public float ZVel = 0;
    float Speed = 4;
    public float stoppingDistance = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponentInParent<AnimController>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponentInParent<CharacterController>();
        controller.enabled = true;
        knight = AnimController.instance.MyHumanoid.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (animController.knightCameraIsActive == true)
        {
            float distance = Vector3.Distance(knight.transform.position, transform.position);
            if (distance <= knightSearchRadius)
            {
                agent.SetDestination(knight.position);
                // enemyAnimator.SetInteger("EnemyCondition", ENEMYWALK);
                if (distance <= agent.stoppingDistance)
                {
                    //CharacterStats targetStats = knight.GetComponent<CharacterStats>();
                    /*if (targetStats != null)
                    {
                        //combat.Attack(targetStats);
                    }*/

                    FaceTarget();
                    /* if (targetStats.currentHealth > 0)
                     {
                         enemyAnimator.SetInteger("EnemyCondition", ENEMYATTACK);
                         ZVel = 0;
                         VerticalVel = 0;
                         horizVel = 0;
                         StartCoroutine(idle());
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
                     }*/

                }
            }
        }
        void FaceTarget()
        {
            Vector3 direction = (knight.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, knightSearchRadius);
        }
    }
}

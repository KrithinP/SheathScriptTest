using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;


namespace RPG.Movement
{
    public class AnimMovement : MonoBehaviour , IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;

        public GameObject MyHumanoid;
        

        public GameObject respawnPointObject;
       // public Vector3 respawnPoint;

        Vector3 MoveDirection = Vector3.zero;

        CharacterController controller;
        RPG.Character.HumanoidScript hScript;
        //RPG.Control.AnimController animControl;
        Animator anim;

        CapsuleCollider humanoidCollider;
        [SerializeField] Transform target;

        // CharacterStats myStats;
        #region Keycodes
        public KeyCode Jump;
        public KeyCode Sprint;
        public KeyCode RunBack;
        public KeyCode TurnRight;
        public KeyCode TurnLeft;
        public KeyCode Roll;
        public KeyCode Skid;
        public KeyCode Death;
        public KeyCode ShieldWeaponUnsheath;
        public KeyCode ShieldBlockWalk;
        public KeyCode Attack;
        public KeyCode moveR;
        public KeyCode moveL;
        public KeyCode walkF;
        #endregion Keycodes
        #region ConstInts
        const int RUNNING = 0;
        const int WALKING = 1;
        const int JUMPING = 2;
        const int ROLLING = 3;
        const int SKID = 4;
        const int DEATH = 5;
        const int GETUP = 6;
        const int IDLE = 7;
        const int ShieldBlockWalking = 11;
        const int ATTACK = 12;
        const int SHIELDDAMAGE01 = 13;
        const int SHIELDDAMAGE02 = 14;
        const int BLENDTREEWALK = 15; 
        #endregion ConstInts
        public bool Sheath;
        public bool Damage;
        public bool tapToMove;

        public float humanoidRollHeight = 1.0f;
        public Vector3 rollCenter;
        public Vector3 standCenter;
        public float humanoidStandHeight = 2.0f;
        public float horizVel = 0;
        public float VerticalVel = 0;
        public float ZVel = 0;
        float Speed = 4;
        float gravity = 1f;

        public int rotateSpeed;

        public List<KeyCode> DoubleTaps = new List<KeyCode>();
        private List<KeyCode> UpKeys = new List<KeyCode>();
        private Dictionary<KeyCode, float> DicDoubleTapTimings = new Dictionary<KeyCode, float>();
        List<Segment> segmentTriggers = new List<Segment>();

        NavMeshAgent navMeshAgent;

        public UnityEngine.Animations.Rigging.Rig shieldBlock;

        public string controlLocked = "n";
        void Start()
        {
            rollCenter = new Vector3(0, 0.5f, 0);
            standCenter = new Vector3(0, 1.0f, 0);
            controller = GetComponentInChildren<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            hScript = MyHumanoid.GetComponent<RPG.Character.HumanoidScript>();
            humanoidCollider = GetComponentInChildren<CapsuleCollider>();
            Sheath = false;
            Damage = false;
            anim.SetBool("IsAlive", true);
            navMeshAgent = MyHumanoid.GetComponent<NavMeshAgent>();
         //   navMeshAgent.destination = target.position;
            controller.enabled = false;
            navMeshAgent.enabled = false;

           // respawnPoint = respawnPointObject.transform.position;
          //  MyHumanoid.transform.position = respawnPoint;

            tapToMove = false;
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
        }
        void ProcDoubleTap(KeyCode keyType)
        {
            if (!DicDoubleTapTimings.ContainsKey(keyType))
            {
                DicDoubleTapTimings.Add(keyType, 0f);
            }

            if (DicDoubleTapTimings[keyType] == 0f || UpKeys.Contains(keyType))
            {
                if (Time.time < DicDoubleTapTimings[keyType])
                {
                    if (!DoubleTaps.Contains(keyType))
                    {
                        DoubleTaps.Add(keyType);
                    }

                }

            }

            if (UpKeys.Contains(keyType))
            {
                UpKeys.Remove(keyType);
            }

            DicDoubleTapTimings[keyType] = Time.time + 0.38f;
        }

        void RemoveDoubleTap(KeyCode keyType)
        {
            if (DoubleTaps.Contains(keyType))
            {
                DoubleTaps.Remove(keyType);
            }
            if (!UpKeys.Contains(keyType))
            {
                UpKeys.Add(keyType);
            }
        }
      
        
        #region Movement
        void Movement()
        {


            MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
            MoveDirection *= Speed;
            //    print("right working");
            MoveDirection = transform.TransformDirection(MoveDirection);

            /*if(Input.GetMouseButtonDown(0))
            {
                MoveToCursor();
                anim.SetInteger("condition", BLENDTREEWALK);
                StartCoroutine(conditionChanger());
            }*/
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            /* Vector3 velocity = MyHumanoid.GetComponent<NavMeshAgent>().velocity;
             Vector3 localVelocity = transform.InverseTransformDirection(velocity);
             float speed = localVelocity.z;
             anim.SetFloat("fowardSpeed", speed);*/
            if (tapToMove == true)
            {
                controller.enabled = true;
                navMeshAgent.enabled = false;
                if (Input.GetKey(walkF))
                {

                    if (Input.GetKeyDown(walkF))
                    {

                        ProcDoubleTap(walkF);
                        if (DoubleTaps.Contains(walkF))
                        {
                            anim.SetInteger("condition", RUNNING); // condition 1 is for walk
                            Debug.LogWarning("Running");
                            ZVel = 1.5f;
                            MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                            MoveDirection *= Speed;
                            MoveDirection = transform.TransformDirection(MoveDirection);
                        }
                        else
                        {
                            anim.SetInteger("condition", WALKING); // condition 1 is for walk
                            ZVel = 0.5f;
                            MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                            MoveDirection *= Speed;
                            MoveDirection = transform.TransformDirection(MoveDirection);
                        }

                    }

                    //           if (anim.GetBool("jumping")==true)
                    //               return;

                }

                if (Input.GetKeyUp(walkF) || Input.GetKeyUp(ShieldBlockWalk))
                {
                    //anim.SetBool("running", true);
                    StartCoroutine(doubleTap());
                    RemoveDoubleTap(walkF);

                }
                if (anim.GetInteger("condition") == IDLE)
                {
                    ZVel = 0;
                    VerticalVel = 0;
                    horizVel = 0;
                    StartCoroutine(conditionChanger());
                    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                    MoveDirection *= Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);

                }



                if (Input.GetKey(TurnLeft))
                {
                    transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
                }

                if (Input.GetKey(TurnRight))
                {
                    transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                }

                if (Input.GetKeyDown(Jump))
                {
                    anim.SetBool("jumping", true);
                    anim.SetInteger("condition", JUMPING); //  condition 2 is for jumping
                    VerticalVel = 2.75f;
                    StartCoroutine(stopJump());
                    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel + 1);
                    MoveDirection *= Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                    Debug.LogWarning("<color = yellow> Jump is Successful !! </color>");
                }

                if (Input.GetKey(Roll))
                {
                    anim.SetBool("Roll", true);
                    anim.SetInteger("condition", ROLLING); // condition 1 is for walk
                    ZVel = 1.5f;
                    StartCoroutine(stopRoll());
                    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                    MoveDirection *= Speed;
                    Debug.LogWarning("Roll Roll Roll I Work");
                    MoveDirection = transform.TransformDirection(MoveDirection);
                    humanoidCollider.height = humanoidRollHeight;
                    humanoidCollider.center = rollCenter;
                    controller.height = humanoidRollHeight;
                    controller.center = rollCenter;
                }

                if (Input.GetKey(Skid))
                {
                    anim.SetBool("Skid", true);
                    anim.SetInteger("condition", SKID); // condition 1 is for walk
                    ZVel = 1.5f;
                    StartCoroutine(stopRoll());
                    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                    MoveDirection *= Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }

                if (Input.GetKeyDown(ShieldBlockWalk) && (Sheath))
                {
                    anim.SetInteger("condition", 11); // condition 1 is for walk
                    shieldBlock.weight = 0.1f;
                    ZVel = 0.5f;
                    horizVel = 0;
                    StartCoroutine(aBlockRig());
                    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                    MoveDirection *= Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }

                if (Input.GetKey(Attack) && (Sheath))
                {
                    anim.SetInteger("condition", ATTACK); // condition 1 is for walk
                    ZVel = 0;
                    VerticalVel = 0;
                    horizVel = 0;
                    //Vector3 fwd = knightRaycastPoint.TransformDirection(Vector3.forward);
                    RaycastHit hit;
                    // Debug.DrawRay(knightRaycastPoint.position, fwd * knightAttackRange, Color.yellow);
                    //Debug.DrawRay(knightRaycastPoint.position, fwd * knightAttackRange, Color.red);
                    Debug.LogWarning("In knight Tracking");
                    StartCoroutine(stopAttack());
                    StartCoroutine(conditionChanger());
                    //Ray ray = cam.SreenPointToRay(Input.mousePosition);
                    //RaycastHit hit;

                }

                MoveDirection.y -= gravity * Time.deltaTime;
                controller.Move(MoveDirection * Time.deltaTime);
            }
            else
            {
                navMeshAgent.enabled = true;
                controller.enabled = false;
                UpdateAnimator();
            }


            if (Input.GetKeyDown(ShieldWeaponUnsheath))
            {
                if (!Sheath)
                {
                    anim.SetLayerWeight(1, 1.0f);
                    anim.SetLayerWeight(2, 1.0f);
                    anim.SetBool("Unsheath", true);
                    anim.SetBool("Sheath", false);
                    Debug.LogWarning("<color = yellow> WeaponUnsheath is Successful !! </color>");
                    Sheath = true;
                    StartCoroutine(Unsheath());
                }
                else
                {
                    anim.SetLayerWeight(1, 1.0f);
                    anim.SetLayerWeight(2, 1.0f);
                    anim.SetBool("Sheath", true);
                    anim.SetBool("Unsheath", false);
                    Debug.LogWarning("<color = yellow> Weaponsheath is Successful !! </color>");
                    Sheath = false;
                    StartCoroutine(Unsheath());
                }
                MoveDirection = transform.TransformDirection(MoveDirection);

            }



            if (anim.GetBool("Damage"))
            {
                anim.SetLayerWeight(3, 1.0f);
                if (UnityEngine.Random.Range(0, 10) < 5)
                {
                    anim.SetInteger(("DamageInt"), 1);
                }
                else
                {
                    anim.SetInteger("DamageInt", 2);
                }
                StartCoroutine(damage());
            }
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = Mathf.Abs(localVelocity.z);
            anim.SetFloat("fowardSpeed", speed);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
            Debug.LogWarning(" navmesh is stopped ? ");
        }

        #endregion Movement
        #region CoRoutines
        IEnumerator stopSlide()
        {
            yield return new WaitForSeconds(0.6f);
            VerticalVel = 0;
            horizVel = 0;
            controlLocked = "n";
        }

        IEnumerator stopJump()
        {
            yield return new WaitForSeconds(0.5f);
            VerticalVel = 0;
            // anim.SetBool("running", true);
            anim.SetInteger("condition", 7); //  condition 0 is running
        }

        IEnumerator stopRoll()
        {
            yield return new WaitForSeconds(0.5f);
            VerticalVel = 0;
            //anim.SetBool("running", true);
            anim.SetInteger("condition", 7); //  condition 0 is running
            humanoidCollider.height = humanoidStandHeight;
            humanoidCollider.center = new Vector3(humanoidCollider.transform.position.x, standCenter.y, humanoidCollider.transform.position.z);
            humanoidCollider.center = standCenter;
            controller.height = humanoidStandHeight;
            //         controller.center = new Vector3(controller.transform.position.x,standCenter.y,controller.transform.position.z);
            controller.center = standCenter;
        }

        IEnumerator stopSkid()
        {
            yield return new WaitForSeconds(1.5f);
            VerticalVel = 0;
            // anim.SetBool("running", true);
            anim.SetInteger("condition", 7); //  condition 0 is running
        }
        IEnumerator stopShieldUnsheath()
        {
            yield return new WaitForSeconds(0.8f);
            VerticalVel = 0;
            anim.SetBool("ShieldWeaponUnsheath", true);
            anim.SetInteger("condition", 7); //  condition 0 is running
        }
        IEnumerator humanoidDeath()
        {
            yield return new WaitForSeconds(0.5f);
            //  Destroy (gameObject);
            // GM.zVelAdj= 0;   
            hScript.StartDissolve();
            yield return new WaitForSeconds(3.5f);
            //yield return new WaitForSeconds(4.2f);
            //  myStats.died = false;
            Respawn();
        }
        IEnumerator doubleTap()
        {
            yield return new WaitForSeconds(0.25f);
            if (!DoubleTaps.Contains(walkF))
            {
                anim.SetInteger("condition", IDLE); //  condition 0 is for run
                shieldBlock.weight = 0.0f;
                VerticalVel = 0;
                MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                MoveDirection *= Speed;
                MoveDirection = transform.TransformDirection(MoveDirection);
            }
        }

        IEnumerator getUp()
        {
            anim.SetBool("Death", false);
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(idle());
            StartCoroutine(conditionChanger());
            anim.SetInteger("condition", GETUP);
        }
        IEnumerator idle()
        {
            yield return new WaitForSeconds(1.8f);
            anim.SetInteger("condition", IDLE);
        }

        IEnumerator Unsheath()
        {
            yield return new WaitForSeconds(1.0f);
            anim.SetLayerWeight(1, 0.0f);
            anim.SetLayerWeight(2, 0.0f);
        }
        IEnumerator conditionChanger()
        {
            yield return new WaitForSeconds(0.12f);
            anim.SetInteger("condition", -1); //  condition 0 is running
                                              // anim.SetBool("Death", false);
        }
        IEnumerator stopAttack()
        {
            yield return new WaitForSeconds(0.35f);
            anim.SetInteger("condition", IDLE); //  condition 0 is running
        }
        IEnumerator aBlockRig()
        {
            yield return new WaitForSeconds(0.03f);
            shieldBlock.weight = 0.4f;
            yield return new WaitForSeconds(0.03f);
            shieldBlock.weight = 0.6f;
            yield return new WaitForSeconds(0.03f);
            shieldBlock.weight = 0.8f;
            yield return new WaitForSeconds(0.03f);
            shieldBlock.weight = 1.0f;
        }

        IEnumerator damage()
        {
            yield return new WaitForSeconds(1.0f);
            anim.SetLayerWeight(3, 0.0f);
            anim.SetInteger("DamageInt", 7);
            anim.SetBool("Damage", false);
        }
        void GetInput()
        {
            if (controller.isGrounded)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (anim.GetBool("jumping") == false)
                    {
                        anim.SetBool("jumping", true);
                        anim.SetInteger("condition", 2);
                    }
                    else
                    {
                        jumping();
                    }

                }
            }
        }
        #endregion CoRoutines
        void jumping()
        {
            anim.SetBool("jumping", true);
            anim.SetInteger("condition", 2);
            StartCoroutine(JumpRoutine());
            anim.SetInteger("condition", 0);
            anim.SetBool("jumping", false);
        }
        IEnumerator JumpRoutine()
        {
            yield return new WaitForSeconds(1);
        }
        public void Respawn()
        {
            Debug.LogWarning("respawn reached");
            // myStats.respawn();
            controller.enabled = false;
          //  MyHumanoid.transform.position = respawnPoint;
            controller.enabled = true;
            anim.SetInteger("condition", IDLE);
            Debug.LogWarning(MyHumanoid.transform.position + "uebwib");
            hScript.Undissolve();
        }
        [System.Serializable]
        struct moverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public object CaptureState()
        {
            moverSaveData data = new moverSaveData();
            data.position = new SerializableVector3(MyHumanoid.transform.position);
            data.rotation = new SerializableVector3(MyHumanoid.transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            moverSaveData data = (moverSaveData)state;
            GetComponentInChildren<NavMeshAgent>().enabled = false;
            //Debug.LogWarning("my humanoid pos" + MyHumanoid.transform.position);
           // print("In load State");
            MyHumanoid.transform.position = data.position.ToVector();
            MyHumanoid.transform.eulerAngles = data.rotation.ToVector();

            GetComponentInChildren<NavMeshAgent>().enabled = true;
           // Debug.LogWarning("my humanoid pos" + MyHumanoid.transform.position);
        }
    }
}

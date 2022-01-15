using System.Collections;
using UnityEngine;
using UnityEditor.Animations.Rigging;
using Cinemachine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class AnimController : MonoBehaviour
    {
        enum CursorType
        {
            None,
            Move,
            Combat,
            UI,

        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        #region variables

        public GameObject MyHumanoid;
        public GameObject MyHumanoidPrefab;
        public GameObject pet;

        RPG.Character.HumanoidScript hScript;
        CharacterController controller;
        AnimMovement animMovement;
       // Mover mover;
        Health health;

        public Transform knightRaycastPoint;
        public float knightAttackRange;
        //  CharacterStats myStats;
        // CharacterStats petStats;
        #region Keycodes

        public KeyCode characterCamSwitch;
        #endregion Keycodes
        
        public int DAMAGEINT;
        public int rotateSpeed;
        public int maxHealth;
        public bool isRespawning;
        public bool knightCameraIsActive;
        public bool switchingCameras;
        public GameObject respawnPointObject;
        public Vector3 respawnPoint;

        Animator anim;
        Animator rigAnim;

        //HumanoidScript hScript;

        public bool dissolveEffectCompleted = true;
        public UnityEngine.Animations.Rigging.Rig shieldBlock;
        public int laneNum = 2;
        public string controlLocked = "n";
        public Transform boomObj;
        Rect buttonSize;
        public GUIStyle customButton;
        public CinemachineFreeLook knightThirdPersonCam;
        public CinemachineFreeLook petThirdPersonCamera;

        Ray lastRay;
        #endregion variables
        #region Singleton
        public static AnimController instance;
        NavMeshAgent navMeshAgent;
       // public Interactables focus; // Our current focus: Item, Enemy etc.
      /* public HumanoidScript HumanoidScript
        {
            get => default;
            set
            {
            }
        }*/

        void Awake()
        {
            instance = this;
        }
        #endregion Singleton

        void Start()
        {
            hScript = MyHumanoid.GetComponent<RPG.Character.HumanoidScript>();
            // myStats = GetComponentInChildren<CharacterStats>();
            //myStats = MyHumanoid.GetComponent<CharacterStats>();
            //petStats = pet.GetComponent<CharacterStats>();
            //ShieldBlockRig = GetComponent<>
            //  rigAnim = hScript.rigController.GetComponent<Animator>();
            Debug.LogWarning(MyHumanoid.transform.position + "cwecwe");
            knightCameraIsActive = true;
            switchingCameras = false;
            //target.position = MyHumanoid.GetComponent<na>
            //hScript.shieldStatus = true;
            anim = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animMovement = GetComponent<AnimMovement>();
            health = GetComponentInChildren<Health>();
        }




        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("nothing to do");
            SetCursor(CursorType.None);
            /*           if (Input.GetKeyDown(characterCamSwitch))
                       {
                           if (knightCameraIsActive)
                           {
                               Debug.LogWarning("switchcamera to pet reached");
                               knightThirdPersonCam.Priority = 10;
                               petThirdPersonCamera.Priority = 11;
                               knightCameraIsActive = false;
                               Debug.LogWarning(knightCameraIsActive + " ");
                               //MyHumanoid.SetActive (false);
                               switchingCameras = true;
                               hScript.StartDissolve();
                               //pet.SetActive(true);

                           }
                           else
                           {
                               knightThirdPersonCam.Priority = 11;
                               petThirdPersonCamera.Priority = 10;
                               knightCameraIsActive = true;
                               MyHumanoid.SetActive(true);
                               switchingCameras = true;
                               hScript.Undissolve();
                               //pet.SetActive(false);
                           }
                           //if(navmesh agent from other code required)
                           if( navMeshAgent.enabled == true)
                           {

                           }

                       }

                       */

            /*  void MoveTo(Vector3 destination)
              {
                  MyHumanoid.GetComponent<NavMeshAgent>().destination = destination;
              }*/



            /*if (Physics.Raycast(knightRaycastPoint.position, fwd, out hit, knightAttackRange))
            {
                Debug.LogWarning("Knight found by " + hit.collider.name);
                //Interactables interactables =   hit.collider.GetComponentInParent<Enemy>();
                CharacterCombat knightCombat = MyHumanoid.GetComponent<CharacterCombat>();
                CharacterStats enemyStats = hit.collider.GetComponent<CharacterStats>();
                if (knightCombat != null)
                {
                    Debug.LogWarning("player combat attack reached");
                    knightCombat.Attack(enemyStats);
                }
                /*if (interactables != null)
                {
                     Debug.LogWarning("Interactables not null");
                    SetFocus(interactables);
                }*/

            /*    MoveDirection = new Vector3(horizVel, VerticalVel, ZVel);
                MoveDirection *= Speed;
                MoveDirection = transform.TransformDirection(MoveDirection);

            }*/

            // GetInput ();
            // knightHealth = hScript.knightHealth;
            /* if (Input.GetMouseButtonDown(1))
             {
                 // We create a ray

                 Vector3 fwd = knightRaycastPoint.TransformDirection(Vector3.forward);
                 RaycastHit hit;
                 Debug.DrawRay(knightRaycastPoint.position, fwd * knightAttackRange, Color.yellow);
                 Debug.DrawRay(knightRaycastPoint.position, fwd * knightAttackRange, Color.red);
                 Debug.LogWarning("In knight Tracking");
                 //Ray ray = cam.SreenPointToRay(Input.mousePosition);
                 //RaycastHit hit;

                 // If the ray hits
                 if (Physics.Raycast(knightRaycastPoint.position, fwd, out hit, knightAttackRange))
                 {
                     Debug.LogWarning("Knight found by " + hit.collider.name);
                     //Interactables interactables =   hit.collider.GetComponentInParent<Enemy>();
                     CharacterCombat knightCombat = MyHumanoid.GetComponent<CharacterCombat>();
                     CharacterStats enemyStats = hit.collider.GetComponent<CharacterStats>();
                     if (knightCombat != null)
                     {
                         Debug.LogWarning("player combat attack reached");
                         knightCombat.Attack(enemyStats);
                     }
                     /*if (interactables != null)
                     {
                          Debug.LogWarning("Interactables not null");
                         SetFocus(interactables);
                     }
                 }
             }
         }

         void SetFocus(Interactables newFocus)
         {
             // If our focus has changed
             if (newFocus != focus)
             {
                 // Defocus the old one
                 if (focus != null)
                     focus.OnDefocused();

                 focus = newFocus;   // Set our new focus
                                     //motor.FollowTarget(newFocus);	// Follow the new focus
             }

             newFocus.OnFocused(transform);
         }

         // Remove our current focus
         void RemoveFocus()
         {
             if (focus != null)
                 focus.OnDefocused();

             focus = null;
             //motor.StopFollowingTarget();
         }*/
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            Debug.LogWarning("raycasted");
            foreach (RaycastHit hit in hits)
            {
                Debug.LogWarning("raycasted foreach");
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponentInChildren<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                Debug.LogWarning("target not null");
                if (Input.GetMouseButton(0))
                {
                    MyHumanoid.GetComponent<Fighter>().Attack(target.gameObject);
                    Debug.LogWarning("target found");
                }
                SetCursor(CursorType.Combat);
                return true;
            }
            // GetComponent<Fighter>().Attack(null);
            Debug.LogWarning("target not found");
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private  CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping  in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;
     
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    animMovement.StartMoveAction(hit.point, 1f);

                    Debug.LogWarning("Hit Point :"+hit.point);
                }
                SetCursor(CursorType.Move);
                return true;
                //controller.enabled = false;
            }
            return false;
        }

        Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        IEnumerator conditionChanger()
        {
            yield return new WaitForSeconds(0.12f);
            anim.SetInteger("condition", -1); //  condition 0 is running
                                              // anim.SetBool("Death", false);
        }
    }



}
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour , IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] Weapon defaultWeapon = null;

        Weapon currentWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        NavMeshAgent navMeshAgent;
        AnimMovement animMovement;
        Animator anim; 
        Mover mover;
        public bool playerIsSelected;
        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            if(this.gameObject.tag == "Player")
            {
                playerIsSelected = true;
                animMovement = GetComponentInParent<AnimMovement>();
            }
            else
            {
                playerIsSelected = false;
                mover = GetComponent<Mover>();
            }
            anim = GetComponent<Animator>();

            EquipWeapon(defaultWeapon);

        }

        // Update is called once per frame
        private void Update()
        {
            // if (target == null) return;
            // Debug.LogWarning("Target " + target.name);
            timeSinceLastAttack += Time.deltaTime;

            if (target != null)
            {
                if (target == null) return;
                if (target.IsDead()) return;

                if (!GetIsInRange())
                {
                    if (playerIsSelected)
                    {
                        animMovement.MoveTo(target.transform.position, 1f);
                    }
                    else
                    {
                        if (mover == null) return;
                        mover.MoveTo(target.transform.position, 1f);
                    }

                   // Debug.LogWarning("destination" + navMeshAgent.destination);
                    //Debug.LogWarning("Distance:" + Vector3.Distance(transform.position, target.transform.position));
                    //target = null;
                }
                else
                {
                    if (playerIsSelected)
                    {
                        animMovement.Cancel();
                    }
                    else
                    {
                        if (mover == null) return;
                        mover.Cancel();
                    }
                    AttackBehavior();
                }
            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
           // Instantiate(weaponPrefab, handTransform);
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform,leftHandTransform, anim);
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAtttck();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAtttck()
        {
            anim.ResetTrigger("StopAttack");
            anim.SetTrigger("Attack");
        }

        void Hit()
        {
            if(target == null)
            {
                return;
            }

            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, currentWeapon.GetDamage());
            }

            Debug.LogWarning("Target" + target);
           // Health healthComponent = target.GetComponent<Health>();
            target.TakeDamage(gameObject, currentWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.GetRange();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToTeat = combatTarget.GetComponent<Health>();
            return targetToTeat != null && !targetToTeat.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponentInParent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();

        }
        public void Cancel()
        {
            StopAttack();
            Debug.LogWarning("In Fighter Cancel");
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("StopAttack");
        }
        //animation event
        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }


}

using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Core;
using RPG.Resources;
using RPG.Stats;
using RPG.Saving;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour , IAction, ISaveable , IModifierProvider    
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] Weapon defaultWeapon = null;

        LazyValue<Weapon> currentWeapon ;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        NavMeshAgent navMeshAgent;
        AnimMovement animMovement;
        Animator anim; 
        Mover mover;
        public bool playerIsSelected;

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(InitialWeapon);
        }
        void Start()
        {
            currentWeapon.ForceInit();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            if(gameObject.tag == "Player")
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

            if(currentWeapon == null) EquipWeapon(defaultWeapon);

        }
        private Weapon InitialWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon ;
        }

        private void Update()
        {
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
            AttachWeapon(weapon);
            // Instantiate(weaponPrefab, handTransform);
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, anim);
        }

        private void AttachWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
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
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stats.Stat.Damage);
            print("Damage " + damage);

            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject,damage);
            }
            else
            {
                
                target.TakeDamage(gameObject, damage);
            }

            Debug.LogWarning("Target" + target);
        }

        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.value.GetRange();
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
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stats.Stat stat)
        {
            if(stat == Stats.Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stat stat)
        {
            if (stat == Stats.Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float healthPoints = 100f;
        bool isDead = false;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0); //mathf max in this if health is less than the damge dealt it will round health to 0, so max of the 2 values given
            Debug.LogWarning("Health" + healthPoints);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GetExperience(GetComponent<BaseStats>().GetExperience());
        }

        public float GetPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetHealth())*100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            if (this.gameObject.tag == "Player")
            {
                GetComponentInParent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        public object CaptureState()
        {
            return healthPoints;
           // Debug.Log("health" + healthPoints);
        }

        public void RestoreState(object state)
        {
            print("health at restore");
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
            if (healthPoints > 0 && isDead)
            {
                GetComponent<Animator>().SetTrigger("Revive");
              //  GetComponent<Animator>().SetTrigger("")
                isDead = false;

            }
        }
    }
}
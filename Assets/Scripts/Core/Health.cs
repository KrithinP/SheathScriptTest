﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float healthPoints = 100f;
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0); //mathf max in this if health is less than the damge dealt it will round health to 0, so max of the 2 values given
            Debug.LogWarning("Health" + healthPoints);
            if (healthPoints == 0)
            {
                Die();
            }
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
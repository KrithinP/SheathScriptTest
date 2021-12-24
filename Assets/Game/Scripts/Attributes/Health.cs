using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPoints;

        [SerializeField] float RegenerationHealthPercentage = 70f;

        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stat.Health);
        }
        private void Start()
        {
           // GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
           // if (healthPoints < 0)
           // {
          //      healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stat.Health);
          //  }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if(healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stat.Health);
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GetExperience(GetComponent<BaseStats>().GetStat(Stats.Stat.ExperienceReward));
        }


        public float GetPercentage()
        {
            Debug.LogWarning("Get health" + GetComponent<BaseStats>().GetStat(Stats.Stat.Health) + "health points" + healthPoints);
            return (healthPoints.value / GetComponent<BaseStats>().GetStat(Stats.Stat.Health)) *100;

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
         private void RegenerateHealth()
         {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stats.Stat.Health) * (RegenerationHealthPercentage/100.0f);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints); 
        }

        public object CaptureState()
        {
            return healthPoints;
           // Debug.Log("health" + healthPoints);
        }

        public void RestoreState(object state)
        {
            print("health at restore");
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Die();
            }
            if (healthPoints.value > 0 && isDead)
            {
                GetComponent<Animator>().SetTrigger("Revive");
              //  GetComponent<Animator>().SetTrigger("")
                isDead = false;

            }
        }
    }
}
using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints = 0.0f;

        //public delegate void ExperienceGainedDelegate();
        public event Action OnExperienceGained;

        public float GetPoints()
        {
            return experiencePoints;
            
        }

        public void GetExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }



        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }

}

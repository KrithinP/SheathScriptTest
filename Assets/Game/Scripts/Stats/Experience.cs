using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints = 0.0f;

        public float GetPoints()
        {
            return experiencePoints;
        }

        public void GetExperience(float experience)
        {
            experiencePoints += experience;
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

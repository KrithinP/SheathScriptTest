using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoits = 0.0f;

        public void GetExperience(float experience)
        {
            experiencePoits += experience;
        }
    }

}

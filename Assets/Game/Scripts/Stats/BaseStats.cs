using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

        public void Start()
        {
            currentLevel = GetLevel();
        }

        private void Update()
        {
            
        }
        public float GetStat(Stat stat)
        {
              //  Debug.Log($"Checking {name}'s health class = {characterClass} level = {startingLevel}");
             return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(experience == null)
            {
                return startingLevel;
            }
            float curentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUP = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if(curentXP < XPToLevelUP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

    }
}

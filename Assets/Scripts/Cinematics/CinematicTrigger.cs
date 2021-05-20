using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponent<PlayableDirector>().Play(); 
        }
    }
}

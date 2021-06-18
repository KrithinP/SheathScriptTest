using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactables
{

    AnimController animController;
    CharacterStats myStats;

    public AnimController AnimController
    {
        get => default;
        set
        {
        }
    }

    public CharacterStats CharacterStats
    {
        get => default;
        set
        {
        }
    }

    void Start()
    {
        animController = AnimController.instance;
        myStats = GetComponent<CharacterStats>();
        
    }
    public override void Interact()
    {
        Debug.LogWarning("interact reached");
       base.Interact();
       CharacterCombat playerCombat = animController.MyHumanoid.GetComponent<CharacterCombat>();
        if(playerCombat != null)
        {
            Debug.LogWarning("player combat attack reached");
            //if(myStats.currentHealth)
            playerCombat.Attack(myStats);

        }
    }

}

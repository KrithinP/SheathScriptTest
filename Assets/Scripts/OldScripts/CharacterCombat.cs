using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
	public float attackSpeed = 1.5f;
	private float attackCooldown = 0f;

	public float attackDelay = .6f;

	public event System.Action OnAttack;


	CharacterStats myStats;

	void Start()
	{
		myStats = GetComponent<CharacterStats>();
	}

	void Update()
	{
		attackCooldown -= Time.deltaTime;
	}

	public void Attack(CharacterStats targetStats)
	{
		if (attackCooldown <= 0f)
		{
			Debug.LogWarning("attack function reached");
			if(targetStats.currentHealth>0)
			{
							StartCoroutine(DoDamage(targetStats, attackDelay));

			if (OnAttack != null)
				OnAttack();
			targetStats.TakeDamage(myStats.damage.GetValue());
			attackCooldown = 1f / attackSpeed;
			}
			

		}

	}

	IEnumerator DoDamage(CharacterStats stats, float delay)
	{
		yield return new WaitForSeconds(delay);

		stats.TakeDamage(myStats.damage.GetValue());
	}

    public CharacterStats CharacterStats
    {
        get => default;
        set
        {
        }
    }
}
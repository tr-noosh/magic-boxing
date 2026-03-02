using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "ScriptableObjects/OpponentAttack", order = 1)]
public class EnemyMove : ScriptableObject
{
	public string triggerName;

	public int maxHits = 4; // maximum amount of hits you can get in while enemy is stunned when you dodge them and counterhit
	public float maxTime = 5.0f; // maximum amount of time the enemy will be stunned for if you dont hit them instead
	
	public int[] phaseWeights = { 0, 0, 0 };
}

public static class RandomMove
{
	public static EnemyMove SelectMove(EnemyMove[] moves, int phase)
	{
		int total = moves.Sum(move => move.phaseWeights[phase]);
		int randNum = Random.Range(0, total-1);

		foreach (EnemyMove move in moves) {
			if (randNum < move.phaseWeights[phase]) {
				return move;
			}
			randNum -= move.phaseWeights[phase];
		}
		return default; 
	}
}
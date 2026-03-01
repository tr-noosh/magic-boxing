using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType : int
{
	NONE = 0,
	LOW = 1,
	HIGH = 2,
	ALL = 3,
}

public class OpponentController : MonoBehaviour
{
	public PlayerController player;

	public EnemyMove[] moveList;

	[Header("Hitting Zones")]
	public bool hitCenter = false;
	public bool hitLow = false;
	public bool hitLeft = false;
	public bool hitRight = false;

	[Header("Opponent Position")]
	public bool center = true;
	public bool left = false;
	public bool right = false;

	public bool low = true;
	public bool high = true;

	public BlockType blocking = BlockType.NONE;

	public void damage(bool highPunch, bool rightPunch) {} // opponent taking damage. interrupt attacks and play animations

	void debug() { // update debug UI
		// something with boxes on the canvas
	}

	void Update() {
		// randomization


		debug();
	}
}

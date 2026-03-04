using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum BlockType : int
{
	NONE = 0,
	LOW = 1,
	HIGH = 2,
	ALL = 3,
}

[RequireComponent(typeof(SpriteRenderer))]
public class OpponentController : MonoBehaviour
{
	public PlayerController player;

	private SpriteRenderer spr;
	private Animator ani;

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

	public int hitsRemaining = 4;
	public float stunTime = 10.0f;

	public bool stunned = false;
	public bool finalHit = false;
	public bool success = false;

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
	}

	public void damage(bool highPunch, bool rightPunch) {	// opponent taking damage. interrupt attacks and play animations
		blocking = BlockType.NONE;
		hitsRemaining--;
		if (hitsRemaining > 0 && stunTime > 0.0f) {
			stunned = true;
			Debug.Log("gorp");
		}
		if (hitsRemaining == 1) { finalHit = true; }
		if (hitsRemaining <= 0) { stunTime = 0.0f; }
		ani.SetBool("stunned", stunned);
		ani.SetBool("final", finalHit);
		ani.SetTrigger("ouch" + (rightPunch ? "Right" : "Left") + (highPunch ? "High" : "Low"));
	} 
	public void block(bool highPunch, bool rightPunch) {}

	private void checkHitting() {
		if (player.center && hitCenter) {
			success = true;
			player.damaged("center");
		}
		else if (player.low && hitLow) {
			success = true;
			player.damaged("low");
		}
		else if (player.left && hitLeft) {
			success = true;
			player.damaged("left");
		}
		else if (player.right && hitRight) {
			success = true;
			player.damaged("right");
		}
		ani.SetBool("success", success);
	}

	public void chooseMove() {
		success = false;
		ani.SetBool("success", success);
		finalHit = false;
		ani.SetBool("final", finalHit);
		int phase = 0;

		EnemyMove move = RandomMove.SelectMove(moveList, phase);

		hitsRemaining = move.maxHits;
		stunTime = move.maxTime;
		if (move.triggerName != "") ani.SetTrigger(move.triggerName);
		
	}
	
	void Update() {
		if (stunned) {
			if (stunTime > 0.0f) { stunTime -= Time.deltaTime; } 
			else {
				stunned = false;
				ani.SetBool("stunned", stunned);
				Debug.Log("prog");
			}
		}

		checkHitting();
	}


	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color blockColor = new(.8f, .8f, .3f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f); Vector3 flatWide = new(.8f, .12f, 0.01f);
	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .25f, 0);

		// Center
		Gizmos.color = center && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above, flat);
		Gizmos.color = center && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f, flat);

		// Left
		Gizmos.color = left && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.right*.25f, flat);
		Gizmos.color = left && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f - transform.right*.25f, flat);

		// Right
		Gizmos.color = right && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above + transform.right*.25f, flat);
		Gizmos.color = right && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f + transform.right*.25f, flat);

		// Blocking
		Gizmos.color = blockColor;
		if (blocking == BlockType.LOW || blocking == BlockType.ALL) {
			Gizmos.DrawCube(above - transform.up*.25f - transform.forward*.01f, flatWide);
		}
		if (blocking == BlockType.HIGH || blocking == BlockType.ALL) {
			Gizmos.DrawCube(above - transform.forward*.01f, flatWide);
		}
	}
}

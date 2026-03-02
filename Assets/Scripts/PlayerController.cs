using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
	public OpponentController opponent;

	private SpriteRenderer spr;
	private Animator ani;

	[Header("Player Position")]
	public bool center = true;
	public bool low = true;
	public bool left = false;
	public bool right = false;
	
	[Header("Player State")]
	public bool actionable = true; // can begin an action or interrupt currently performing action

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
	}

	void miss() { }
	void blocked() { } 

	public void hit(string punch) { // Called by the animation played by beginPunch()
		bool highPunch = false;
		bool rightPunch = false;

		switch(punch) {
			case "LEFTJAB":
				highPunch = true; 
				break;
			case "RIGHTJAB":
				rightPunch = true;
				highPunch = true;
				break;
			case "LEFTHOOK":
				break;
			case "RIGHTHOOK":
				rightPunch = true;
				break;
		}

		if (highPunch) { // JAB
			if (!opponent.high) { miss(); }
			else if (opponent.blocking == BlockType.HIGH || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch);
			}
			else { miss(); }
		}
		else { // HOOK
			if (!opponent.low) { miss(); }
			else if (opponent.blocking == BlockType.LOW || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch);
			}
			else { miss(); }
		}
	}

	private void startPunch(bool right) {
		bool jab = Input.GetKey(KeyCode.UpArrow);
		ani.SetTrigger(
			(right ? "right" : "left") + (jab ? "Jab" : "Hook")
		);
	}

	void Update() {

		if (!actionable) return;

		if (Input.GetKey(KeyCode.LeftArrow)) {
			ani.SetTrigger("dodgeLeft");
		} 
		else if (Input.GetKey(KeyCode.RightArrow)) {
			ani.SetTrigger("dodgeRight");
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			ani.SetTrigger("dodgeDown");
		}
		else if (Input.GetKey(KeyCode.Z)) {
			startPunch(false);
		}
		else if (Input.GetKey(KeyCode.X)) {
			startPunch(true);
		}
	}

	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color hurtColor = new(.93f, .25f, .25f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f);
	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .25f, 0);

		// Center
		Gizmos.color = center ? activeColor : inactiveColor;
		Gizmos.DrawCube(above, flat);
		if (opponent.hitCenter) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.forward*.01f, flat*0.6f);
		}

		// Low
		Gizmos.color = low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f, flat);
		if (opponent.hitLow) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.up*.25f - transform.forward*.01f, flat*0.6f);
		}

		// Left
		Gizmos.color = left ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.right*.25f, flat);
		if (opponent.hitLeft) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.right*.25f - transform.forward*.01f, flat*0.6f);
		}

		// Right
		Gizmos.color = right ? activeColor : inactiveColor;
		Gizmos.DrawCube(above + transform.right*.25f, flat);
		if (opponent.hitRight) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above + transform.right*.25f - transform.forward*.01f, flat*0.6f);
		}
	}
}

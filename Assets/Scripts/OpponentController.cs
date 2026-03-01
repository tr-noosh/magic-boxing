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

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
	}

	public void damage(bool highPunch, bool rightPunch) {} // opponent taking damage. interrupt attacks and play animations


	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color blockColor = new(.8f, .8f, .3f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f); Vector3 flatWide = new(.8f, .12f, 0.01f);
	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
			
		
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .5f, 0);

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

	void Update() {
		// randomization


	}
}

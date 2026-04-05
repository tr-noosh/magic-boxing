using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
	public OpponentController opponent;
	public StrikeSender sender;

	public Slider healthBar;

	public SpriteRenderer spr;
	public Animator ani;

	public TextMeshProUGUI scoreText;

	[Header("Player Position")]
	public bool center = true;
	public bool low = true;
	public bool left = false;
	public bool right = false;
	
	[Header("Player State")]
	public bool actionable = true; // can begin an action or interrupt currently performing action
	public bool invincible = false; // temporarily immune to further damage
	public int knockouts = 0;
	public int roundKOs = 0;
	public bool knockedOut = false;
	public float koTimer = 0.0f;
	public float getupProgress = 0.0f;

	[Header("Stats")]
	public float maxHealth = 100.0f;
	public float health = 100.0f;
	public float damageStat = 5.0f;

	keymap keymap;

	void Awake() {
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
		keymap = MenuController.currentKeymap;
	}
	void Start() {
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
		keymap = MenuController.currentKeymap;
	}

	void miss() { }
	void blocked() { } 
	public void damaged(string zone, float damage) {
		resetAllTriggers();
		health -= damage;
		healthBar.value = health / maxHealth;
		ani.SetTrigger("stun");
		sender.dshake();
	} 

	public void hit(string punch) { // Called by the animation played by beginPunch()
		if (!actionable) return;
		bool highPunch = false;
		bool rightPunch = false;

		switch(punch) {
			case "LEFTHOOK": break;
			case "RIGHTHOOK": rightPunch = true; break;
			case "LEFTJAB": highPunch = true; break;
			case "RIGHTJAB": rightPunch = true; highPunch = true; break;
		}

		if (highPunch) { // JAB
			if (!opponent.high) { miss(); }
			else if (opponent.blocking == BlockType.HIGH || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch, damageStat); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch, damageStat);
			}
			else { miss(); }
		}
		else { // HOOK
			if (!opponent.low) { miss(); }
			else if (opponent.blocking == BlockType.LOW || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch, damageStat); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch, damageStat);
			}
			else { miss(); }
		}
	}

	void resetAllTriggers(bool stun = false) {
		ani.ResetTrigger("dodgeLeft");
		ani.ResetTrigger("dodgeRight");
		ani.ResetTrigger("dodgeDown");
		ani.ResetTrigger("leftHook");
		ani.ResetTrigger("rightHook");
		ani.ResetTrigger("leftJab");
		ani.ResetTrigger("rightJab");
		if (stun) ani.ResetTrigger("stun");
	}

	private void startPunch(bool right, bool jab) {
		resetAllTriggers();
		ani.SetTrigger(
			(right ? "right" : "left") + (jab ? "Jab" : "Hook")
		);
	}

	void Update() {
		updateScoreText();

		if (!actionable) return;

		if (Input.GetKey(keymap.left_dodge)) {
			ani.SetTrigger("dodgeLeft");
		} 
		else if (Input.GetKey(keymap.right_dodge)) {
			ani.SetTrigger("dodgeRight");
		}
		else if (Input.GetKey(keymap.low_dodge)) {
			ani.SetTrigger("dodgeDown");
		}
		else if (Input.GetKey(keymap.left_punch)) {
			startPunch(false, false);
		}
		else if (Input.GetKey(keymap.right_punch)) {
			startPunch(true, false);
		}
		else if (Input.GetKey(keymap.left_jab)) {
			startPunch(false, true);
		}
		else if (Input.GetKey(keymap.right_jab)) {
			startPunch(true, true);
		}
	}

	void updateScoreText() {
		string playerTxt = ( roundKOs == 2 ? "<color=\"red\">2</color>" : roundKOs.ToString());
		string opponentTxt = ( opponent.roundKOs == 2 ? "<color=\"red\">2</color>" : opponent.roundKOs.ToString());
		scoreText.text = playerTxt + "-" + opponentTxt;
	}

	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color hurtColor = new(.93f, .25f, .25f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f);
	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .25f, 0);

		// Actionability indicator
        if (!actionable)
        {
            Gizmos.color = new(0, 0, 0, .7f);
            Gizmos.DrawCube(above - transform.up * .125f, flat * 4f);
        }

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

		// Invincibility indicator
        if (invincible)
		{
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(above - transform.up * .125f, flat * 3f);
        }
	}
}

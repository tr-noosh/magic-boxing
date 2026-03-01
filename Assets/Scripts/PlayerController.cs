using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	public OpponentController opponent;

	[Header("Player Position")]
	public bool center = true;
	public bool low = true;
	public bool left = false;
	public bool right = false;
	
	[Header("Player State")]
	public bool actionable = true; // can begin an action or interrupt currently performing action

	void beginPunch() {	// play punch animation

	}

	void beginDodge() { // play dodge animation

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
			else if (opponent.blocking == BlockType.HIGH || opponent.blocking == BlockType.ALL) { blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch);
			}
			else { miss(); }
		}
		else { // HOOK
			if (!opponent.low) { miss(); }
			else if (opponent.blocking == BlockType.LOW || opponent.blocking == BlockType.ALL) { blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch);
			}
			else { miss(); }
		}
	}

	void debug() { // update debug UI
		// something with boxes on the canvas
	}

	void Update() {
		// take inputs

		debug();
	}
}

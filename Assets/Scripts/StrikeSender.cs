
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StrikeSender : MonoBehaviour
{
	public PlayerController player;
	public OpponentController opponent;

	public ParticleSystem powsL, powsR;

	
	// seems to manage opponent hit particle effect, move to opponent

	public float shakeLength = 0.5f; // default shake duration
	public float shakeSize = 0.3f;
	public float shakeSpeed = 1.0f;

	Vector3 iPos = new Vector3(0f, 2f, -4.5f);
	float shakeTime = 0.0f;

	Camera cam;

	void Awake()
	{
		cam = Camera.main;
		iPos = cam.transform.position;
	}

	void Update()
	{
		if (shakeTime > 0.0f) {
			cam.transform.localPosition = iPos + Random.insideUnitSphere * shakeSize;
			shakeTime -= Time.deltaTime * shakeSpeed;
		}
		else {
			shakeTime = 0.0f;
			cam.transform.localPosition = iPos;
		}
	}

	public void move(int lr) {
		if (lr == 0) {
			transform.position = new Vector3(-4.52f, -1.4f, 0.52f);
		}
		else {
			transform.position = new Vector3(4.52f, -1.4f, 0.52f);
		}
	}

	public void shake() { // unused
		shakeLength = 0.3f;
		shakeSize = 0.5f;
		shakeTime = shakeLength;
	}

	public void dshake() {
		shakeLength = 0.1f;
		shakeSize = 0.2f;
		shakeTime = shakeLength;
	}

	//public void reset_Damage() { player.ani.ResetTrigger("stun"); }
	public void pow(bool rightPunch){
		if (rightPunch) {
			powsR.Play();
		}
		else {
			powsL.Play();
		}
	}
}

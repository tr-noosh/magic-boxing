using Unity.VisualScripting;
using UnityEngine;

public class ghost : MonoBehaviour
{
	public float spawnRate = 0.05f;
	public float ghostLifetime = 0.5f;
	public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);

	public PlayerController player;
	public OpponentController opponent;

	private float timer;
	private SpriteRenderer sr;

	public float ghostSize = 0.2f;

	public bool hitting = false;
	public bool left, right, low = false;

	public bool ghosti = true;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		left = false;
		right = false;
		low = false;
		timer = 0.0f;
	}

	private void checkHitting() {
		if (player.invincible) { return; }
		if (player.knockedOut) { return; }
		if (player.health <= 0.0f) { return; }
		if (player.center && (left || right))
		{
			player.damaged("center", opponent.currentMoveDamage);
		}
		else if (player.low && low)
		{
			player.damaged("low", opponent.currentMoveDamage);
		}
		else if (player.left && left)
		{
			player.damaged("left", opponent.currentMoveDamage);
		}
		else if (player.right && right)
		{
			player.damaged("right", opponent.currentMoveDamage);
		}
	}

	void Update()
	{
		if (ghosti) {
			timer += Time.deltaTime;

			if (timer >= spawnRate) {
				SpawnGhost();
				timer = 0f;
			}
		}
		if (hitting) {
			Debug.Log(gameObject.name);
			checkHitting();
		}
	}

	void SpawnGhost()
	{
		GameObject ghost = new GameObject("ghost_behind");
		ghost.transform.position = transform.position;
		ghost.transform.rotation = transform.rotation;
		ghost.transform.localScale = new Vector3(ghostSize, ghostSize, ghostSize);

		SpriteRenderer ghostSR = ghost.AddComponent<SpriteRenderer>();
		ghostSR.sprite = sr.sprite;
		ghostSR.flipX = sr.flipX;
		ghostSR.flipY = sr.flipY;
		ghostSR.sortingLayerID = sr.sortingLayerID;
		ghostSR.sortingOrder = sr.sortingOrder - 1;
		ghostSR.color = ghostColor;

		StartCoroutine(FadeAndDestroy(ghostSR));
	}

	System.Collections.IEnumerator FadeAndDestroy(SpriteRenderer ghostSR)
	{
		float elapsed = 0f;
		Color startColor = ghostSR.color;

		while (elapsed < ghostLifetime)
		{
			elapsed += Time.deltaTime;
			float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / ghostLifetime);
			ghostSR.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
			yield return null;
		}

		Destroy(ghostSR.gameObject);
	}
}
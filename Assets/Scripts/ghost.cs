using UnityEngine;

public class ghost : MonoBehaviour
{
    public float spawnRate = 0.05f;
    public float ghostLifetime = 0.5f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);

    private float timer;
    private SpriteRenderer sr;

    public bool ghosti = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (ghosti == true)
        {
            timer += Time.deltaTime;

            if (timer >= spawnRate)
            {
                SpawnGhost();
                timer = 0f;
            }
        }
    }

    void SpawnGhost()
    {
        GameObject ghost = new GameObject("ghost_behind");
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

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
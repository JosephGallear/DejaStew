using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [SerializeField] private float maxDifficultyTime = 600f;
    [SerializeField] private float startSpawnRate = 60f;
    [SerializeField] private float minSpawnRate = 5f;

    private float gameTimer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            gameTimer += Time.deltaTime;
        }
    }

    public float GetCurrentSpawnRate()
    {
        float t = Mathf.Clamp01(gameTimer / maxDifficultyTime);
        return Mathf.Lerp(startSpawnRate, minSpawnRate, t);
    }
}

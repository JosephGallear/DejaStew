using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private Sprite lifeOnSprite;
    [SerializeField] private Sprite lifeOffSprite;

    private void Start()
    {
        GameManager.Instance.OnLivesChanged += GameManager_OnLivesChanged;
        UpdateVisual();
    }

    private void GameManager_OnLivesChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        int currentLives = GameManager.Instance.GetLives();

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].sprite = i < currentLives ? lifeOnSprite : lifeOffSprite;
        }
    }
}

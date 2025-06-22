using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        if (hasProgress == null && hasProgressGameObject != null)
        {
            SetHasProgress(hasProgressGameObject.GetComponent<IHasProgress>());
        }

        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }

    }

    private void OnDestroy()
    {
        if (hasProgress != null)
        {
            hasProgress.OnProgressChanged -= HasProgress_OnProgressChanged;
        }
    }

    public void SetHasProgress(IHasProgress hasProgress)
    {
        if (this.hasProgress != null)
        {
            this.hasProgress.OnProgressChanged -= HasProgress_OnProgressChanged;
        }

        this.hasProgress = hasProgress;
        this.hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

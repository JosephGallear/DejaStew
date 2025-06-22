using UnityEngine;

public class FlashingBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    [SerializeField] private float warningShowProgressAmount = 0.25f;

    [SerializeField] private bool inverted;

    private const string IS_FLASHING = "IsFlashing";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (hasProgress == null && hasProgressGameObject != null)
        {
            SetHasProgress(hasProgressGameObject.GetComponent<IHasProgress>());
        }

        animator.SetBool(IS_FLASHING, false);
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        if (inverted)
        {
            animator.SetBool(IS_FLASHING, e.progressNormalized < warningShowProgressAmount);
        }
        else
        {
            animator.SetBool(IS_FLASHING, e.progressNormalized >= warningShowProgressAmount);
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
}

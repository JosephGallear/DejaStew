using UnityEngine;

public class OrderFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    private Animator animator;

    [SerializeField] private float warningShowProgressAmount = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnProgressChanged += DeliveryManager_OnProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    }

    private void DeliveryManager_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = DeliveryManager.Instance.IsOrderListFull() && e.progressNormalized >= warningShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}

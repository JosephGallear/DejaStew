using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyUseText;
    [SerializeField] private TextMeshProUGUI keyGamepadUseText;
    [SerializeField] private TextMeshProUGUI keyRecordText;
    [SerializeField] private TextMeshProUGUI keyGamepadRecordText;
    [SerializeField] private TextMeshProUGUI keyDeleteText;
    [SerializeField] private TextMeshProUGUI keyGamepadDeleteText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActve())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        keyUseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Use);
        keyGamepadUseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Use);
        keyRecordText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Record);
        keyGamepadRecordText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Record);
        keyDeleteText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Delete);
        keyGamepadDeleteText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Delete);
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

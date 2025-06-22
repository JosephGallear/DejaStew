using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button recordButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadUseButton;
    [SerializeField] private Button gamepadRecordButton;
    [SerializeField] private Button gamepadDeleteButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI useText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI deleteText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadUseText;
    [SerializeField] private TextMeshProUGUI gamepadRecordText;
    [SerializeField] private TextMeshProUGUI gamepadDeleteText;
    [SerializeField] private Transform pressToRebindKeyTransform;

    [SerializeField] private List<GameObject> buttonsList = new List<GameObject>();
    private GameObject lastSelectedButton;

    private Action onCloseButtonAction;

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

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        useButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Use); });
        recordButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Record); });
        deleteButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Delete); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        gamepadUseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Use); });
        gamepadRecordButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Record); });
        gamepadDeleteButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Delete); });

        lastSelectedButton = soundEffectsButton.gameObject;
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    void Update()
    {
        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (!buttonsList.Contains(selectedGameObject))
        {
            EmptyAndSetSelectedGameObject(lastSelectedButton.gameObject);
        }
        else
        {
            lastSelectedButton = selectedGameObject;
        }
    }

    private void EmptyAndSetSelectedGameObject(GameObject gameObjectToSelect)
    {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObjectToSelect);
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        useText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Use);
        recordText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Record);
        deleteText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Delete);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadUseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Use);
        gamepadRecordText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Record);
        gamepadDeleteText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Delete);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        lastSelectedButton = soundEffectsButton.gameObject;

        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}

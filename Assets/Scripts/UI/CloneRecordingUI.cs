using UnityEngine;

public class CloneRecordingUI : MonoBehaviour
{
    [SerializeField] private GameObject notRecordingObject;
    [SerializeField] private GameObject recordingObject;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        recordingObject.SetActive(true);
        notRecordingObject.SetActive(false);
    }

    public void Hide()
    {
        recordingObject.SetActive(false);
        notRecordingObject.SetActive(true);
    }
}

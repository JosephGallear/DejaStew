using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private List<GameObject> buttonsList = new List<GameObject>();
    private GameObject lastSelectedButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;

        playButton.Select();
        lastSelectedButton = playButton.gameObject;
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
}

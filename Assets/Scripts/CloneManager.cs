using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CloneAppearance
{
    public Color cloneColour;
    public Color highlightedColour;
    public Sprite markerSprite;
}

public class CloneManager : MonoBehaviour
{
    public static CloneManager Instance { get; private set; }

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private GameObject markerPrefab;

    [SerializeField] private List<CloneAppearance> cloneAppearances;

    private List<CloneAppearance> availableCloneAppearances;
    private List<CloneAppearance> usedCloneAppearances;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        availableCloneAppearances = new List<CloneAppearance>(cloneAppearances);
        usedCloneAppearances = new List<CloneAppearance>();
    }

    public bool IsCloneLimitReached()
    {
        return availableCloneAppearances.Count == 0;
    }

    public void SpawnClone(Vector3 startPosition, List<TransformFrame> transformFrames, List<InputFrame> inputFrames, float duration)
    {
        if (availableCloneAppearances.Count == 0)
        {
            Debug.LogWarning("No more clone appearances available!");
            return;
        }

        CloneAppearance cloneAppearance = availableCloneAppearances[0];
        availableCloneAppearances.RemoveAt(0);
        usedCloneAppearances.Add(cloneAppearance);

        GameObject clone = Instantiate(clonePrefab, startPosition, Quaternion.identity);

        CloneInput cloneInput = clone.GetComponent<CloneInput>();
        cloneInput.Initialise(inputFrames, duration);

        CloneMovement cloneMovement = clone.GetComponent<CloneMovement>();
        cloneMovement.Initialise(transformFrames, duration, startPosition);

        CloneVisual cloneVisual = clone.GetComponent<CloneVisual>();
        cloneVisual.SetAppearance(cloneAppearance);

        CloneMarker marker = Instantiate(markerPrefab, startPosition, Quaternion.identity).GetComponent<CloneMarker>();
        marker.LinkToClone(clone);
        marker.SetMarkerAppearance(cloneAppearance);
    }

    public void ReleaseCloneAppearance(CloneAppearance cloneAppearance)
    {
        if (usedCloneAppearances.Contains(cloneAppearance))
        {
            usedCloneAppearances.Remove(cloneAppearance);
            availableCloneAppearances.Insert(0, cloneAppearance);
        }
    }
}
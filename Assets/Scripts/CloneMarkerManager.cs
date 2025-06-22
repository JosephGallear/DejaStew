using System.Collections.Generic;
using UnityEngine;

public class CloneMarkerManager : MonoBehaviour
{
    public static CloneMarkerManager Instance { get; private set; }

    private List<CloneMarker> nearbyMarkers = new();
    private CloneMarker currentClosestMarker;

    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnDeleteAction += GameInput_OnDeleteAction;
    }

    private void GameInput_OnDeleteAction(object sender, System.EventArgs e)
    {
        currentClosestMarker?.DeleteLinkedClone();
    }

    private void Update()
    {
        if (nearbyMarkers.Count == 0)
        {
            currentClosestMarker?.Highlight(false);
            currentClosestMarker = null;
            return;
        }

        CloneMarker closest = null;
        float closestDistance = float.MaxValue;

        foreach (CloneMarker marker in nearbyMarkers)
        {
            if (marker == null) continue;

            float distance = Vector3.Distance(playerTransform.position, marker.GetPosition());
            if (distance < closestDistance)
            {
                closest = marker;
                closestDistance = distance;
            }
        }

        if (closest != currentClosestMarker)
        {
            currentClosestMarker?.Highlight(false);
            closest?.Highlight(true);
            currentClosestMarker = closest;
        }
    }

    public void AddNearbyMarker(CloneMarker marker)
    {
        if (!nearbyMarkers.Contains(marker))
            nearbyMarkers.Add(marker);
    }

    public void RemoveNearbyMarker(CloneMarker marker)
    {
        nearbyMarkers.Remove(marker);
        if (currentClosestMarker == marker)
        {
            currentClosestMarker.Highlight(false);
            currentClosestMarker = null;
        }
    }
}

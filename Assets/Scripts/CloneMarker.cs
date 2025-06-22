using UnityEngine;

public class CloneMarker : MonoBehaviour
{
    private GameObject linkedClone;
    private bool isPlayerInside = false;

    [SerializeField] private Renderer markerRenderer;

    private Color normalColor;
    private Color highlightColor = Color.yellow;

    private void Awake()
    {
        normalColor = markerRenderer.material.color;
    }

    public void LinkToClone(GameObject clone)
    {
        linkedClone = clone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            CloneMarkerManager.Instance.AddNearbyMarker(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            CloneMarkerManager.Instance.RemoveNearbyMarker(this);
        }
    }

    public void Highlight(bool shouldHighlight)
    {
        markerRenderer.material.color = shouldHighlight ? highlightColor : normalColor;
    }

    public void DeleteLinkedClone()
    {
        if (linkedClone != null)
        {
            CloneMarkerManager.Instance.RemoveNearbyMarker(this);
            Destroy(linkedClone);
            Destroy(gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetMarkerAppearance(CloneAppearance cloneAppearance)
    {
        normalColor = cloneAppearance.cloneColour;
        highlightColor = cloneAppearance.highlightedColour;

        Material newMat = new Material(markerRenderer.material);
        newMat.mainTexture = cloneAppearance.markerSprite.texture;
        newMat.color = normalColor;
        
        markerRenderer.material = newMat;
    }
}

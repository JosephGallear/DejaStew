using UnityEngine;

public class CloneVisual : MonoBehaviour
{
    [SerializeField] private Renderer cloneRenderer;

    private CloneAppearance assignedAppearance;

    public void SetAppearance(CloneAppearance appearance)
    {
        assignedAppearance = appearance;

        Material newMat = new Material(cloneRenderer.material);

        newMat.color = new Color(appearance.cloneColour.r, appearance.cloneColour.g, appearance.cloneColour.b, 0.7f);

        cloneRenderer.material = newMat;
    }

    private void OnDestroy()
    {
        if (assignedAppearance != null && CloneManager.Instance != null)
        {
            CloneManager.Instance.ReleaseCloneAppearance(assignedAppearance);
        }
    }
}
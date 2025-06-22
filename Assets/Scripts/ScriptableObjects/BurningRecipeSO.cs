using UnityEngine;

[CreateAssetMenu(fileName = "Burning Recipe", menuName = "Burning Recipe")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}

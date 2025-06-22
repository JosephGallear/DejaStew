using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    public static event EventHandler OnAnyObjectTrashed;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else if (player.GetKitchenObject().GetKitchenObjectSO() == kitchenObjectSO)
        {
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            player.GetKitchenObject().DestroySelf();
        }
        else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            plateKitchenObject.TryAddIngredient(kitchenObjectSO);
        }
    }
}

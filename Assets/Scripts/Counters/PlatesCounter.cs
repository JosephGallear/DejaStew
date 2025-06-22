using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public EventHandler OnPlateSpawned;
    public EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (platesSpawnedAmount > 0)
        {
            if (!player.HasKitchenObject())
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this).TryGetPlate(out PlateKitchenObject plateKitchenObject);

                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().DestroySelf();

                    GetKitchenObject().SetKitchenObjectParent(player);

                }
                else
                {
                    GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}

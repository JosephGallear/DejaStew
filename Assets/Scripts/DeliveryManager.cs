using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour, IHasProgress
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    [SerializeField] private float maxCustomerPatience = 60f;

    private List<CustomerOrder> customerOrderList;

    private float spawnRecipeTimer;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

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

        customerOrderList = new List<CustomerOrder>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        float currentSpawnRate = DifficultyManager.Instance.GetCurrentSpawnRate();

        if (spawnRecipeTimer <= 0f && GameManager.Instance.IsGamePlaying())
        {
            spawnRecipeTimer = currentSpawnRate;

            if (!IsOrderListFull())
            {
                CustomerOrder customerOrder = new CustomerOrder(recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)], maxCustomerPatience);
                customerOrderList.Add(customerOrder);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnRecipeFailed.Invoke(this, EventArgs.Empty);
            }
        }

        for (int i = customerOrderList.Count - 1; i >= 0; i--)
        {
            customerOrderList[i].UpdatePatience(Time.deltaTime);

            if (customerOrderList[i].IsExpired())
            {
                customerOrderList.RemoveAt(i);
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 1f - (spawnRecipeTimer / DifficultyManager.Instance.GetCurrentSpawnRate())
        }) ;
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < customerOrderList.Count; i++)
        {
            RecipeSO waitingRecipeSO = customerOrderList[i].recipeSO;

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchRecipe = false;
                    }
                }

                if (plateContentsMatchRecipe)
                {
                    successfulRecipesAmount++;

                    customerOrderList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<CustomerOrder> GetCustomerOrderList()
    {
        return customerOrderList;
    }

    public bool IsOrderListFull()
    {
        return customerOrderList.Count >= waitingRecipesMax;
    }
    
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}

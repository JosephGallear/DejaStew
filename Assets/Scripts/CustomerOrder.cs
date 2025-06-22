using System;
using UnityEngine;

public class CustomerOrder : IHasProgress
{
    public RecipeSO recipeSO { get; private set; }
    public float patienceTimer { get; private set; }
    public float maxPatienceTime { get; private set; }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public CustomerOrder(RecipeSO recipeSO, float maxPatienceTime)
    {
        this.recipeSO = recipeSO;
        this.maxPatienceTime = maxPatienceTime;
        patienceTimer = maxPatienceTime;
    }

    public void UpdatePatience(float deltaTime)
    {
        patienceTimer -= deltaTime;
        patienceTimer = Mathf.Clamp(patienceTimer, 0f, maxPatienceTime);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = patienceTimer / maxPatienceTime
        });
    }

    public bool IsExpired() => patienceTimer <= 0f;
}
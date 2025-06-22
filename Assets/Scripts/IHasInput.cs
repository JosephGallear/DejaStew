using System;
using UnityEngine;

public interface IHasInput
{
    Vector2 GetMovementVectorNormalized();
    public event EventHandler OnInteractAction;
    public event EventHandler OnUseAction;
}

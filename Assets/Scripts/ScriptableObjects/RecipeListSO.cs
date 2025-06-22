using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe List", menuName = "Recipe List")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using FateGames.ArcadeIdle;

public class AssemblingMachine : Machine
{
    [SerializeField] private string outcomeTag;
    protected override Stackable Produce()
    {
        foreach (Ingredient ingredient in ingredientDictionary.Values)
        {
            for (int i = 0; i < ingredient.RequiredQuantity; i++)
            {
                Stackable item = ingredient.Stack.Pop();
                item.gameObject.SetActive(false);
            }
        }
        Stackable outcome = ObjectPooler.Instance.SpawnFromPool(outcomeTag, outcomeStack.Transform.position, outcomeStack.Transform.rotation).GetComponent<Stackable>();
        outcomeStack.Push(outcome);
        return outcome;
    }
}

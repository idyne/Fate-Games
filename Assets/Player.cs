using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
public class Player : MonoBehaviour
{
    [SerializeField] private ItemStack itemStack;
    [SerializeField] private ItemStack otherStack;
    [SerializeField] private AssemblingMachine machine;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if (!itemStack.CanPush) return;
            Stackable item = ObjectPooler.Instance.SpawnFromPool("Item", Vector3.zero, Quaternion.identity).GetComponent<Stackable>();
            itemStack.Push(item, true);
        }
        if (Input.GetKey(KeyCode.J))
        {
            itemStack.Transfer(otherStack, true);
        }
        if (Input.GetKey(KeyCode.K))
        {
            otherStack.Transfer(itemStack, true);
        }

        if (Input.GetKey(KeyCode.L))
        {
            machine.TransferIngredient("Paper", itemStack, true);
        }

    }

}

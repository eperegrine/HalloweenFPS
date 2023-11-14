using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct DropItem
{
    public GameObject Loot;
    [Range(0, 1)]
    public float DropRate;
}
    
[CreateAssetMenu(menuName = "Loot/DropTable", fileName = "DropTable")]
public class DropTable : ScriptableObject
{
    public int maxLoop = 15;
    public List<DropItem> Drops;

    public GameObject GetDrop()
    {
        var count = 0;
        while (count < maxLoop)
        {
            count++;
            var drop = GetRandomDrop();
            if (drop.DropRate > Random.Range(0f, 1f))
            {
                return drop.Loot;
            }
        }

        Debug.Log("Failed Early");
        //Failsafe
        return Drops[0].Loot;
    }

    private DropItem GetRandomDrop()
    {
        var index = Random.Range(0, Drops.Count);
        return Drops[index];
    }
}
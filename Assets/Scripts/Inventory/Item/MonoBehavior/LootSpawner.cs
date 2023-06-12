using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour {
    [System.Serializable]
    public class LootItem {
        public GameObject item;

        [Range(0, 1)]
        public float weight;
    }

    public LootItem[] lootItems;

    public void SpawnLoot() {
        float currentValue = Random.value;
        foreach (var lootItem in lootItems) {
            if (currentValue <= lootItem.weight) {
                GameObject obj = Instantiate(lootItem.item);
                obj.transform.position = transform.position + Vector3.up * 0.8f;
                break;
            }
        }
    }
}

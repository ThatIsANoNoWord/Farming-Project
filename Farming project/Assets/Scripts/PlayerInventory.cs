using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class PlayerInventory {
    private static Dictionary<string, int> inventory = new();

    public static void AddItem(string item, int count) {
        if (inventory.ContainsKey(item)) {
            inventory[item] += count;
        } else {
            inventory.Add(item, count);
        }
    }

    public static bool RemoveItem(string item, int requiredCount = 1) {
        if (!inventory.ContainsKey(item)) return false;

        int collectibleCount = inventory[item];

        if (collectibleCount < requiredCount) return false;

        inventory[item] -= requiredCount;

        return true;
    }

    public static void ClearItem(string item) {
        inventory.Remove(item);
    }

    public static void ClearInventory() {
        inventory.Clear();
    }

    public static bool HasItem(string item, int requiredCount = 1) {
        if (!inventory.ContainsKey(item)) return false;
        return inventory[item] >= requiredCount;
    }

    public static int GetItemCount(string item) {
        return inventory.GetValueOrDefault(item, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType { weapon, magazine, health, grenade, Armor, Key}
public enum DropSlot { none, hand, pistol, fragmentation, repetition, special }
public enum DropClass { normal, rare, legenday, epic}

[System.Serializable]
public class DropObject {
    public Transform prefab;
    public DropType dropType;
    public DropSlot dropSlot;
    public DropClass dropClass;
    public int internalIndex;
    public AudioClip pickSound = null;
    public Sprite invetoryIcon = null;
}

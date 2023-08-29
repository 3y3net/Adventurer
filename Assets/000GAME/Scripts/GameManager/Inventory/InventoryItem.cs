using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DD_GameManager
{    
    public class InventoryItem : ScriptableObject {
        public int UID;                     //Unique tag identifier
        public string Name;                 //Item name
        public Texture2D Icon;              // Texture for the icon
        public int stackAmount = 1;         // 1 no stackable  >1 max number of items in stack
        public bool allowRecipes = false;   // Is item selectable for recipre mode?
        public string description;          //Text description
        public GameObject prefab;           //Game object that represent the icon
        public bool getPrefabIcon = false;  //Auto update the texture with game object preview
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD_GameManager
{
    public class InventoryDatabase : MonoBehaviour
    {
        public static InventoryDatabase instance;
        //public List<InventoryItem> itemsDatabase = new List<InventoryItem>();
        public InventoryItem[] items = new InventoryItem[0];

        void Awake()
        {
            instance = this;
        }
    }
}
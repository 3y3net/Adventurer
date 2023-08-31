using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DD_GameManager
{
    public class InventoryManager : GameUIApp
    {
        public InventoryManager instance;

        public Text ItemDescription, itemName;
        public Sprite BGNormal, BGOver, BGSelected;
        public AudioClip itemEnter, itemSelect;

        public event Action<int> OnItemInspect;

        public List<InventoryItem> items = new List<InventoryItem>();
        public List<GameObject> itemsSlots = new List<GameObject>();

        public GameObject ItemSlot;

        
        bool invOpen = false;

        int SelectedItem = -1;
        public int nameDelta = 50000;
        public int descriptionDelta = 51000;       



        public void MouseEnterItem(GameObject slot)
        {
            Image Bg = slot.transform.Find("BackgroundImage").GetComponent<Image>();
            if (!slot.GetComponent<ItemSelector>().selected)          
                Bg.sprite = BGOver;
            audioSource.PlayOneShot(itemEnter);
        }

        public void MouseExitItem(GameObject slot)
        {
            Image Bg = slot.transform.Find("BackgroundImage").GetComponent<Image>();
            if (!slot.GetComponent<ItemSelector>().selected)
                Bg.sprite = BGNormal;
        }

        public void MouseDown(GameObject slot)
        {
            for (int i = 0; i < itemsSlots.Count; i++)
                if (itemsSlots[i] == slot)
                {
                    SelectedItem = i;
                    SelectItem(i);
                }
        }

        public void DeleteSelectedItem()
        {
            if(SelectedItem!=-1)
            {
                RemoveItemToInventory(SelectedItem);
                SelectedItem = -1;
            }
        }

        public void SelectItem(int index)
        {
            for (int i = 0; i < itemsSlots.Count; i++)
                if (itemsSlots[i].GetComponent<ItemSelector>().selected)
                    DeselectItem(i);

            itemsSlots[index].GetComponent<ItemSelector>().selected = true;
            Image Bg = itemsSlots[index].transform.Find("BackgroundImage").GetComponent<Image>();
            Bg.sprite = BGSelected;
            audioSource.PlayOneShot(itemSelect);
            ItemDescription.text = LocalizableData.instance.languageText[descriptionDelta + items[index].UID]; 
            itemName.text = LocalizableData.instance.languageText[nameDelta + items[index].UID];
        }

        public void DeselectItem(int index)
        {
            itemsSlots[index].GetComponent<ItemSelector>().selected = false;
            Image Bg = itemsSlots[index].transform.Find("BackgroundImage").GetComponent<Image>();
            Bg.sprite = BGNormal;
            ItemDescription.text = "";
            itemName.text = LocalizableData.instance.languageText[27];
        }

        public void InspectItem()
        {
            for (int i = 0; i < itemsSlots.Count; i++)
                if (itemsSlots[i].GetComponent<ItemSelector>().selected)
                {
                    if (OnItemInspect != null)
                        OnItemInspect(i);
                }
        }

        public void AddItemToInventory(int itemDBIndex)
        {
            InventoryItem item = InventoryDatabase.instance.items[itemDBIndex];
            GameObject go = Instantiate(ItemSlot, ItemSlot.transform.parent);
            Image img=go.transform.Find("ItemImage").GetComponent<Image>();
            img.sprite= Sprite.Create(item.Icon, new Rect(0, 0, item.Icon.width, item.Icon.height), new Vector2(0.5f, 0.5f));            
            go.SetActive(true);
            itemsSlots.Add(go);
            items.Add(item);
            badgeAmount++;
            localAmount++;
        }

        public void RemoveItemToInventory(int index)
        {
            GameObject destr = itemsSlots[index];
            items.RemoveAt(index);
            itemsSlots.RemoveAt(index);
            Destroy(destr);
            badgeAmount--;
            if (badgeAmount < 0)
                badgeAmount = 0;
            localAmount--;
            if (localAmount < 0)
                localAmount = 0;
        }

        void Awake()
        {
            instance = this;
        }
              
    }
}
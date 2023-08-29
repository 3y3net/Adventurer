using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DD_GameManager
{
    public class GameGoalsManager : GameUIApp
    {
        public Text mainTitle;
        public GameObject ItemSlotToDo;
        public GameObject ItemSlotDone;
        public GameObject ItemSlotClue;

        public List<GameGoal> itemsToDo = new List<GameGoal>();
        public List<GameObject> itemsSlotsToDo = new List<GameObject>();
        public List<GameGoal> itemsDone = new List<GameGoal>();
        public List<GameObject> itemsSlotsDone = new List<GameObject>();

        public string cluePending, clueDone, Conclusion;
        GameState gameState;

        void Start()
        {
            gameState = FindObjectOfType<GameState>();
            if (!gameState)
                throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");

            InitialLoad();
            SetText();
        }

        public void InitialLoad()
        {            
            for (int i = 0; i < GameGoalsList.instance.gameGoals.Length; i++)
                if (GameGoalsList.instance.gameGoals[i].active && gameState.gameStates[(int)GameGoalsList.instance.gameGoals[i].asocState])
                    AddGoalToDone(i);
                else if (GameGoalsList.instance.gameGoals[i].active && !gameState.gameStates[(int)GameGoalsList.instance.gameGoals[i].asocState])
                    AddGoalToDo(i);
        }

        public void SetText()
        {
            float total = 100.0f * (float)itemsDone.Count / (float)GameGoalsList.instance.gameGoals.Length;
            mainTitle.text = LocalizableData.instance.languageText[30] + " (" + (int)total + "% " + LocalizableData.instance.languageText[33] + ")";
        }

        public void AddGoalToDo(int itemDBIndex)
        {
            GameGoal item = GameGoalsList.instance.gameGoals[itemDBIndex];
            GameObject go = Instantiate(ItemSlotToDo, ItemSlotToDo.transform.parent);

            Image img = go.transform.Find("PanelTop/ItemSlot/ItemImage").GetComponent<Image>();
            Text title = go.transform.Find("PanelTop/ItemSlot/Title").GetComponent<Text>();
            Text Description = go.transform.Find("PanelTop/ItemSlot/Description").GetComponent<Text>();
            img.sprite = item.iconToDo;
            title.text = LocalizableData.instance.languageText[item.titleIdx];
            Description.text = LocalizableData.instance.languageText[item.descIniIdx];            

            item.active = true;
            go.SetActive(true);
            itemsSlotsToDo.Insert(0, go);
            itemsToDo.Insert(0, item);
            badgeAmount++;
            localAmount++;
            SetText();

            RepaintItemToDo(0);
        }
        
        public void RepaintItemAllToDo()
        {
            for (int i = 0; i < itemsSlotsToDo.Count; i++)
                RepaintItemToDo(i);
        }

        public void RepaintItemToDo(int internalIndex)
        {
            GameGoal item = itemsToDo[internalIndex];
            GameObject go = itemsSlotsToDo[internalIndex];
            
            foreach (Transform child in go.transform.Find("PanelBottom"))
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < item.clues.Length; i++)
            {
                GameObject titleGo = Instantiate(ItemSlotClue, go.transform.Find("PanelBottom"));
                titleGo.SetActive(true);
                if (gameState.gameStates[(int)item.clues[i].asocState])
                {
                    titleGo.GetComponent<Text>().text = clueDone + LocalizableData.instance.languageText[item.clues[i].textDoneIdx];
                }
                else
                {
                    if (item.clues[i].textClueIdx >= 0)
                        titleGo.GetComponent<Text>().text = cluePending + LocalizableData.instance.languageText[item.clues[i].textClueIdx];
                    else
                        titleGo.GetComponent<Text>().text = cluePending + LocalizableData.instance.languageText[29];
                }
            }

        }

        public void RepaintItemDone(int internalIndex)
        {
            GameGoal item = itemsDone[internalIndex];
            GameObject go = itemsSlotsDone[internalIndex];

            foreach (Transform child in go.transform.Find("PanelBottom"))
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < item.clues.Length; i++)
            {
                GameObject titleGo = Instantiate(ItemSlotClue, go.transform.Find("PanelBottom"));
                titleGo.SetActive(true);
                titleGo.GetComponent<Text>().text = clueDone + LocalizableData.instance.languageText[item.clues[i].textDoneIdx];                
            }

        }

        public void MoveGoalToDone(int itemDBIndex)
        {
            GameGoal item = GameGoalsList.instance.gameGoals[itemDBIndex];
            GameObject go = Instantiate(ItemSlotDone, ItemSlotDone.transform.parent);
            go.transform.SetAsFirstSibling();
            Text title = go.transform.Find("PanelTop/ItemSlot/Title").GetComponent<Text>();
            Image img = go.transform.Find("PanelTop/ItemSlot/ItemImage").GetComponent<Image>();
            Text Description = go.transform.Find("PanelTop/ItemSlot/Description").GetComponent<Text>();
            title.text = LocalizableData.instance.languageText[item.titleIdx];
            img.sprite = item.iconDone;
            Description.text = LocalizableData.instance.languageText[item.descEndIdx];
            go.SetActive(true);
            itemsSlotsDone.Insert(0, go);
            itemsDone.Insert(0, item);
            badgeAmount--;
            if (badgeAmount < 0)
                badgeAmount = 0;
            localAmount--;
            if (localAmount < 0)
                localAmount = 0;
            for (int i = 0; i < itemsToDo.Count; i++)
                if (itemsToDo[i] == GameGoalsList.instance.gameGoals[itemDBIndex])
                {
                    RemoveGoalFromToDoAtIndex(i);
                    break;
                }

            SetText();
            RepaintItemDone(0);
        }

        public void AddGoalToDone(int itemDBIndex)
        {
            GameGoal item = GameGoalsList.instance.gameGoals[itemDBIndex];
            GameObject go = Instantiate(ItemSlotDone, ItemSlotDone.transform.parent);
            go.transform.SetAsFirstSibling();
            Image img = go.transform.Find("ItemImage").GetComponent<Image>();
            Text title = go.transform.Find("Title").GetComponent<Text>();
            Text Description = go.transform.Find("Description").GetComponent<Text>();
            img.sprite = item.iconDone;
            title.text = LocalizableData.instance.languageText[item.titleIdx];
            Description.text = LocalizableData.instance.languageText[item.descEndIdx];

            for (int i = 0; i < item.clues.Length; i++)
            {
                GameObject titleGo = Instantiate(ItemSlotClue, go.transform.Find("PanelBottom"));
                titleGo.SetActive(true);
                titleGo.GetComponent<Text>().text = clueDone + LocalizableData.instance.languageText[item.clues[i].textDoneIdx];                
            }
            if(item.conclusionIdx>=0)
            {
                GameObject titleGo = Instantiate(ItemSlotClue, go.transform.Find("PanelBottom"));
                titleGo.SetActive(true);
                titleGo.GetComponent<Text>().text = Conclusion + LocalizableData.instance.languageText[item.conclusionIdx];
            }

            go.SetActive(true);
            itemsSlotsDone.Insert(0, go);
            itemsDone.Insert(0, item);
            SetText();
            RepaintItemDone(0);
        }

        public void RemoveGoalFromToDoAtIndex(int index)
        {
            GameObject destr = itemsSlotsToDo[index];
            itemsToDo.RemoveAt(index);
            itemsSlotsToDo.RemoveAt(index);
            Destroy(destr);
        }
    }
}
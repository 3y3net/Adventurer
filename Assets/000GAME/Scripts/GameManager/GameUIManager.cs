using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DD_GameManager
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager instance;

        public GameObject Interface, badgeApp;
        public Text badgeText;
        public AudioClip On, Off;
        public Button tabletIcon;
        AudioSource audioSource;

        public List<GameUIApp> gameApps = new List<GameUIApp>();
        private int badgeAmount = 0;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            foreach(GameUIApp gap in gameApps)
                gap.audioSource = audioSource;
        }

        void Update()
        {
            badgeAmount = 0;
            foreach (GameUIApp gap in gameApps)
                badgeAmount += gap.badgeAmount;

            badgeText.text = "" + badgeAmount;
            if (badgeAmount > 0 && !badgeApp.activeSelf)
            {                
                badgeApp.SetActive(true);
            }

            if (badgeAmount == 0 && badgeApp.activeSelf)
                badgeApp.SetActive(false);
        }

        public void ShowTabletIcon()
        {
            tabletIcon.interactable = true;
        }

        public void HideTabletIcon()
        {
            tabletIcon.interactable = false;
        }

        public void ShowInterface()
        {
            if (Interface != null)
            {
                if (audioSource != null && On != null)
                    audioSource.PlayOneShot(On);
                Interface.SetActive(true);

                PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                if (pm)
                    pm.StopAll();
            }
        }

        public void FastHide()
        {
            Interface.SetActive(false);
        }

        public void FastRestore()
        {
            Interface.SetActive(true);
        }

        public void HideInterface()
        {
            if (Interface != null)
            {
                foreach (GameUIApp gap in gameApps)
                    gap.FastSilentCloseApp();

                if (audioSource != null && Off != null)
                    audioSource.PlayOneShot(Off);
                Interface.SetActive(false);
                PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                if (pm)
                    pm.ResumeAll();
            }
        }

        public void ShowHideInterface()
        {
            if (Interface != null && !Interface.activeSelf)
                ShowInterface();
            else if (Interface != null && Interface.activeSelf)
                HideInterface();
        }
    }
}
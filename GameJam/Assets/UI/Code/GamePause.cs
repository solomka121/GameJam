using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GamePause : MonoBehaviour
    {
        [SerializeField] internal GameObject pauseMenu;
        [SerializeField] private GameObject pauseSettings;
        [SerializeField] private GameObject pauseControls;

        public bool messageUIActive;
        
        private void Update()
        {
           
            if (Input.GetButtonDown(NameManager.CancelAxis) /*Input.GetKeyDown(KeyCode.Escape)*/) //esc открывает меню паузы и стопит игру
            {
                if (messageUIActive)
                {
                    return;
                } 
                if (!pauseMenu.activeInHierarchy)
                {
                    MenuCode.CursorShow();
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0; //останавливает игру
                }
                else
                {
                    pauseControls.SetActive(false);
                    pauseSettings.SetActive(false);
                    pauseMenu.SetActive(false); //выйти с паузы тоже можно с помощью esc
                    Time.timeScale = 1; //восстанавливает ход времени
                    MenuCode.CursorLock();
                }
            }
        }

     

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            MenuCode.CursorLock();
        }

        public void SettingBtnPause()
        {
            pauseSettings.SetActive(true);
            pauseMenu.SetActive(false);
            pauseControls.SetActive(false);
        }
        
        public void BackBtnPause()
        {
            pauseSettings.SetActive(false);
            pauseMenu.SetActive(true);
            pauseControls.SetActive(false);
        }

        public void ControlsBtnPause()
        {
            pauseSettings.SetActive(false);
            pauseMenu.SetActive(false);
            pauseControls.SetActive(true);
        }

        public void MainMenuBtn()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            //SceneManager.LoadScene(NameManager.MenuScene);
        }


    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GamePause : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject pauseSettings;
        [SerializeField] private GameObject pauseControls;


        private void Update()
        {
            if (Input.GetButtonDown(NameManager.CancelAxis)) //esc открывает меню паузы и стопит игру
            {
                if (!pauseMenu.activeInHierarchy)
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0; //останавливает игру
                }
                else
                {
                    pauseMenu.SetActive(false); //выйти с паузы тоже можно с помощью esc
                    Time.timeScale = 1; //восстанавливает ход времени
                }
            }
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
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
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            //SceneManager.LoadScene(NameManager.MenuScene);
        }
    }
}
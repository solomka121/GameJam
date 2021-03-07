using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCode : MonoBehaviour
{
    // сериализуем объекты с менюшками
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject difficulty;

    private const int Easy = 10;
    private const int Medium = 8;
    private const int Hard = 5;

    
    private void Start()
    {
        CursorShow();
    }

    public void StartBtn() //кнопка запуска игры
    {
        menu.SetActive(false);
        controls.SetActive(false);
        settings.SetActive(false);
        difficulty.SetActive(true);
    }

    public void EasyBtn()
    {
        DifficultyTemp.TempHp = Easy; // записываем количество хп
        SceneManager.LoadScene(1, LoadSceneMode.Single); //по айдишнику в билде
        //SceneManager.LoadScene(NameManager.GameScene); //по названию сцены
    }    
    public void MediumBtn()
    {
        DifficultyTemp.TempHp = Medium; // записываем количество хп
        SceneManager.LoadScene(1, LoadSceneMode.Single); //по айдишнику в билде
        //SceneManager.LoadScene(NameManager.GameScene); //по названию сцены
    }   
    public void HardBtn()
    {
        DifficultyTemp.TempHp = Hard; // записываем количество хп
        SceneManager.LoadScene(1, LoadSceneMode.Single); //по айдишнику в билде
        //SceneManager.LoadScene(NameManager.GameScene); //по названию сцены
    }

    public void SettingsBtn() //переключаем на настройки
    {
        menu.SetActive(false);
        controls.SetActive(false);
        settings.SetActive(true);
        difficulty.SetActive(false);
    }
    
    public void ControlsBtn() //переключаем на управление
    {
        menu.SetActive(false);
        controls.SetActive(true);
        settings.SetActive(false);
        difficulty.SetActive(false);
    }

    public void BackBtn() //возвращает в меню
    {
        menu.SetActive(true);
        settings.SetActive(false);
        controls.SetActive(false);
        difficulty.SetActive(false);
    }

    public void ExitBtn() 
    {
        Application.Quit();
    }
    
    public static void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void CursorShow()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
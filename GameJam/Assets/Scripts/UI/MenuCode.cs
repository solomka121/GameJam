using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCode : MonoBehaviour
{
    // сериализуем объекты с менюшками
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;

    public void StartBtn() //кнопка запуска игры
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single); //по айдишнику в билде
        //SceneManager.LoadScene(NameManager.GameScene); //по названию сцены
    }

    public void SettingsBtn() //переключаем на настройки
    {
        menu.SetActive(false);
        controls.SetActive(false);
        settings.SetActive(true);
    }
    
    public void ControlsBtn() //переключаем на управление
    {
        menu.SetActive(false);
        controls.SetActive(true);
        settings.SetActive(false);
    }

    public void BackBtn() //возвращает в меню
    {
        menu.SetActive(true);
        settings.SetActive(false);
        controls.SetActive(false);
    }

    public void ExitBtn() 
    {
        Application.Quit();
    }
}
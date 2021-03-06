using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SoundControllerUI : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private AudioSource musicSource;

        private void Start()
        {
            musicSlider.value = PlayerPrefs.GetFloat("Music", musicSlider.value);
        }

        private void Update()
        {
            musicSource.volume = musicSlider.value;
            
            PlayerPrefs.SetFloat("Music", musicSlider.value); //сохраняем пользовательский выбор
        }
    }
}
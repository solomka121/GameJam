using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
		else
		{
            Destroy(gameObject);
            return; //to make sure no code is called before we destroy our object
		}
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
		{
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
           s.source.loop = s.loop;
		}
    }

    // Update is called once per frame
    public void PlaySound(string name)
    {
		Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound" + name + " not found! Check the spelling in inspector!");
            return; 
        }
        s.source.Play();
    }

    public void FadeSound(string name)
	{
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float currentTime = 0;
        float currentVolume = s.volume;
        while (currentTime < s.fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            s.volume = Mathf.Lerp(currentVolume, s.targetFadeOutVolume, currentTime / s.fadeOutDuration);
        }
    }
}

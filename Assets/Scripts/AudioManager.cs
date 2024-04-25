using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; //Skaber et array kaldet sounds som indeholder alle sounds
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            //s.source = gameObject.AddComponent<AudioSource>();     //Tager fat i Audio Source komponenten p� et gameObject
            s.source = gameObject.GetComponent<AudioSource>();
            //s.source.clip = s.clip;         //S�tter audio klippet fra audio source component til s.clip

            //s.source.volume = s.volume;     //S�tter volume fra audio source component til s.volume
            //s.source.pitch = s.pitch;       //S�tter pitch fra audio source til s.pitch
        }
    }

    public float Play (string name)       //Tilf�jer en m�de at spille en specifik sound ud fra den string name
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);     //Finder den sound i 'sounds' array som har det samme navn som string name (i denne void) og opbevarer den fundne sound i variablen 's'
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        
        s.source.Play();
            
        float clipLength = s.source.clip.length;
        
        return clipLength;
    }
}

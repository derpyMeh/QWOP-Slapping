using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //For at nedenst�ende custom class kan vises i inspector skal dette linje bruges
public class Sound
{

    public string name;
    
    public AudioClip clip;
    
    [Range(0f, 1f)]     //G�r s� volume kan s�ttes til en float v�rdi mellem 0 og 1
    public float volume;
    
    [Range(.1f, 3f)]    //G�r s� pitch kan s�ttes til en float v�rdi mellem 0.1 og 3
    public float pitch;
    
    [HideInInspector]       //Vises ikke i inspector, men kan stadig tage fat i variablen i andre scripts da den er public
    public AudioSource source;
}
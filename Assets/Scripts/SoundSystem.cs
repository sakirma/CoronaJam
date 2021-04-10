using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem INSTANCE;
    
    [SerializeField] private GameObject soundPrefab;

    [SerializeField] private AudioClip ambientClip;

    void Start()
    {
        INSTANCE = this;
        
        PlaySound(ambientClip);
    }

    public void PlaySound(AudioClip clip, float x = 0f, float y = 0f, float z = 0f)
    {
        GameObject sound = Instantiate(soundPrefab, new Vector3(x, y, z), Quaternion.identity);
        sound.GetComponent<Sound>().Setup(clip);
    }
}

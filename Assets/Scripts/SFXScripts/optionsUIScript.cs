using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using UnityEngine.UI;

public class optionsUIScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;


    // Start is called before the first frame update
    void Start()
    {
        SFXVolumeChange();
        MasterVolumeChange();
        MusicVolumeChange();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MasterVolumeChange()
    {
        float newvolume = masterVolumeSlider.value;

        audioMixer.SetFloat("Master", newvolume);
    }

    public void MusicVolumeChange()
    {
        float newvolume = MusicVolumeSlider.value;

        audioMixer.SetFloat("Music", newvolume);
    }

    public void SFXVolumeChange()
    {
        float newvolume = SFXVolumeSlider.value;

        audioMixer.SetFloat("soundFX", newvolume);
    }
}

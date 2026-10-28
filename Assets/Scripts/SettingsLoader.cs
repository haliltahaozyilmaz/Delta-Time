using UnityEngine;
using UnityEngine.Audio;

public class SettingsLoader : MonoBehaviour
{
    public AudioMixer mainMixer;

    void Start()
    {
        if (mainMixer == null)
        {
            Debug.LogError("AYAR YÜKLEYİCİ: Mixer atanmamış!");
            return;
        }

        float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        Debug.Log("Ayarlar Yükleniyor - Müzik: " + musicVol + " SFX: " + sfxVol);

        ApplyVolume("MusicVol", musicVol);
        ApplyVolume("SFXVol", sfxVol);
    }

    void ApplyVolume(string parameterName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, sliderValue)) * 20;
        bool result = mainMixer.SetFloat(parameterName, dB);
        
        if(!result) Debug.LogError(parameterName + " ismi Mixer'da bulunamadı!");
    }
}
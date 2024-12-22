using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; 
    
    [SerializeField] private float defaultVolumeScene = 0.5f; 

    private const string VolumePrefKey = "GameVolume"; 

    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        var savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        audioSource.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void SetVolume(float value)
    {
        audioSource.volume = Mathf.Clamp(value, 0f, 1f);
        PlayerPrefs.SetFloat(VolumePrefKey, value); 
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    //private void OnApplicationQuit()
    //{
    //    // Сохраняем изменения громкости при выходе из игры
    //    PlayerPrefs.Save();
    //}
}

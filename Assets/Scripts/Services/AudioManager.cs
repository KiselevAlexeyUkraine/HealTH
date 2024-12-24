using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Services
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _mixer;
        [SerializeField] 
        private Slider _volumeSlider; 
        [SerializeField] 
        private float _defaultVolumeScene = 0.5f; 

        private const string MasterVolumePrefKey = "MasterVolume"; 

        private void Start()
        {
            var savedVolume = PlayerPrefs.GetFloat(MasterVolumePrefKey, 0.5f);

            if (_volumeSlider != null)
            {
                _volumeSlider.value = savedVolume;
                _volumeSlider.onValueChanged.AddListener(SetVolume);
            }
            
            SetVolume(savedVolume);
        }

        private void SetVolume(float value)
        {
            value = Mathf.Clamp(value, 0.0001f, 1f);
            var volume = Mathf.Log10(value) * 20f;
            _mixer.SetFloat(MasterVolumePrefKey, volume); 
            
            PlayerPrefs.SetFloat(MasterVolumePrefKey, value); 
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            if (_volumeSlider != null)
            {
                _volumeSlider.onValueChanged.RemoveListener(SetVolume);
            }
        }
    }
}

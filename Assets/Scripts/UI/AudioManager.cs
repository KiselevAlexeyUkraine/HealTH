using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Slider для управления громкостью
    [SerializeField] private AudioSource audioSource; // Источник звука, которым управляет Slider
    [SerializeField] private float defaultVolumeScene = 0.5f; // если мы заходим впервые

    private const string VolumePrefKey = "GameVolume"; // Ключ для сохранения громкости

    private void Start()
    {
        // Загружаем сохранённое значение громкости или устанавливаем его по умолчанию
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        audioSource.volume = savedVolume;

        // Если Slider задан, настраиваем его
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value; // Обновляем громкость
        PlayerPrefs.SetFloat(VolumePrefKey, value); // Сохраняем значение громкости
    }

    private void OnDestroy()
    {
        // Убираем слушателя при уничтожении объекта
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

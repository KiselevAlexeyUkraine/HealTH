using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float motivation = 100f; // Текущая мотивация игрока
    public float Motivation { get => motivation; }
    [SerializeField] private TMP_Text scoreTextCoins; // TextMeshPro-текст для отображения очков монет
    [SerializeField] private TMP_Text scoreTextDollars; // TextMeshPro-текст для отображения очков долларов
    [SerializeField] private Image motivationImage; // Изображение для отображения мотивации
    [SerializeField] private PlayerMovementRidgitBody playerMovementRidgitBody;
    private CursorToggle cursorToggle;

    [SerializeField] private AudioClip coinPickupSound; // Звук подбора монеты
    [SerializeField] private AudioClip dollarPickupSound; // Звук подбора доллара
    [SerializeField] private AudioClip brightPickupSound; // Звук подбора доллара
    private AudioSource audioSource; // Источник звука

    private int scoreDollars = 0; // Текущие очки долларов
    public float ScoreDollars { get => scoreDollars; }
    private int scoreCoins = 0; // Текущие очки монет
    public float ScoreCoins { get => scoreCoins; }

    private float motivationLogInterval = 0.2f; // Интервал времени для вывода мотивации в консоль
    private float lastMotivationLogTime = 0f; // Время последнего вывода мотивации
    public bool IsDeath { get; private set; }

    private float playTime; // Время, проведенное в игре

    private void Start()
    {
        cursorToggle = GetComponent<CursorToggle>();
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }

    private void Update()
    {
        // Увеличиваем время игры на время, прошедшее с последнего кадра
        playTime += TimeManager.instance.TimeDeltaTime;
    }

    // Метод для получения времени в формате "минуты:секунды.десятые"
    public string GetFormattedPlayTime()
    {
        int minutes = Mathf.FloorToInt(playTime / 60);
        float seconds = playTime % 60;
        return string.Format("{0}:{1:00.0}", minutes, seconds);
    }

    public void AddScore(int score)
    {
        if (score == (int)ScoreValutes.scoreCoins)
        {
            scoreCoins++;
            motivation += 5f;
            PlaySound(coinPickupSound); // Воспроизводим звук подбора монеты
        }
        else if (score == (int)ScoreValutes.scoreDollars)
        {
            scoreDollars++;
            motivation += 50f;
            PlaySound(dollarPickupSound); // Воспроизводим звук подбора доллара
        }
        else if (score == (int)ScoreValutes.scoreMotivations)
        {
            motivation += 20f;
            PlaySound(brightPickupSound);
        }
        UpdateScoreText();
        UpdateScoreMotivation();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Воспроизводим звук
        }
    }

    private void UpdateScoreText()
    {
        scoreTextCoins.text = "Score coins: " + scoreCoins;
        scoreTextDollars.text = "Score dollars: " + scoreDollars;
    }

    private void UpdateScoreMotivation()
    {
        motivation = Mathf.Clamp(motivation, 0, 100); // Ограничиваем мотивацию от 0 до 100
        motivationImage.fillAmount = motivation / 100f; // Обновляем заполнение изображения
    }

    public void DecreaseMotivation(float amount)
    {
        if (motivation > 0)
        {
            motivation -= amount;
            motivation = Mathf.Clamp(motivation, 0, 100); // Ограничиваем мотивацию от 0 до 100

            // Проверяем, прошло ли достаточно времени для вывода мотивации
            if (Time.time - lastMotivationLogTime >= motivationLogInterval)
            {
                motivationImage.fillAmount = motivation / 100f; // Обновляем заполнение изображения
                Debug.Log($"Мотивация игрока: {motivation}");
                lastMotivationLogTime = Time.time; // Обновляем время последнего вывода
            }
        }
    }

    public void DecreaseMotivationForEnemy(float amount)
    {
        if (motivation > 0)
        {
            motivation -= amount;
            motivation = Mathf.Clamp(motivation, 0, 100); // Ограничиваем мотивацию от 0 до 100
            motivationImage.fillAmount = motivation / 100f; // Обновляем заполнение изображения
            playerMovementRidgitBody.TakeDamage();
        }

        // Если мотивация падает до 0, можно добавить логику поражения
        if (motivation <= 0)
        {
            Debug.Log("Игрок потерял всю мотивацию!");
            FaildGame();
        }
    }

    private void FaildGame()
    {
        IsDeath = true;
        cursorToggle.ToggleCursorVisibility();
    }
}

public enum ScoreValutes
{
    scoreCoins,
    scoreDollars,
    scoreMotivations
}
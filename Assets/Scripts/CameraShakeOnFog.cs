using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShakeOnFog : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Ссылка на вашу виртуальную камеру
    [SerializeField] private float shakeDuration = 1f; // Длительность тряски
    [SerializeField] private float shakeMagnitude = 0.1f; // Амплитуда тряски
    [SerializeField] private float shakeInterval = 0.5f; // Интервал между трясками

    private bool isShaking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Убедитесь, что у игрока установлен тег "Player"
        {
            StartCoroutine(ShakeCamera());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines(); // Остановить все корутины, чтобы прекратить тряску
            isShaking = false; // Убедитесь, что тряска отключена
        }
    }

    private IEnumerator ShakeCamera()
    {
        if (isShaking) yield break; // Если уже трясется, выходим

        isShaking = true;

        // Получаем Transposer
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 originalPosition = transposer.m_FollowOffset;

        while (true) // Бесконечный цикл для периодической тряски
        {
            float elapsed = 0f;

            // Тряска
            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;

                transposer.m_FollowOffset = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                elapsed += Time.deltaTime;

                yield return null; // Ждем следующего кадра
            }

            transposer.m_FollowOffset = originalPosition; // Возвращаем камеру в исходное положение

            // Ждем перед следующей тряской
            yield return new WaitForSeconds(shakeInterval);
        }
    }
}
using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] KeyCode dashKey = KeyCode.Z; // Замените KeyCode.Space на нужную вам кнопку
    [SerializeField] float dashDistance = 4f; // Дистанция рывка
    [SerializeField] float dashDuration = 0.5f; // Время рывка
    [SerializeField] float dashCooldown = 1f; // Время перезарядки рывка
    bool canDash = true; // Флаг, указывающий, может ли персонаж сделать рывок

    PlayerStats playerStats;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash && playerStats.Helth > 0)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false; // Запретить повторный рывок
        Vector3 startPosition = rb.position;
        Vector3 dashDirection = transform.forward * dashDistance; // Направление рывка
        Vector3 targetPosition = startPosition + dashDirection;

        // Проверка на столкновение с препятствиями
        if (IsPathClear(startPosition, targetPosition))
        {
            float elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, (elapsedTime / dashDuration)));
                elapsedTime += Time.deltaTime;
                yield return null; // Ждать следующего кадра
            }

            rb.MovePosition(targetPosition); // Убедиться, что персонаж достиг конечной позиции
            playerStats.DecreaseMotivationForEnemy(1); // Забираем одну единицу здоровья за ускорение
        }
        else
        {
            Debug.Log("Dash blocked by an obstacle!");
        }

        yield return new WaitForSeconds(dashCooldown); // Ждать время перезарядки
        canDash = true; // Разрешить следующий рывок
    }

    private bool IsPathClear(Vector3 start, Vector3 target)
    {
        // Проверка на наличие препятствий между начальной и целевой позицией
        RaycastHit hit;
        if (Physics.Linecast(start, target, out hit))
        {
            // Если есть столкновение, возвращаем false
            return false;
        }
        return true; // Путь свободен
    }
}
using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private KeyCode dashKey = KeyCode.Z; // Замените KeyCode.Space на нужную вам кнопкуs
    [SerializeField] private float dashDistance = 4f; // Дистанция рывка
    [SerializeField] private float dashDuration = 0.5f; // Время рывка
    [SerializeField] private float dashCooldown = 1f; // Время перезарядки рывка
    private bool canDash = true; // Флаг, указывающий, может ли персонаж сделать рывок
    private PlayerStats playerStats;

    private Rigidbody rb;

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

    private IEnumerator Dash()
    {
        canDash = false; // Запретить повторный рывок
        Vector3 startPosition = rb.position;
        Vector3 dashDirection = transform.forward * dashDistance; // Направление рывка
        Vector3 targetPosition = startPosition + dashDirection;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, (elapsedTime / dashDuration)));
            elapsedTime += Time.deltaTime;
            yield return null; // Ждать следующего кадра
        }

        rb.MovePosition(targetPosition); // Убедиться, что персонаж достиг конечной позиции
        playerStats.DecreaseMotivationForEnemy(1); // Забераем одну единицу здоровья за ускорение
        yield return new WaitForSeconds(dashCooldown); // Ждать время перезарядки
        canDash = true; // Разрешить следующий рывок
    }
}
   
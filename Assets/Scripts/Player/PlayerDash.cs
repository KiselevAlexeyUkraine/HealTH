using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public KeyCode dashKey = KeyCode.Z;
    public float dashDistance = 4f; // Дистанция рывка
    public float dashDuration = 0.5f; // Время рывка
    public float dashCooldown = 1f; // Время перезарядки рывка
    private bool canDash = true; // Флаг, указывающий, может ли персонаж сделать рывок

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash) // Замените KeyCode.Space на нужную вам кнопку
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
        yield return new WaitForSeconds(dashCooldown); // Ждать время перезарядки
        canDash = true; // Разрешить следующий рывок
    }
}
   
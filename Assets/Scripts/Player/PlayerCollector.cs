using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    // Переменная для хранения количества собранных предметов
    private int itemCount = 0;

    // Метод, который будет вызываться при столкновении с предметом
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект, с которым мы столкнулись, предметом
        if (other.CompareTag("Key"))
        {
            // Увеличиваем количество собранных предметов
            itemCount++;

            // Выводим количество собранных предметов в консоль
            Debug.Log("Собрано предметов: " + itemCount);

            // Удаляем предмет из сцены
            Destroy(other.gameObject);
        }
    }
}

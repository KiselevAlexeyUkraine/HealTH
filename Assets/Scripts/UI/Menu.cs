using UnityEngine;

public class Menu : MonoBehaviour
{
    // Имя меню, которое можно установить в инспекторе Unity
    public string menuName;

    // Метод для открытия меню
    public void Open()
    {
        // Активируем объект меню, чтобы он стал видимым
        gameObject.SetActive(true);
    }

    // Метод для закрытия меню
    public void Close()
    {
        // Деактивируем объект меню, чтобы он стал невидимым
        gameObject.SetActive(false);
    }
}

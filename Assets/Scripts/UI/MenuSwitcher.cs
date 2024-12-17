using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    // Статическая переменная для хранения единственного экземпляра MenuManager (Singleton)
    public static MenuSwitcher instance;

    // Список меню, который будет сериализован и доступен в инспекторе Unity
    [SerializeField] private List<Menu> _menuList;

    private void Awake()
    {
        instance = this;
    }


    // Метод для открытия меню по имени (для кнопки)
    public void OpenMenu(string menuName)
    {
        // Проходим по каждому меню в списке
        foreach (Menu menu in _menuList)
        {
            // Если имя меню совпадает с переданным именем
            if (menu.menuName == menuName)
            {
                // Открываем это меню
                menu.Open();
            }
            else
            {
                // Закрываем все остальные меню
                menu.Close();
                Debug.Log("Закрыли все меню");
            }
        }
    }
}
public enum MenuNames
{
    MainMenu = 0,
    GameMenu = 1,
    ContinuationGameMenu,
    FaildGameMenu
}
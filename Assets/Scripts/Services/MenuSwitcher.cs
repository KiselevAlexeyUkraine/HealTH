using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public static MenuSwitcher instance;

    [SerializeField] private List<Menu> _menuList;

    private void Awake()
    {
        instance = this;
    }

    public void OpenMenu(MenuNames menuName)
    {
        foreach (var menu in _menuList)
        {
            if (menu.menuName == menuName)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
}
public enum MenuNames
{
    None,
    MainMenu,
    GameMenu,
    ContinuationGame,
    ResultGame,
    FailedGame,
    Authors,
    Settings,
}
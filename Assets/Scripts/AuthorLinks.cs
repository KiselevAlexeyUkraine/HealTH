using UnityEngine;
using UnityEngine.UI;

public class AuthorLinks : MonoBehaviour
{
    // Ссылки на авторов
    [System.Serializable]
    public class Author
    {
        public string name; // Имя автора
        public string telegramLink; // Ссылка на Telegram
        public string twitterLink; // Ссылка на Twitter
        public string otherLink; // Другие ссылки
    }

    public Author[] authors; // Массив авторов
    [SerializeField] private Button[] buttons;

    // Метод для открытия ссылки
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

}
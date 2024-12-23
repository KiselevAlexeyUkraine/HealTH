using UnityEngine;

public class Menu : MonoBehaviour
{
    public MenuNames menuName;

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    private bool _isCursorVisible;

    public void ToggleCursorVisibility()
    {
        _isCursorVisible = !_isCursorVisible;
        
        Cursor.visible = _isCursorVisible;
        Cursor.lockState = _isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
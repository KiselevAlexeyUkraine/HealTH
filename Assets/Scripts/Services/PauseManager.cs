using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        Time.timeScale = 1;
    }

    public void SwitchState()
    {
        if (IsPaused)
        {
            Pause();
        }
        else
        {
            Play();  
        }
        
        IsPaused = !IsPaused;
    }
}

using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public Vector3 Movement => new Vector3(Horizontal, 0.0f, Vertical).normalized;
    public bool Spriting => Input.GetKey(KeyCode.LeftShift);
    public bool Jump => Input.GetKey(KeyCode.Space);
    public bool PauseMenu => Input.GetKeyDown(KeyCode.Escape);
    public float Horizontal => Input.GetAxis(InputKey.Horizontal.ToString());
    public float Vertical => Input.GetAxis(InputKey.Vertical.ToString());


    //public float Attack { get; private set; }

    //public bool PauseMenu { get; private set; }
}

public enum InputKey
{
    Horizontal,
    Vertical,
    Fire1,
    Jump,
}
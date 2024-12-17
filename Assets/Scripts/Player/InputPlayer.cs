using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Vector3 movement;

    public Vector3 Movement
    {
        get
        {
            return movement;
        }
        private set { }
    }

    private enum InputKey
    {
        Horizontal,
        Vertical,
        //Fire1,
        Jump
    }
    
    public bool Spriting { get; private set; }
    public bool Jump { get; private set; }
    public bool PauseMenu { get; private set; }

    public float Horizontal { get; private set; }
    public float Vertival { get; private set; }


    //public float Attack { get; private set; }

    //public bool PauseMenu { get; private set; }

    private void Update()
    {
        Horizontal = Input.GetAxis(InputKey.Horizontal.ToString()); // A/D или стрелки влево/вправо
        Vertival = Input.GetAxis(InputKey.Vertical.ToString()); // W/S или стрелки вверх/вниз
        Jump = Input.GetKey(KeyCode.Space); // Space кнопка прыжка
        Spriting = Input.GetKey(KeyCode.LeftShift); // LeftShift кнопка ускорения
        PauseMenu = Input.GetKeyDown(KeyCode.Escape);

        //Debug.Log(Horizontal); 
        //Debug.Log(Vertival); 
        //Debug.Log(Jump);
        //Debug.Log(Spriting);
        //Attack = Input.GetAxis(InputKey.Fire1.ToString());
        movement = new Vector3(Horizontal, 0.0f, Vertival).normalized; // сдесь вывести это значение в инпут плеере
    }
}
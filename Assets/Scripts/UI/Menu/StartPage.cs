namespace UI.Menu
{
    public class StartPage : BasePage
    {
        private void Awake()
        {
            Opened += () => SceneSwitcher.instance.LoadScene(1);
        }
    }
}
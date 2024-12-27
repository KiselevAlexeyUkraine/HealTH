namespace UI.Menu
{
    public class StartPage : BasePage
    {
        private void Awake()
        {
            Opened += () => SceneSwitcher.Instance.LoadScene(1);
        }
    }
}
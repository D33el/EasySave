using EasySave.Views;


namespace EasySave
{
    class Program
    {
        public static void Main(string[] args)
        {
            Config _config = Config.GetConfig();
            View _view = new();

            bool configExists = _config.CheckConfig();

            if (!configExists)
            {
                View.ShowFirstLaunchMenu();
                _view.SetParameters(1);
                _config.SaveConfig();
                _config.LoadConfig();
            }
            _view.ShowMainMenu();
        }
    }
}

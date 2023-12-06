using EasySave.Views;


namespace EasySave
{
    class Program
    {
        public static void Main(string[] args)
        {
            Config configObj = Config.getConfig();
            View view = new();

            bool configExists = configObj.CheckConfig();

            if (!configExists)
            {
                View.ShowFirstLaunchMenu();
                view.SetParameters(1);
                configObj.SaveConfig();
                configObj.LoadConfig();
            }
            view.ShowMainMenu();
        }
    }
}

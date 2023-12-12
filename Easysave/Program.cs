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

        public static string FormatFileSize(long sizeInBytes)
        {
            string[] sizeSuffixes = { "B", "KB", "MB", "GB" };

            int i = 0;
            double size = sizeInBytes;

            while (size >= 1024 && i < sizeSuffixes.Length - 1)
            {
                size /= 1024;
                i++;
            }

            return $"{size:N2} {sizeSuffixes[i]}";
        }
    }
}

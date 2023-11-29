using System;
using EasySave.ViewModels;

namespace EasySave.Views
{
    public class View
    {
        ViewModel viewModel = new ViewModel();

        public void ShowSaveInfo(int saveNb)
        {
            String[] saveList = viewModel.GetSaveInfo(saveNb);
        }
        public static void ShowSaveList()
        {
            Console.WriteLine("list");
        }
        public void ShowSaveError() { }
        public void ShowSaveProgress() { }
        public void GetSaveInput() { }
        public void GetParametersInput(int step)
        {
            
        }

        public bool ValidateInput(string inputType, string input)
        {
            if (inputType.Length == 0)
            {
                Console.WriteLine("No input given");
                return false;
            }
            switch (inputType)
            {
                case "navigation":

                    break;

                case "SaveInfo":

                    break;

                case "parameters":

                    break;

                default:
                    break;
            }

            return true;
        }
    }
}
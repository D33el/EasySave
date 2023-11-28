using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using EasySave.Views;
using EasySave.ViewModels;


namespace EasySave
{
    class Program
    {
        public static void Main(string[] args)
        {

            Config obj = getConfig();
            int check = obj.checkConfig();
            if(check == 0 )
            {
                obj.SaveConfig();
            }
            else
            {
                Console.WriteLine("hello");
            }


        }
    }
}


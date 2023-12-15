using System;
using System.Text.Json;

namespace EasySave.Models
{
    public sealed class AccessList
    {
        public string[]? EncryptableFiles { get; set; }
        public string[]? IgnoredFiles { get; set; }

        private static string FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/Assets/accessLists.json";
        private static AccessList Instance;

        private AccessList()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                using StreamWriter sw = File.CreateText(FilePath);
                sw.Write("{}");
            }
        }

        public static AccessList GetAccessList()
        {
            Instance ??= new AccessList();
            Instance.LoadAccessList();
            return Instance;
        }

        public void WriteList()
        {
            try
            {
                AccessListData accessLists = new AccessListData();
                accessLists.EncryptableFiles = EncryptableFiles;
                accessLists.IgnoredFiles = IgnoredFiles;

                string serializedJSON = JsonSerializer.Serialize(accessLists) + Environment.NewLine;
                File.WriteAllText(FilePath, serializedJSON);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool FileIsInList(string type, FileInfo file)
        {
            string fileExt = file.Extension;
            if (type == "ignored")
            {
                if (IgnoredFiles.Contains(fileExt)) { return true; }
            }
            else if (type == "encryptable")
            {
                if (EncryptableFiles.Contains(fileExt)) { return true; }
            }
            return false;
        }

        private void LoadAccessList()
        {
            string JSONtext = File.ReadAllText(FilePath);
            try
            {
                AccessListData accessList = JsonSerializer.Deserialize<AccessListData>(JSONtext);

                EncryptableFiles = accessList.EncryptableFiles;
                IgnoredFiles = accessList.IgnoredFiles;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private class AccessListData
        {
            public string[]? EncryptableFiles { get; set; }
            public string[]? IgnoredFiles { get; set; }
        }
    }
}


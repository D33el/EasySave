using System;
namespace EasySave.Views
{
    public class Navigation : View
    {
        public void ShowParameters()
        {
            string lang = "";

            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|=========================== Paramétres ==============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Changer la langue                                               |");
                Console.WriteLine("| [2] Configurer l'emplacement dossier de destination des sauvegardes |");
                Console.WriteLine("| [3] Configurer l'emplacement du fichier de log                      |");
                Console.WriteLine("| [4] Configurer l'emplacement du fichier state                       |");
                Console.WriteLine("|=====================================================================|");
            }
            else
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|============================ Settings ===============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Change language                                                 |");
                Console.WriteLine("| [2] Set backup destination path                                     |");
                Console.WriteLine("| [3] Set log file path                                               |");
                Console.WriteLine("| [4] Set state file path                                             |");
                Console.WriteLine("|=====================================================================|");
            }
        }
        public void ShowMainMenu()
        {
            string lang = "";

            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("|==(EasySave V1.0 / Console)==========================================|");
                Console.WriteLine("|===================== Bienvenue sur EasySave ========================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Afficher les sauvegardes                                        |");
                Console.WriteLine("| [2] Créer une nouvelle sauvegarde                                   |");
                Console.WriteLine("| [3] Supprimer une sauvegarde                                        |");
                Console.WriteLine("| [4] Paramétres                                                      |");
                Console.WriteLine("| [5] Quitter                                                         |");
                Console.WriteLine("|=====================================================================|");
            }
            else
            {
                Console.WriteLine("|==(EasySave V1.0 / Console)==========================================|");
                Console.WriteLine("|======================= Welcome to EasySave =========================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Show backups                                                    |");
                Console.WriteLine("| [2] Create Backup                                                   |");
                Console.WriteLine("| [3] Delete Backup                                                   |");
                Console.WriteLine("| [4] Settings                                                        |");
                Console.WriteLine("| [5] Exit                                                            |");
                Console.WriteLine("|=====================================================================|");
            }

        }
        public void ShowSaveMenu(int step, string type)
        {
            string lang = "";

            Console.Clear();

            if (type == "")
            {
                if (lang == "fr")
                {
                    Console.WriteLine("==== Etape 1");
                    Console.WriteLine("==== Veuillez saisir le type de sauvegarde");
                    Console.WriteLine("==== [1] Complete");
                    Console.WriteLine("==== [2] Différentielle");
                }
                else
                {
                    Console.WriteLine("==== Step 1");
                    Console.WriteLine("==== Please enter the type of backup");
                    Console.WriteLine("==== [1] Full");
                    Console.WriteLine("==== [2] Differential");
                }
            }
            else
            {
                if (type == "full")
                {
                    switch (step)
                    {
                        case 1: // Save name
                            if (lang == "fr")
                            {
                                Console.WriteLine("==== Etape 2 sur 3");
                                Console.WriteLine("==== Veuillez saisir le nom de la sauvegarde :");
                            }
                            else
                            {
                                Console.WriteLine("==== Step 2 of 3");
                                Console.WriteLine("==== Please enter the name of the backup :");
                            }
                            break;

                        case 2: // Save source path
                            if (lang == "fr")
                            {
                                Console.WriteLine("==== Etape 3 sur 3");
                                Console.WriteLine("==== Veuillez saisir le chemin source des fichiers :");
                            }
                            else
                            {
                                Console.WriteLine("==== Step 3 of 3");
                                Console.WriteLine("==== Please enter the source path of the files :");
                            }
                            break;

                        default:
                            Console.WriteLine("Error : Unrecognized step");
                            break;
                    }
                }
                else // differential backup
                {
                    switch (step)
                    {
                        case 1: // select save
                            if (lang == "fr")
                            {
                                Console.WriteLine("==== Etape 2 sur 3");
                                Console.WriteLine("==== Veuillez selectionner la sauvegarde a modifier :");
                            }
                            else
                            {
                                Console.WriteLine("==== Step 2 of 3");
                                Console.WriteLine("==== Please select the save to be changed :");
                            }
                            ShowSaveList();
                            break;
                        case 2: // Save source path
                            if (lang == "fr")
                            {
                                Console.WriteLine("==== Etape 3 sur 3");
                                Console.WriteLine("==== Veuillez saisir le chemin source des fichiers :");
                            }
                            else
                            {
                                Console.WriteLine("==== Step 3 of 3");
                                Console.WriteLine("==== Please enter the source path of the files :");
                            }
                            break;

                        default:
                            Console.WriteLine("Error : Unrecognized step");
                            break;
                    }
                }

            }
        }
        public void ShowDeleteMenu(int step)
        {
            string lang = "";

            Console.Clear();

            switch (step)
            {
                case 1:
                    if (lang == "fr")
                    {
                        Console.WriteLine("==== Etape 1 sur 2");
                        Console.WriteLine("==== Veuillez selectionner la sauvegarde a supprimer");
                    }
                    else
                    {
                        Console.WriteLine("==== Step 1 of 2");
                        Console.WriteLine("==== Please select the save to delete");
                    }
                    ShowSaveList();
                    break;
                case 2:
                    if (lang == "fr")
                    {
                        Console.WriteLine("==== Etape 2 sur 2");
                        Console.WriteLine("==== Êtes-vous sûr de vouloir supprimer cette sauvegarde ? (O / N)");
                    }
                    else
                    {
                        Console.WriteLine("==== Step 1 of 2");
                        Console.WriteLine("==== Are you sure you want to delete this backup ? (Y / N)");
                    }
                    break;

                default:
                    Console.WriteLine("Error : Unrecognized step");
                    break;
            }

        }
    }
}


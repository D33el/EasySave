using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace easyesaveVF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> choices = new List<string>();
            choices.Add("Francais");
            choices.Add("Anglais");


            // Set the ComboBox's ItemsSource to the list of choices
            LangueComboBox.ItemsSource = choices;
        }
        private void LangueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LangueComboBox.SelectedItem != null)
            {
                TextBlock langText = (TextBlock)FindName("LanguageText");

                if (langText != null)
                {
                    string selectedLanguage = LangueComboBox.SelectedItem.ToString();
                    switch (selectedLanguage)
                    {
                        case "English":
                            langText.Text = "Language:";
                            break;
                        case "French":
                            langText.Text = "Langage :";
                            break;
                        default:
                            break;
                    }
                }  
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string enteredText = textBox.Text;

            // Update your data using the entered text
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            StackPanel parentPanel = (StackPanel)clickedButton.Parent;
            TextBox textBox = (TextBox)parentPanel.Children[0];

            string enteredText = textBox.Text;

            // Perform actions based on the clicked button and entered text
        }



        private void Continuer_Click(object sender, RoutedEventArgs e)
        {
            // Your code to handle the "Continuer" button click event
        }
    }

}






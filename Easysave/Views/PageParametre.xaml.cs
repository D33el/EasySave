using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for PageParametre.xaml
    /// </summary>
    public partial class PageParametre : Page
    {
        public PageParametre()
        {
            InitializeComponent();
        }


        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic for the "Enregistrer" button click event
        }

        private void AAjouter_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            StackPanel parentPanel = (StackPanel)clickedButton.Parent;
            TextBox textBox = (TextBox)parentPanel.Children[0];

            string enteredText = textBox.Text;

            // Perform actions based on the clicked button and entered text
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string enteredText = textBox.Text;

            // Update your data using the entered text
        }
    
}
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using IMGSeed.Presentation.ViewModels;
using IMGSeed.Presentation.Domain.Models;
using IMGSeed.Application.Services;
using IMGSeed.Domain.Models;

namespace IMGSeed.Presentation
{
    public partial class MainWindow : Window
    {
        private ArtworkViewModel _viewModel;
        private Artwork _artwork;

        private string? _imagePath;
        private Brush _backgroundColor;
        private Brush _textColor;
        private int _scale = 1;
        private int _size = 2;

        public MainWindow()
        {
            InitializeComponent();

            _artwork = new Artwork();
            CharacterMap characterMap = new CharacterMap();
            ArtworkDrawerService drawerService = new ArtworkDrawerService(characterMap);

            _viewModel = new ArtworkViewModel(_artwork, drawerService, _scale);
            DataContext = _viewModel;

            AvaibalableColors();

            backgroundColors.SelectedIndex = 0;
            textColors.SelectedIndex = 1;

            _backgroundColor = (Brush)backgroundColors.SelectedItem;
            _textColor = (Brush)textColors.SelectedItem;

            scaleValue.Text = _scale.ToString();
            sizeValue.Text = _size.ToString();
        }

        private void selectImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.FileName = "";
            dialog.DefaultExt = "";
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files(*.*)|*.*";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                _imagePath = dialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(_imagePath);
                bitmapImage.EndInit();

                originalImage.Source = bitmapImage;
            }
        }

        private void generateTextImage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_imagePath))
            {
                _viewModel.GenerateArt(_imagePath);
            }
        }

        private void AvaibalableColors()
        {
            List<Brush> colors = new List<Brush>()
            {
                Brushes.Black,
                Brushes.White,
            };

            backgroundColors.ItemsSource = colors;
            textColors.ItemsSource = colors;
        }
        private void scaleValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateTextBox(scaleValue.Text, scaleValue))
            {
                _scale = int.Parse(scaleValue.Text);
                _viewModel.SetScale(_scale);
            }
            
        }

        private void sizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateTextBox(sizeValue.Text, sizeValue))
            {
                _size = int.Parse(sizeValue.Text);
                _viewModel.SetSize(_size);
            }
        }

        private void backgroundColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _backgroundColor = (Brush)backgroundColors.SelectedItem;
            txtImg.Background = _backgroundColor;
        }

        private void textColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _textColor = (Brush)textColors.SelectedItem;
            txtImg.Foreground = _textColor;
        }

        private bool ValidateTextBox(string str, TextBox textBox)
        {
            if (str.Length < 0 || str.Length > 1)
            {
                textBox.Text = "2";
            }
            else
            {
                if (!string.IsNullOrEmpty(str) && str.All(char.IsDigit))
                {
                    textBox.Text = str;
                    return true;
                }
                else
                {
                    textBox.Text = "";
                }
            }

            return false;
        }
    }    
}
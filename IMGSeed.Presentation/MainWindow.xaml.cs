using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Resources;
using System.Windows.Media;

namespace IMGSeed.Presentation
{
    public partial class MainWindow : Window
    {
        private string? _imagePath;
        private Brush _backgroundColor;
        private Brush _textColor;
        private int _scale = 1;
        private int _size = 2;

        public MainWindow()
        {
            InitializeComponent();
            AvaibalableColors();

            backgroundColors.SelectedIndex = 0;
            textColors.SelectedIndex = 1;

            _backgroundColor = (Brush)backgroundColors.SelectedItem;
            _textColor = (Brush)textColors.SelectedItem;

            scaleValue.Text = _scale.ToString();
            sizeValue.Text = _size.ToString();

            txtImgWrapper.Height = 0;
            txtImgWrapper.Width = 0;
        }

        private void Generate(BitmapImage bitmapImage)
        {
            txtImg.Background = _backgroundColor;
            txtImg.Foreground = _textColor;

            StringBuilder str = new StringBuilder();

            if (bitmapImage != null)
            {
                int width = bitmapImage.PixelWidth;
                int height = bitmapImage.PixelHeight;

                int stride = width * ((bitmapImage.Format.BitsPerPixel + 7) / 8);
                byte[] pixelData = new byte[height * stride];

                bitmapImage.CopyPixels(pixelData, stride, 0);

                for (int y = 0; y < height; y+=_scale)
                {
                    if (y != 0 || y != height)
                    {
                        str.Append('\n');
                    }

                    for (int x = 0; x < width; x+=_scale)
                    {
                        int index = y * stride + x * ((bitmapImage.Format.BitsPerPixel + 7) / 8);

                        byte blue = pixelData[index];
                        byte green = pixelData[index + 1];
                        byte red = pixelData[index + 2];

                        double brightness = GetPixelBrightness(red, green, blue);

                        if (brightness >= 0 && brightness < 0.1)
                        {
                            str.Append('@');
                        }
                        else if (brightness >= 0.1 && brightness < 0.2)
                        {
                            str.Append("8");
                        }
                        else if(brightness >= 0.2 && brightness < 0.3)
                        {
                            str.Append("O");
                        }
                        else if(brightness >= 0.3 && brightness < 0.4)
                        {
                            str.Append("*");
                        }
                        else if(brightness >= 0.4 && brightness < 0.5)
                        {
                            str.Append("!");
                        }
                        else if(brightness >= 0.5 && brightness < 0.6)
                        {
                            str.Append("=");
                        }
                        else if(brightness >= 0.6 && brightness < 0.7)
                        {
                            str.Append(">");
                        }
                        else if(brightness >= 0.7 && brightness < 0.8)
                        {
                            str.Append("}");
                        }
                        else if(brightness >= 0.8 && brightness < 0.9)
                        {
                            str.Append("~");
                        }
                        else if(brightness >= 0.9 && brightness < 0.95)
                        {
                            str.Append(";");
                        }
                        else if(brightness > 0.95 && brightness <= 1)
                        {
                            str.Append(".");
                        }
                    }
                }

                Write(str.ToString(), height, width);
            }
            
        }

        private double GetPixelBrightness(byte red, byte green, byte blue)
        {
            double brightness = (0.2126 * red + 0.7152 * green + 0.0722 * blue) / 255.0;
            return brightness;
        }

        private void Write(string str, int height, int width)
        {
            txtImg.Text = str;
            txtImgWrapper.Width = width * _size;
            txtImgWrapper.Height = height * _size;

            if (txtImgWrapper.Width > 1920)
            {
                txtImgWrapper.Width = 1920;
            }
            if (txtImgWrapper.Height > 1080)
            {
                txtImgWrapper.Height = 1080;
            }
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
                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(_imagePath);
                bitmapImage.EndInit();

                Generate(bitmapImage);
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
            }
            
        }

        private void sizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateTextBox(sizeValue.Text, sizeValue))
            {
                _size = int.Parse(sizeValue.Text);
            }
        }

        private void backgroundColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _backgroundColor = (Brush)backgroundColors.SelectedItem;
        }

        private void textColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _textColor = (Brush)textColors.SelectedItem;
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
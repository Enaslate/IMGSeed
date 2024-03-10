using System.Windows.Media.Imaging;

namespace IMGSeed.Presentation.Domain.Models
{
    public class Artwork
    {
        public BitmapImage? Bitmap;
        public byte[]? PixelData { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Stride { get; private set; }
        public int BitsPerPixelRounded { get; private set; }

        public Artwork() { }

        public Artwork(string bitmap)
        {
            SetBitmap(bitmap);
        }

        public Artwork(BitmapImage bitmap)
        {
            Bitmap = bitmap;
            Init();
        }

        private void Init()
        {
            if (Bitmap != null)
            {
                Width = Bitmap.PixelWidth;
                Height = Bitmap.PixelHeight;

                BitsPerPixelRounded = (Bitmap.Format.BitsPerPixel + 7) / 8;

                Stride = Width * BitsPerPixelRounded;
                PixelData = new byte[Height * Stride];

                Bitmap.CopyPixels(PixelData, Stride, 0);
            }
        }

        public void SetBitmap(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                Bitmap = new BitmapImage();
                Bitmap.BeginInit();
                Bitmap.UriSource = new Uri(imagePath);
                Bitmap.EndInit();
                Init();
            }
        }
    }
}

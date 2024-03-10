using IMGSeed.Domain.Models;
using IMGSeed.Presentation.Domain.Models;
using System.Text;
using System.Windows.Media.Imaging;

namespace IMGSeed.Application.Services
{
    public class ArtworkDrawerService
    {
        private Artwork? _artwork;
        private CharacterMap _characterMap;

        public ArtworkDrawerService(CharacterMap characterMap)
        {
            _characterMap = characterMap;
        }

        public ArtworkDrawerService(Artwork artwork, CharacterMap characterMap)
        {
            _artwork = artwork;
            _characterMap = characterMap;
        }

        public string DrawArt(Artwork artwork, int scale)
        {
            return DrawArt(artwork.PixelData, artwork.Height, artwork.Width,
                artwork.Stride, artwork.BitsPerPixelRounded, scale);
        }

        public string DrawArt(byte[] pixelData, int height, int width, int stride, int bitsPerPixelRounded, int scale)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (pixelData != null)
            {
                for (int y = 0; y < height; y += scale)
                {
                    if (y != 0)
                    {
                        stringBuilder.Append('\n');
                    }

                    for (int x = 0; x < width; x += scale)
                    {
                        int index = y * stride + x * bitsPerPixelRounded;

                        byte blue = pixelData[index];
                        byte green = pixelData[index + 1];
                        byte red = pixelData[index + 2];

                        double brightness = GetPixelBrightness(red, green, blue);

                        foreach (var symbol in _characterMap.ArtworkSymbols)
                        {
                            if (brightness <= symbol.Key)
                            {
                                stringBuilder.Append(symbol.Value);
                                break;
                            }
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private double GetPixelBrightness(byte red, byte green, byte blue)
        {
            double brightness = (0.2126 * red + 0.7152 * green + 0.0722 * blue) / 255.0;
            return brightness;
        }
    }
}

using IMGSeed.Application.Services;
using IMGSeed.Presentation.Domain.Models;
using System.ComponentModel;

namespace IMGSeed.Presentation.ViewModels
{
    public class ArtworkViewModel : INotifyPropertyChanged
    {
        public string? Art { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        private int Scale = 1;
        private int Size = 2;

        private Artwork _artwork;
        private ArtworkDrawerService _drawerService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ArtworkViewModel(Artwork artwork, ArtworkDrawerService drawerService, int scale)
        {
            _artwork = artwork;
            _drawerService = drawerService;
            Scale = scale;

            GenerateArt();
        }

        public void GenerateArt(string imagePath)
        {
            _artwork.SetBitmap(imagePath);
            GenerateArt();
        }

        private void GenerateArt()
        {
            Art = _drawerService.DrawArt(_artwork, Scale);
            UpdateResolution();
            OnPropertyChanged(nameof(Art));
        }

        private void UpdateResolution()
        {
            Height = _artwork.Height * Size;
            OnPropertyChanged(nameof(Height));

            Width = _artwork.Width * Size;
            OnPropertyChanged(nameof(Width));
        }

        public void SetScale(int scale)
        {
            Scale = scale;
            GenerateArt();
        }

        public void SetSize(int size)
        {
            Size = size;
            GenerateArt();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

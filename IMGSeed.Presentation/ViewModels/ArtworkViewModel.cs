using IMGSeed.Application.Services;
using IMGSeed.Presentation.Domain.Models;
using System.ComponentModel;

namespace IMGSeed.Presentation.ViewModels
{
    public class ArtworkViewModel : INotifyPropertyChanged
    {
        public string? Art { get; set; }
        public int Scale { get; set; }

        private Artwork _artwork;
        private ArtworkDrawerService _drawerService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ArtworkViewModel(Artwork artwork, ArtworkDrawerService drawerService, int scale)
        {
            _artwork = artwork;
            _drawerService = drawerService;

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
            OnPropertyChanged(nameof(Art));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

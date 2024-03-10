namespace IMGSeed.Domain.Models
{
    public class CharacterMap
    {
        public Dictionary<double, char> ArtworkSymbols {  get; private set; }

        public CharacterMap() 
        { 
            ArtworkSymbols = new Dictionary<double, char>
            {
                { 0.1, '@' },
                { 0.2, '8' },
                { 0.3, 'O' },
                { 0.4, '*' },
                { 0.5, '!' },
                { 0.6, '=' },
                { 0.7, '>' },
                { 0.8, '}' },
                { 0.9, '~' },
                { 0.95, ';' },
                { 1, '.' },
            };
        }
    }
}

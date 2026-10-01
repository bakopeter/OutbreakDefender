using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OutbreakDefender
{
    public class Varos : INotifyPropertyChanged
    {
        // háttérmezők a változtatható adatokhoz
        private int _fertozottseg;
        private int _vedelem;
        private bool _karanten;

        public string Nev {  get; }
        public int Lakossag { get; }
        public int Fertozottseg 
        { 
            get => _fertozottseg; 
            set
            {
                _fertozottseg = Math.Clamp(value, 0, 100);
                OnPropertyChanged();
            } 
        } // 0-100%
        public int Vedelem 
        { 
            get => _vedelem; 
            set
            {
                _vedelem = Math.Clamp(value, 0, 3);
                OnPropertyChanged();
            } 
        } // 0-3
        public bool Karanten 
        {  
            get => _karanten; 
            set
            {
                _karanten = value;
                OnPropertyChanged();
            } 
        }

        public Varos(string nev, int lakossag, int fertozottseg)
        {
            Nev = nev;
            Lakossag = lakossag;
            this._fertozottseg = Math.Clamp(fertozottseg, 0, 100);
        }

        public override string ToString()
        {
            return $"{Nev} - Fertőzöttség: {Fertozottseg}%";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? nev = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nev));
        }
    }
}

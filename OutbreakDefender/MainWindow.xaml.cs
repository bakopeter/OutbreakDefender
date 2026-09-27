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
using System.Collections.ObjectModel;

namespace OutbreakDefender
{
    /// <summary>
    /// A játék főablaka: itt van a játék teljes állapota és logikája.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ========================= Játékállapot =========================

        /// <summary>A városok gyűjteménye – a DataGrid adatforrása.</summary>
        ObservableCollection<Varos> varosok = new ObservableCollection<Varos>();

        /// <summary>Véletlenszám-generátor.</summary>
        Random rnd = new Random();

        int kor = 1;                 // aktuális kör száma
        int akciopont = 10;          // rendelkezésre álló akciópont
        bool jatekVege = false;      // vége van-e a játéknak
        int pontszam = 0;            // a játék végén számolt pontszám

        // ========================= Konstruktor =========================
        public MainWindow()
        {
            InitializeComponent();
            dgVarosok.ItemsSource = varosok; // adatforrás bekötése 
            VarosokLetrehozasa();
        }

        // ========================= 3. feladat: városok létrehozása =========================

        /// <summary>Létrehozza a kezdő városokat (6 db, különféle típusokkal).</summary>
        private void VarosokLetrehozasa()
        {
            varosok.Clear();
            varosok.Add(new Varos("Novapolis", 850_000, 12));
            varosok.Add(new Varos("Greenhill", 320_000, 5));
            varosok.Add(new Varos("Iron City", 1_200_000, 23));
            varosok.Add(new Varos("Riverside", 470_000, 8));
            varosok.Add(new Varos("Northpoint", 610_000, 14));
            varosok.Add(new Varos("Sunset Bay", 900_000, 18));
        }

        // ========================= 5. feladat: következő kör =========================

        /// <summary>A "Következő kör" gomb eseménykezelője.</summary>
        private void BtnKovetkezoKor_Click(object sender, RoutedEventArgs e)
        {
            // 1. növekszik a kör száma
            kor++;

            // 2. a játékos új akciópontokat kap
            akciopont += 10; //minden körben új akciópontok
            EsemenyNaplo($"---{ kor}. kör-- - "); 

            FertozesTerjedese();

            // 5. felület frissítése, majd győzelem/vereség ellenőrzése
            FeluletFrissites();
        }

        private void FertozesTerjedese()
        {
            foreach (Varos v in varosok)
            {
                if (v.Fertozottseg <= 0) continue; // csak a fertőzött város terjed

                int elozo = v.Fertozottseg;
                int novekedes = v.Karanten
                    ? rnd.Next(0, 4)            // karanténban: 0–3%
                    : rnd.Next(3, 11);          // normálisan: 3–10%

                int tenyleges = Math.Max(0, novekedes - v.Vedelem * 2);
                v.Fertozottseg = Math.Min(100, v.Fertozottseg + tenyleges);

                EsemenyNaplo($"{v.Nev}: {elozo}% -> {v.Fertozottseg}%");
            }
        }

        private Varos? KivalasztottVaros()
        {
            Varos? kivalasztott = dgVarosok.SelectedItem as Varos;
            if (kivalasztott == null)
            {
                MessageBox.Show("Válassz ki egy várost!");
            }
            return kivalasztott;
        }

        private void BtnKezeles_Click(object sender, RoutedEventArgs e)
        {
            Varos? kivalasztott = KivalasztottVaros();
            if (kivalasztott == null) return;

            if (akciopont < 3)
            {
                MessageBox.Show("Nincs elegendő akciópontod!");
                return;
            }

            akciopont -= 3;
            kivalasztott.Fertozottseg = Math.Max(0, kivalasztott.Fertozottseg - 15);

            EsemenyNaplo($"{ kivalasztott.Nev} kezelést kapott. " 
                + $"Fertőzöttség:{ kivalasztott.Fertozottseg}% "
);          FeluletFrissites();
        }

        private void EsemenyNaplo(string uzenet)
        {
            // Ezt a 9. lépésben töltjük fel.
        }

        /// <summary>Frissíti a statisztikákat és a kijelölt város adatait.</summary>
        private void FeluletFrissites()
        {
            txtKor.Text = kor.ToString();
            txtAkcioPont.Text = akciopont.ToString();
        }
    }
}
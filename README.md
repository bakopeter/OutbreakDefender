# Outbreak Defender – WPF játékprojekt

## Projektleírás

Készíts egy **Outbreak Defender** nevű, körökre osztott stratégiai játékot **WPF alkalmazásként**.

A játékos feladata, hogy több várost megvédjen egy járványtól. Minden körben terjed a fertőzés, a játékos pedig akciópontokat kap, amelyekből különböző védekezési műveleteket hajthat végre.

---

# A játék alapötlete

A játékban több város szerepel.

Minden város rendelkezik például az alábbi adatokkal:

- név;
- lakosság;
- fertőzöttség;
- védelem;
- karantén állapota.

Példa:

| Város | Lakosság | Fertőzöttség | Védelem | Karantén |
|---|---:|---:|---:|---|
| Novapolis | 850 000 | 12% | 0 | Nem |
| Greenhill | 320 000 | 5% | 0 | Nem |
| Iron City | 1 200 000 | 23% | 0 | Nem |
| Riverside | 470 000 | 8% | 0 | Nem |

Minden körben:

- növekszik a fertőzöttség;
- a játékos akciópontokat kap;
- kiválaszthat egy várost;
- kezelheti a várost;
- karantént rendelhet el;
- fejlesztheti a város védelmét;
- véletlen események történhetnek.

---

# 1. feladat – A WPF felület elkészítése

Készítsd el a játék főablakát XAML segítségével.

Használj legalább az alábbi WPF vezérlőkből:

- `Grid`
- `StackPanel`
- `Border`
- `TextBlock`
- `Button`
- `DataGrid`
- `ListBox`

A fejlécben jelenjen meg:

```text
OUTBREAK DEFENDER
Járványvédelmi stratégiai játék
```

A játék indulásakor:

```text
Kör: 1
Akciópont: 10
```

Legyenek az alábbi gombok:

- **Kezelés**
- **Karantén**
- **Védelem fejlesztése**
- **Következő kör**
- **Új játék**

A fő elrendezéshez használj `Grid`-et és `RowDefinition` / `ColumnDefinition` elemeket.

Például:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>
</Grid>
```

---

# 2. feladat – `Varos` osztály létrehozása

Hozz létre egy `Varos` nevű osztályt külön fájlban.

```csharp
public class Varos
{
    public string Nev { get; set; }
    public int Lakossag { get; set; }
    public int Fertozottseg { get; set; }
    public int Vedelem { get; set; }
    public bool Karanten { get; set; }
}
```

A `Fertozottseg` értéke százalék legyen:

```text
0   = nincs fertőzés
50  = a város fele fertőzött
100 = teljesen fertőzött
```

Készíts konstruktort is.

Írd felül a `ToString()` metódust.

Példa:

```text
Novapolis – Fertőzöttség: 34%
```

---

# 3. feladat – Városok tárolása és DataGrid megjelenítés

A városokat egy:

```csharp
ObservableCollection<Varos>
```

gyűjteményben tárold.

Például:

```csharp
ObservableCollection<Varos> varosok = new ObservableCollection<Varos>();
```

Szükséges névtér:

```csharp
using System.Collections.ObjectModel;
```

A játék indulásakor legyen legalább 6 város:

```text
Novapolis
Greenhill
Iron City
Riverside
Northpoint
Sunset Bay
```

Mindegyik kapjon eltérő lakosságot és 0–25% közötti kezdő fertőzöttséget.

A `DataGrid` adatforrása legyen a városgyűjtemény:

```csharp
dgVarosok.ItemsSource = varosok;
```

A `DataGrid` oszlopai:

- Város
- Lakosság
- Fertőzöttség
- Védelem
- Karantén

A felhasználó ne adhasson hozzá új sort kézzel:

```xml
CanUserAddRows="False"
```

---

# 4. feladat – Fertőzöttség megjelenítése ProgressBar segítségével

A fertőzöttség ne csak számként jelenjen meg.

A `DataGrid` egyik oszlopában használj `ProgressBar` vezérlőt.

Példa:

```xml
<DataGridTemplateColumn Header="Fertőzöttség">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <StackPanel>
                <ProgressBar
                    Minimum="0"
                    Maximum="100"
                    Value="{Binding Fertozottseg}"
                    Height="18"/>

                <TextBlock
                    Text="{Binding Fertozottseg}"
                    HorizontalAlignment="Center"/>
            </StackPanel>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

A cél, hogy a játékos vizuálisan is lássa, melyik város van veszélyben.

---

# 5. feladat – Következő kör és fertőzés terjedése

A **Következő kör** gombra kattintva:

1. növekedjen a kör száma;
2. minden fertőzött város fertőzöttsége növekedjen;
3. a fertőzöttség ne mehessen 100 fölé;
4. frissüljön a felület.

Használj:

```csharp
Random rnd = new Random();
```

Például:

```csharp
int novekedes = rnd.Next(3, 11);
```

Ez 3–10 százalékpontos növekedést jelent.

Példa:

```text
Iron City
48% → 55%
```

A kör számát `TextBlock` jelenítse meg.

Például:

```xml
<TextBlock x:Name="txtKor" Text="Kör: 1"/>
```

---

# 6. feladat – Akciópont-rendszer és kezelés

A játékos a játék elején rendelkezzen:

```text
10 akcióponttal
```

Minden új körben kapjon:

```text
+10 akciópontot
```

A műveletek költsége:

| Művelet | Költség |
|---|---:|
| Kezelés | 3 AP |
| Karantén | 5 AP |
| Védelem fejlesztése | 4 AP |

A **Kezelés** gomb a `DataGrid` kijelölt városának fertőzöttségét csökkentse:

```text
-15 százalékponttal
```

A kijelölt várost például így kérheted le:

```csharp
Varos? kivalasztott = dgVarosok.SelectedItem as Varos;
```

Ha nincs kijelölt város:

```text
Válassz ki egy várost!
```

Ha nincs elég akciópont:

```text
Nincs elegendő akciópontod!
```

A fertőzöttség nem lehet kisebb 0-nál.

Az eseménynaplóba kerüljön:

```text
Iron City kezelést kapott. Fertőzöttség: 52%
```

---

# 7. feladat – Karantén

A **Karantén** gomb a kijelölt várost helyezze karantén alá.

Költsége:

```text
5 akciópont
```

A város:

```csharp
Karanten = true;
```

értéket kapjon.

Normál város fertőzésnövekedése:

```csharp
rnd.Next(3, 11);
```

Karantén alatt:

```csharp
rnd.Next(0, 4);
```

Ha a város már karantén alatt van:

```text
Ez a város már karantén alatt van.
```

Az eseménynapló:

```text
Greenhill karantén alá került.
```

A `DataGrid` karantén oszlopához használhatsz:

```xml
<DataGridCheckBoxColumn
    Header="Karantén"
    Binding="{Binding Karanten}"/>
```

---

# 8. feladat – Városi védelem fejlesztése

A **Védelem fejlesztése** gomb a kijelölt város `Vedelem` értékét növelje.

Költsége:

```text
4 akciópont
```

A védelmi szintek:

```text
0 → 1 → 2 → 3
```

A maximális szint 3.

A védelem csökkentse a körönkénti fertőzésnövekedést:

```text
Védelem 0 → 0 csökkentés
Védelem 1 → -2 százalékpont
Védelem 2 → -4 százalékpont
Védelem 3 → -6 százalékpont
```

Példa:

```text
Véletlenszerű növekedés: +8%
Védelem: 2
Csökkentés: -4%

Tényleges növekedés: +4%
```

A tényleges növekedés ne legyen negatív.

Ha a város védelme már maximális:

```text
A város védelme már maximális.
```

Az eseménynaplóba:

```text
Riverside védelmi szintje 2-re nőtt.
```

---

# 9. feladat – Véletlen események és eseménynapló

Minden kör végén történhessen véletlen esemény.

Például:

```csharp
int esemeny = rnd.Next(0, 4);
```

Készíts legalább 4 különböző eseményt.

## Nemzetközi segítség

```text
Nemzetközi segítség érkezett!
+5 akciópont
```

## Új fertőzési hullám

Egy véletlenszerű város fertőzöttsége nőjön 15 százalékponttal.

```text
Új fertőzési hullám!
Riverside fertőzöttsége 48%-ra nőtt.
```

## Sikeres kutatás

Minden város fertőzöttsége csökkenjen 5 százalékponttal.

```text
Sikeres kutatás!
Minden város fertőzöttsége csökkent.
```

## Védelmi rendszer meghibásodása

Egy véletlenszerű város védelmi szintje csökkenjen eggyel, ha nagyobb 0-nál.

```text
Védelmi rendszer meghibásodott!
Iron City védelmi szintje 2-ről 1-re csökkent.
```

Az eseményeket `ListBox` jelenítse meg.

Példa:

```text
--- 7. kör ---
Új fertőzési hullám!
Riverside fertőzöttsége 48%-ra nőtt.
```

---

# 10. feladat – Győzelem, vereség, pontszám és új játék

## Vereség

A játékos veszít, ha minden város fertőzöttsége eléri a 100%-ot.

```text
JÁTÉK VÉGE

A járvány minden várost elért.
```

A játék gombjait tiltsd le:

```csharp
btnKezeles.IsEnabled = false;
btnKaranten.IsEnabled = false;
btnVedelem.IsEnabled = false;
btnKovetkezoKor.IsEnabled = false;
```

## Győzelem

A játékos győz, ha minden város fertőzöttsége 0%.

```text
GYŐZELEM!

Sikerült megfékezni a járványt.
```

## Pontszám

Használhatod például:

```text
Pontszám =
1000
- körök száma × 20
+ megmaradt akciópont × 5
```

A minimum pontszám 0.

## Új játék

Az **Új játék** gomb:

- állítsa vissza a kör számát 1-re;
- állítsa vissza az akciópontokat 10-re;
- törölje és hozza létre újra a városokat;
- törölje az eseménynaplót;
- engedélyezze újra a gombokat.

---

# WPF-specifikus továbbfejlesztés – `INotifyPropertyChanged`

A `Varos` osztályt később módosítsd úgy, hogy megvalósítsa az:

```csharp
INotifyPropertyChanged
```

interfészt.

Ennek segítségével a WPF automatikusan frissíti a felületet, amikor megváltozik például:

```text
Fertozottseg
Vedelem
Karanten
```

Ez fontos különbség a Windows Forms és a WPF működése között.

---

# Javasolt projektstruktúra

```text
OutbreakDefender
|
+-- App.xaml
+-- App.xaml.cs
+-- MainWindow.xaml
+-- MainWindow.xaml.cs
+-- Varos.cs
```

## `MainWindow.xaml`

Itt legyen:

- a vizuális felület;
- `Grid`;
- `DataGrid`;
- gombok;
- `TextBlock`;
- `ListBox`;
- `ProgressBar`.

## `MainWindow.xaml.cs`

Itt legyen:

- a játék állapota;
- eseménykezelők;
- fertőzés terjedése;
- akciópont-kezelés;
- véletlen események;
- győzelem és vereség ellenőrzése.

## `Varos.cs`

Itt legyen:

- a város adatai;
- konstruktor;
- propertyk;
- `ToString()`;
- opcionálisan `INotifyPropertyChanged`.

---

# Javasolt változók

```csharp
ObservableCollection<Varos> varosok =
    new ObservableCollection<Varos>();

Random rnd = new Random();

int kor = 1;
int akciopont = 10;
bool jatekVege = false;
```

---

# Javasolt saját metódusok

Ne egyetlen hosszú eseménykezelőbe írj mindent.

Érdemes például:

```csharp
private void VarosokLetrehozasa()
{
}
```

```csharp
private void FertozesTerjedese()
{
}
```

```csharp
private void VeletlenEsemeny()
{
}
```

```csharp
private void JatekVegeEllenorzes()
{
}
```

```csharp
private void FeluletFrissites()
{
}
```

```csharp
private Varos? KivalasztottVaros()
{
}
```

```csharp
private void UjJatek()
{
}
```

---

# Pluszfeladatok

## 1. Színezett fertőzöttség

A fertőzöttség alapján változzon a megjelenítés:

```text
0–24%     → alacsony veszély
25–49%    → közepes veszély
50–74%    → magas veszély
75–100%   → kritikus veszély
```

Használj WPF `Style` és `DataTrigger` elemeket.

## 2. DataGrid sorainak színezése

A teljes várossor háttérszíne változzon a fertőzöttség alapján.

## 3. Saját gombstílus

Készíts közös WPF `Style`-t a gombokhoz.

```xml
<Window.Resources>
    <Style TargetType="Button">
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Padding" Value="10"/>
        <Setter Property="FontSize" Value="16"/>
    </Style>
</Window.Resources>
```

## 4. Nehézségi szintek

`ComboBox` segítségével lehessen választani:

```text
Könnyű
Normál
Nehéz
```

A nehézség befolyásolhatja:

- fertőzés növekedését;
- akciópontokat;
- negatív események esélyét.

## 5. Különleges városok

Például:

```text
Kikötőváros
+2% fertőzés körönként
```

```text
Kutatóváros
A kezelés olcsóbb.
```

```text
Erődített város
Kezdő védelem: 1
```

## 6. Mentés és betöltés

Készíts:

```text
Mentés
Betöltés
```

gombokat, és mentsd fájlba:

- kör;
- akciópont;
- városok;
- fertőzöttség;
- védelem;
- karantén.

## 7. Toplista

A játék végén kérd be a játékos nevét.

Mentsd el:

```text
Név
Pontszám
Körök száma
```

A toplistát külön WPF `Window` ablakban jelenítsd meg.

## 8. Saját Game Over ablak

A `MessageBox` helyett készíts külön:

```text
GameOverWindow
```

ablakot, amely megjeleníti:

- győzelem vagy vereség;
- körök számát;
- pontszámot;
- Új játék gombot;
- Kilépés gombot.

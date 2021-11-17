# Image Histogram

## Popis projektu

Cílem projektu je vytvoření aplikace, která měří podobnost vstupního obrázku s databází obrázků na základě
analýzy jejich histogramů barev.

### Další Informace

Obrázek je možno reprezentovat jako množinu bodů a jejich barev. Četnosti jednotlivých barev pak tvoří
histogram. Podobnost obrázků lze definovat tak, že 2 obrázky prohlásíme jako podobné, pokud mají podobnou
barevnost – histogram. Tímto způsobem dokážeme identifikovat podobně barevné obrázky, nicméně vnitřní
sémantika v takové reprezentaci schází (neuvažujeme-li i prostorovou informaci, tj. jaký je vztah z hlediska
barevnosti mezi jednotlivými regiony). Histogramy lze chápat jako n-dimenzionální vektory celých čísel a lze na
nich tedy definovat podobnost. U libovolné ze zvolených měr je třeba také aplikovat nějakou normalizační
techniku pro omezení vlivu velikosti obrázku.




## Způsob řešení

* Aplikace je napsána v C# nad ASP.NET Core a Razor Web Pages. 
* Aplikace závisí na dvou balíčcích třetích stran
  * `MathNet.Numerics` pro práci s vektory a maticemi.
  * `SixLabors.ImageSharp` pro práci s obrazky.
* Aplikaci lze kontejnerizovat (docker).


Aplikace obsahuje tyto části:
* Modul extrakce histogramu (slozka histogram)
  * Lze konfigurovat počet binů. (default je 16)
  * Počítá se histogram pro HSV a RGB reprezentaci barev.
  * Normalizace probíhá přes dělení celkovým počtem pixelů.
* Modul podobnostní míra pro porovnání dvojice obrázků, tj. jejich histogramů (slozka similarity)
  * Implementoval jsem tyto typy měr:
    * KullbackLeiblerDivergence
	* MinkowskiDistance
	* Qfd
* Modul databaze obrazku (slozka database)
  * Aplikace na zacatku nahraje z nakonfigurovane lokace obrazky, spocita jejich histogramy a ulozi do pameti.
* Modul identifikace podobných databázových obrázků vzhledem ke vstupnímu obrázku (slozka evaluation)
  * Aplikace seradi podobnosti dotazovaneho obrazku vuci databazi podle ruznych mer a vrati nejlepsiho kandidata vcetne informaci o delce porovnani.
* Webový interface (slozka Pages)
  * Jedna stranka, kde se vlozi dotazovany obrazek a vrati se kandidatni obrazky vcetne vizualizace.

## Běh programu

Program běží na adrese `http://imagehistogram.northeurope.azurecontainer.io`. 
Zde se dá vše vyzkoušet.
Deploy skript je `containarize.ps1`.

Pokud máte zájem o lokální build, pak se musí udělat tři kroky.
1. Build aplikace
  * `docker build -t image_histogram ImageHistogram/.`
2. Zajištění fotografií pro inicializace databáze. 
  * Je možné se podívat na můj sdílený disk (skripty connectShare pro namountování disku do systému)
3. Spustit aplikaci s namountovanými obrázky ve složce '\images\train'
  * Příklad `docker run -p 1234:80 -v c://Users/karymes/source/repos/ImageHistogram/photos:/images -i image_histogram`
 

## Příklad výstupu
Popis s obrázkem konkrétního vstupu a výstupu aplikace.

## Experimentální sekce
data
o Většina projektů lze posuzovat z hlediska přesnosti či rychlosti (nebo obojího),
přičemž tyto jsou závislé na různých vstupních parametrech projektu. V této sekci by
měly být takové parametry zkoumány. Např. rychlost typicky závisí na velikosti
vstupu nebo naopak velikosti výstupu. Lze pak například do grafu nebo tabulku
vynést takovéto závislosti.

## Diskuze

Toto řešení slouží jako proof of concept hledání podobných obrázků za použití jejich histogramů.
Aplikace by se dala dále rozšiřovat v každé ze svých komponent.

* Extrakce histogramu
  * SIMD, OpenMP zvýšení performance.
* Podobnostní míra
  * Zvýšení performance
  * Další míry
* Databáze obrázků
  * Dynamické přidávání a odebírání
* Identifikace nejpodobnějších
  * Využít jiné metody než pouze porovnání histogramů.
  * Využít možnosti nedělat kompletní průchod databáze, ale approximate, či techniky indexování.
* Webový interface
  * Přidat historii hodnocení
  * Přidat zpětnou uživatelskou referenci

## Závěr

Ukázali jsme si, že podobnost dle histogramů je rychlé přiblížení se k podobnému obrázku. 
Dále jsme si ukázali vliv různých měr na přesnost a rychlost.
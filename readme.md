# NRank - implementacja algorytmu VC-DomLEM

## Uruchamianie

Uruchamianie eksperymentów polega na uruchmieniu aplikacji **nRank.console.exe**. Jako paramter uruchomienia należy podać jeden z plików eksperymentu:

- AirlinesIntCardinal.isf
- grm.isf
- Notebooks.isf
- winequality-red.isf

Drugim parametrem jest próg spójności reguły. Proszę pamiętać o tym, że w Polsce separatorem dziesiętnym jest przecinek...

Wyniki pojawią się w folderach **NAZWA_PLIKU/result.txt**

Przykład poprawnego wywołania na polskim systemie operacyjnym:

**nRank.console.exe AirlinesIntCardinal.isf 0,2**

Reguły pojawią się w:

**AirlinesIntCardinal/rules.txt**

## Struktura pliku z regułami

Przykładowy wiersz:

if (f(AET, x) <= 3) then x E Cl2>= z a = 0

- if (f(AET, x) <= 3) then x E Cl2>= - Wygenerowana reguła
- "x E Cl2>=" - Zapis oznacza x należy do skumulowanej klasy decyzyjnej numer 2 skierowanej ku górze (zapis Cl2<= oznacza klasę skierowaną ku dołowi)
- a = 0 - poziom spójności reguły

## Struktura pliku do liczenia dopasowania modelu do danych

Dodano również generowanie pliku

**AirlinesIntCardinal/predicted.csv**

Jest to plik składający się z 3 kolumn: identyfikatora, wartości zmiennej decyzyjnej w danych wejściowych, przewidzianej wartości zmiennej decyzyjnej przy pomocy modelu. Wartości oddzielone są średnikami.

## Struktura pliku wynikowego

**AirlinesIntCardinal/result.txt**

W pliku result pojawią się wartości błędów uczenia i zbioru testowego (RMSE).

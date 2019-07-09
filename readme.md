# NRank - implementacja algorytmu VC-DomLEM

## Uruchamianie

Uruchamianie eksperymentów polega na uruchmieniu aplikacji **nRank.console.exe**. Jako paramter uruchomienia należy podać jeden z plików eksperymentu:

- AirlinesIntCardinal.isf
- grm.isf
- Notebooks.isf

Drugim parametrem jest próg spójności reguły. Proszę pamiętać o tym, że w Polsce separatorem dziesiętnym jest przecinek...

Wyniki pojawią się w folderach **NAZWA_PLIKU/result.txt**

Przykład poprawnego wywołania na polskim systemie operacyjnym:

**nRank.console.exe AirlinesIntCardinal.isf 0,2**

Wyniki pojawią się w:

**AirlinesIntCardinal/result.txt**

## Struktura pliku wynikowego

Przykładowy wiersz:

if (f(AET, x) <= 3) then x E Cl2>= z a = 0 { 0, 1, 2, 5 }

- if (f(AET, x) <= 3) then x E Cl2>= - Wygenerowana reguła
- "x E Cl2>=" - Zapis oznacza x należy do skumulowanej klasy decyzyjnej numer 2 skierowanej ku górze (zapis Cl2<= oznacza klasę skierowaną ku dołowi)
- a = 0 - poziom spójności reguły
- { 0, 1, 2, 5 } - identyfikatory obiektów pokrytych przez regułę

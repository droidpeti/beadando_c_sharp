# Filmlista kezelő program

Ebben a programban egy txt fájlban tárolt filmek adatait lehet lekérni/kezelni

Az adatokat a filmek.txt fájlban tárolom

A fájlra így hivatkozok: ../../../../filmek.txt, mert olyan helyre szerettem volna tenni a fájlt, amit a githubra is feltölt pusholással (bin/debug/dotnet7.0-át nem tölti fel!!!)

## Funkciók

- [x] Filmlista kiiratása
- [x] Új film feltöltése a fájlba
- [ ] Film keresése a megadott szempont alapján
- [ ] A Fájlban egy film adatai módosítása

## A filmek tárolási szerkezete

### Egy Sorban egy film adatai vannak
Cím;Megjelenés(év);Hossz(Perc);Gyártó;Műfajok(#-el elválasztva)(pl: vígjáték#horror#akció#sci-fi);Szereplők(#-el elválasztva) (pl: Törppapa#Törppilla#Okoska);Forgalmazó
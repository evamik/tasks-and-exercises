## Rezultatai

![Result](https://i.imgur.com/3MI1uKS.jpg)

## Užduotis

Sukurti formą, kurioje įvedus sumą BTC, gyvai rodytų jos atitikmenį USD, EUR, ir GBP valiutomis,
panaudojant JSON API. API: https://api.coindesk.com/v1/bpi/currentprice.json

### Reikalavimai

1. Formoje turi būti įvesties laukas, kuriame vartotojas gali įvesti BTC sumą
2. Formoje turi būti dinaminis (0-3) skaičius laukų, kurie rodys sumos atitikmenį valiutomis.
3. Valiutų laukai gali būti išimti (X/Remove mygtukas)
4. Jei ne visi valiutų laukai matomi, turi būti meniu (dropdown) pridėti papildomą valiutą.
5. Valiutų (išskyrus BTC) laukai turi būti teisingai suformatuoti (pvz $21,198.03)
6. Valiutų kursas ir sumos atitikmenys turi automatiškai atsinaujinti kas minutę.
7. Komponentas turi veikti naršyklėje SPA principu. Galima naudotis bet kokiais įrankiais ar bibliotekomis. JS versija ES2017, bei TC39 Stage 3 proposals.

ARBA

7. Komponentas turi būti parašytas su React, apart jo galima naudoti bet kokius įrankius ar bibliotekas. JS versija ES2017 bei TC39 Stage 3 proposals.

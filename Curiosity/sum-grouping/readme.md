## Užduotis

1. Nuskaityti duomenis .csv formatu. Sukurti naują failą .csv formatu. Naujame faile išsaugoti duomenis lentelės formatu. Lentelėje turi būti suskaičiuota suma grupuojant pagal darbuotoją.
   Atskirame stulpelyje reikia paskaičiuoti mokesčius nuo visos sumos. Mokesčiai - 40%.

Pvz, pagal siųstus duomenis būtų: Zuokas - 100 EUR (20 + 30 + 20 + 30). Mokesčiai 100 \* 40% = 40 EUR

Turi gautis tokia lentelė išvardinant visus darbuotojus:

| Darbuotojas | Suma | Mokesciai |
| ----------- | ---- | --------- |
| Zuokas      | 100  | 40        |

2. Iš tuo pačių duomenų sukurti .csv failą, kuriame būtų suskaičiuotos sumos, grupuojant pagal darbuotoją ir tipą.
   Zuoko atveju turi atsirasti dvi eilutės, viena Priedui (30 + 30) ir viena Algai (20 + 20)

| Darbuotojas | Tipas   | Suma |
| ----------- | ------- | ---- |
| Zuokas      | Priedas | 60   |
| Zuokas      | Alga    | 40   |

## Paleidimas

### Programos paleidimas

Iš terminalo esant _\\sum-grouping\\_ kataloge:

```sh
python main.py
```

##### Terminalo rezultatai:

```sh
python main.py

Executing task...
Saving results...


Executing task...
Saving results...

```

##### rezultatai1.csv:

| Darbuotojas          | Suma | Mokesciai |
| -------------------- | ---- | --------- |
| Dalia Grybauskaitė   | 400  | 160.0     |
| Jonas Jonaitis       | 100  | 40.0      |
| Valdas Adamkus       | 300  | 120.0     |
| Vytautas Landzbergis | 4800 | 1920.0    |
| Zuokas               | 98   | 39.2      |

##### rezultatai2.csv:

| Darbuotojas          | Tipas        | Suma |
| -------------------- | ------------ | ---- |
| Dalia Grybauskaitė   | Atostoginiai | 400  |
| Jonas Jonaitis       | Premija      | 100  |
| Valdas Adamkus       | Alga         | 100  |
| Valdas Adamkus       | Priedas      | 200  |
| Vytautas Landzbergis | Alga         | 4600 |
| Vytautas Landzbergis | Priedas      | 200  |
| Zuokas               | Alga         | 40   |
| Zuokas               | Priedas      | 58   |

---

### Unit-test paleidimas

Iš terminalo esant _\\sum-grouping\\_ kataloge:

```sh
python -m unittest
```

Rezultatas turėtų atrodyti šitaip:

```sh
python -m unittest
......
----------------------------------------------------------------------
Ran 6 tests in 0.004s

OK
```

package edu.ktu.ds.lab3.mikalauskas;

import edu.ktu.ds.lab3.utils.*;

import java.util.Locale;

public class ManualTest {

    public static void main(String[] args) {
        Locale.setDefault(Locale.US); // suvienodiname skaičių formatus
        executeTest();
    }

    public static void executeTest() {
        Bike c1 = new Bike("Cannondale", "Jekyll291", 1997, 500, 1700);
        Bike c2 = new Bike.Builder()
                .make("Cannondale")
                .model("SynapseNeoSE")
                .year(2001)
                .mileage(2000)
                .price(3500)
                .build();
        Bike c3 = new Bike.Builder().buildRandom();
        Bike c4 = new Bike("Colnago Roland 2001 1159 700");
        Bike c5 = new Bike("Colnago Classic 1946 3651 9500");
        Bike c6 = new Bike("Cube   Aerium  2001  364 80.3");
        Bike c7 = new Bike("Diamondback DualSport 2001 1159 7500");

        // Raktų masyvas
        String[] bikesIds = {"TA156", "TA102", "TA178", "TA171", "TA105", "TA106", "TA107", "TA108", "TA109"};
        int id = 0;
        ParsableMap<String, Bike> bikesMap
                = new ParsableHashMap<>(String::new, Bike::new, HashType.DIVISION);

        // Reikšmių masyvas
        Bike[] bikes = {c1, c2, c3, c4, c5, c7, c7, c7};


        for (Bike c : bikes) {
            bikesMap.put(bikesIds[id++], c);
        }
        bikesMap.println("Porų išsidėstymas atvaizdyje pagal raktus");
        /*Ks.oun("Ar egzistuoja pora atvaizdyje?");
        Ks.oun(bikesMap.contains(bikesIds[6]));
        Ks.oun(bikesMap.contains(bikesIds[7]));
        Ks.oun("Pašalinamos poros iš atvaizdžio:");
        Ks.oun(bikesMap.remove(bikesIds[1]));
        Ks.oun(bikesMap.remove(bikesIds[7]));
        bikesMap.println("Porų išsidėstymas atvaizdyje pagal raktus");
        Ks.oun("Atliekame porų paiešką atvaizdyje:");
        Ks.oun(bikesMap.get(bikesIds[2]));
        Ks.oun(bikesMap.get(bikesIds[7]));*/
        Ks.oun("Išspausdiname atvaizdžio poras String eilute:");
        Ks.ounn(bikesMap);


        Ks.oun("Padarome replaceAll(c7, c1)" +
                "\n   c7 - " + c7 +
                "\n   c1 - " + c1);
        bikesMap.replaceAll(c7, c1);
        Ks.ounn(bikesMap);

        Ks.oun("Padarome values()");
        Ks.ounn(bikesMap.values());

        Ks.oun("Padarome putIfAbsent(c6)");
        Ks.ounn(bikesMap.putIfAbsent(bikesIds[id], c6));
        Ks.oun("Padarome putIfAbsent(c6)");
        Ks.ounn(bikesMap.putIfAbsent(bikesIds[id], c6));

        Ks.oun("Padarome numberOfEmpties()");
        Ks.ounn(bikesMap.numberOfEmpties());
        bikesMap.println();
    }
}

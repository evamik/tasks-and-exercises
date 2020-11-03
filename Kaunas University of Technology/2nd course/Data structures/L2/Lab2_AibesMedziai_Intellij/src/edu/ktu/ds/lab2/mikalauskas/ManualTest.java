package edu.ktu.ds.lab2.mikalauskas;

import edu.ktu.ds.lab2.utils.*;

import java.util.Arrays;
import java.util.Collections;
import java.util.Iterator;
import java.util.Locale;

/*
 * Aibės testavimas be Gui
 *
 */
@SuppressWarnings("ALL")
public class ManualTest {

    static Bike[] bikes;
    static ParsableSortedSet<Bike> bSeries = new ParsableBstSet<>(Bike::new, Bike.byPrice);

    public static void main(String[] args) throws CloneNotSupportedException {
        Locale.setDefault(Locale.US); // Suvienodiname skaičių formatus
        executeTest();
    }

    public static void executeTest() throws CloneNotSupportedException {
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
        Bike c8 = new Bike("GT Bikes Grunge 1946 3651 950");
        Bike c9 = new Bike("Tommaso   Forza  2007  364 850.3");

        Bike[] bikesArray = {c9, c7, c8, c5, c1, c6};

        Ks.oun("Dviračių Aibė:");
        ParsableSortedSet<Bike> bikesSet = new ParsableBstSet<>(Bike::new);

        for (Bike c : bikesArray) {
            bikesSet.add(c);
            Ks.oun("Aibė papildoma: " + c + ". Jos dydis: " + bikesSet.size());
        }
        Ks.oun("");
        Ks.oun(bikesSet.toVisualizedString(""));

        ParsableSortedSet<Bike> bikesSetCopy = (ParsableSortedSet<Bike>) bikesSet.clone();

        bikesSetCopy.add(c2);
        bikesSetCopy.add(c3);
        bikesSetCopy.add(c4);
        Ks.oun("Papildyta dviračių aibės kopija:");
        Ks.oun(bikesSetCopy.toVisualizedString(""));

        ParsableSortedSet<Bike> bikesSet2 = new ParsableBstSet<>(Bike::new);
        bikesSet2.addAll((BstSet<? extends Bike>)bikesSet);
        Ks.oun("Papildyta dviračių aibės kopija su addAll:");
        Ks.oun(bikesSet2.toVisualizedString(""));
        Iterator<Bike> iterator = bikesSet2.iterator();
        iterator.next();
        iterator.next();
        iterator.remove();
        Ks.oun(bikesSet2.toVisualizedString(""));

        Ks.oun("c2: " + c2);
        Ks.oun(bikesSet2.lower(c2));
        Ks.oun("c8: " + c8);
        Ks.oun(bikesSet2.lower(c8));

        Ks.oun(bikesSet2.headSet(c7, false));
        Ks.oun(bikesSet2.headSet(c3, true));
        Ks.oun(bikesSet2.higher(c2));
        Ks.oun(bikesSet2.last());
        Ks.oun(bikesSet2.pollLast());
        Ks.oun(bikesSet2.pollLast());
        Ks.oun(bikesSet2.toVisualizedString(""));
        Ks.oun(bikesSet2.floor(c5));
        Ks.oun(bikesSet.toVisualizedString(""));
        bikesSet.removeAll((BstSet<? extends Bike>)bikesSet2);
        Ks.oun(bikesSet.toVisualizedString(""));

        c9.setMileage(10000);

        Ks.oun("Originalas:");
        Ks.ounn(bikesSet.toVisualizedString(""));

        Ks.oun("Ar elementai egzistuoja aibėje?");
        for (Bike c : bikesArray) {
            Ks.oun(c + ": " + bikesSet.contains(c));
        }
        Ks.oun(c2 + ": " + bikesSet.contains(c2));
        Ks.oun(c3 + ": " + bikesSet.contains(c3));
        Ks.oun(c4 + ": " + bikesSet.contains(c4));
        Ks.oun("");

        Ks.oun("Ar elementai egzistuoja aibės kopijoje?");
        for (Bike c : bikesArray) {
            Ks.oun(c + ": " + bikesSetCopy.contains(c));
        }
        Ks.oun(c2 + ": " + bikesSetCopy.contains(c2));
        Ks.oun(c3 + ": " + bikesSetCopy.contains(c3));
        Ks.oun(c4 + ": " + bikesSetCopy.contains(c4));
        Ks.oun("");

        Ks.oun("Elementų šalinimas iš kopijos. Aibės dydis prieš šalinimą:  " + bikesSetCopy.size());
        for (Bike c : new Bike[]{c2, c1, c9, c8, c5, c3, c4, c2, c7, c6, c7, c9}) {
            bikesSetCopy.remove(c);
            Ks.oun("Iš dviračių aibės kopijos pašalinama: " + c + ". Jos dydis: " + bikesSetCopy.size());
        }
        Ks.oun("");

        Ks.oun("Dviračių aibė su iteratoriumi:");
        Ks.oun("");
        for (Bike c : bikesSet) {
            Ks.oun(c);
        }
        Ks.oun("");
        Ks.oun("Dviračių aibė AVL-medyje:");
        ParsableSortedSet<Bike> bikesSetAvl = new ParsableAvlSet<>(Bike::new);
        for (Bike c : bikesArray) {
            bikesSetAvl.add(c);
        }
        Ks.ounn(bikesSetAvl.toVisualizedString(""));

        Ks.oun("Dviračių aibė su iteratoriumi:");
        Ks.oun("");
        for (Bike c : bikesSetAvl) {
            Ks.oun(c);
        }

        Ks.oun("");
        Ks.oun("Dviračių aibė su atvirkštiniu iteratoriumi:");
        Ks.oun("");
        Iterator iter = bikesSetAvl.descendingIterator();
        while (iter.hasNext()) {
            Ks.oun(iter.next());
        }

        Ks.oun("");
        Ks.oun("Dviračių aibės toString() metodas:");
        Ks.ounn(bikesSetAvl);

        // Išvalome ir suformuojame aibes skaitydami iš failo
        bikesSet.clear();
        bikesSetAvl.clear();

        Ks.oun("");
        Ks.oun("Dviračių aibė DP-medyje:");
        bikesSet.load("data\\ban.txt");
        Ks.ounn(bikesSet.toVisualizedString(""));
        Ks.oun("Išsiaiškinkite, kodėl medis augo tik į vieną pusę.");

        Ks.oun("");
        Ks.oun("Dviračių aibė AVL-medyje:");
        bikesSetAvl.load("data\\ban.txt");
        Ks.ounn(bikesSetAvl.toVisualizedString(""));

        Set<String> bikesSet4 = BikeMarket.duplicateBikeMakes(bikesArray);
        Ks.oun("Pasikartojančios dviračių markės:\n" + bikesSet4.toString());

        Set<String> bikesSet5 = BikeMarket.uniqueBikeModels(bikesArray);
        Ks.oun("Unikalūs dviračių modeliai:\n" + bikesSet5.toString());
    }

    static ParsableSortedSet generateSet(int kiekis, int generN) {
        bikes = new Bike[generN];
        for (int i = 0; i < generN; i++) {
            bikes[i] = new Bike.Builder().buildRandom();
        }
        Collections.shuffle(Arrays.asList(bikes));

        bSeries.clear();
        Arrays.stream(bikes).limit(kiekis).forEach(bSeries::add);
        return bSeries;
    }
}

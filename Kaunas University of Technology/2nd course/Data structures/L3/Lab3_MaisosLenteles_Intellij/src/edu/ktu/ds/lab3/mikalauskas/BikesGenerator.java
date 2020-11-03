package edu.ktu.ds.lab3.mikalauskas;


import edu.ktu.ds.lab3.gui.ValidationException;

import java.util.Arrays;
import java.util.Collections;
import java.util.stream.IntStream;
import java.util.stream.Stream;

public class BikesGenerator {

    private static final String ID_CODE = "TA";      //  ***** Nauja
    private static int serNr = 10000;               //  ***** Nauja

    private Bike[] bikes;
    private String[] keys;

    private int currentBikeIndex = 0, currentBikeIdIndex = 0;

    public static Bike[] generateShuffleBikes(int size) {
        Bike[] bikes = IntStream.range(0, size)
                .mapToObj(i -> new Bike.Builder().buildRandom())
                .toArray(Bike[]::new);
        Collections.shuffle(Arrays.asList(bikes));
        return bikes;
    }

    public static String[] generateShuffleIds(int size) {
        String[] keys = IntStream.range(0, size)
                .mapToObj(i -> ID_CODE + (serNr++))
                .toArray(String[]::new);
        Collections.shuffle(Arrays.asList(keys));
        return keys;
    }

    public Bike[] generateShuffleBikesAndIds(int setSize,
                                           int setTakeSize) throws ValidationException {

        if (setTakeSize > setSize) {
            setTakeSize = setSize;
        }
        bikes = generateShuffleBikes(setSize);
        keys = generateShuffleIds(setSize);
        this.currentBikeIndex = setTakeSize;
        return Arrays.copyOf(bikes, setTakeSize);
    }

    // Imamas po vienas elementas iš sugeneruoto masyvo. Kai elementai baigiasi sugeneruojama
    // nuosava situacija ir išmetamas pranešimas.
    public Bike getBike() {
        if (bikes == null) {
            throw new ValidationException("bikesNotGenerated");
        }
        if (currentBikeIndex < bikes.length) {
            return bikes[currentBikeIndex++];
        } else {
            throw new ValidationException("allSetStoredToMap");
        }
    }

    public String getBikeId() {
        if (keys == null) {
            throw new ValidationException("bikesIdsNotGenerated");
        }
        if (currentBikeIdIndex < keys.length) {
            return keys[currentBikeIdIndex++];
        } else {
            throw new ValidationException("allKeysStoredToMap");
        }
    }
}

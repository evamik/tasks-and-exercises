package edu.ktu.ds.lab2.mikalauskas;

import edu.ktu.ds.lab2.utils.BstSet;
import edu.ktu.ds.lab2.utils.Set;

public class BikeMarket {

    public static Set<String> duplicateBikeMakes(Bike[] bikes) {
        Set<Bike> uni = new BstSet<>(Bike.byMake);
        Set<String> duplicates = new BstSet<>();
        for (Bike bike : bikes) {
            int sizeBefore = uni.size();
            uni.add(bike);

            if (sizeBefore == uni.size()) {
                duplicates.add(bike.getMake());
            }
        }
        return duplicates;
    }

    public static Set<String> uniqueBikeModels(Bike[] bikes) {
        Set<String> uniqueModels = new BstSet<>();
        for (Bike bike : bikes) {
            uniqueModels.add(bike.getModel());
        }
        return uniqueModels;
    }
}

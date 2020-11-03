package edu.ktu.ds.lab2.mikalauskas;

import edu.ktu.ds.lab2.utils.Ks;
import edu.ktu.ds.lab2.utils.Parsable;

import java.time.LocalDate;
import java.util.*;

/**
 * @author EK
 */
public final class Bike implements Parsable<Bike> {

    // bendri duomenys visiems automobiliams (visai klasei)
    private static final int minYear = 1990;
    private static final int currentYear = LocalDate.now().getYear();
    private static final double minPrice = 100.0;
    private static final double maxPrice = 333000.0;
    private static final String idCode = "TA";   //  ***** nauja
    private static int serNr = 100;               //  ***** nauja

    private final String bikeRegNr;

    private String make = "";
    private String model = "";
    private int year = -1;
    private int mileage = -1;
    private double price = -1.0;

    public Bike() {
        bikeRegNr = idCode + (serNr++);    // suteikiamas originalus bikeRegNr
    }

    public Bike(String make, String model, int year, int mileage, double price) {
        bikeRegNr = idCode + (serNr++);    // suteikiamas originalus bikeRegNr
        this.make = make;
        this.model = model;
        this.year = year;
        this.mileage = mileage;
        this.price = price;
        validate();
    }

    public Bike(String dataString) {
        bikeRegNr = idCode + (serNr++);    // suteikiamas originalus bikeRegNr
        this.parse(dataString);
        validate();
    }

    public Bike(Builder builder) {
        bikeRegNr = idCode + (serNr++);    // suteikiamas originalus bikeRegNr
        this.make = builder.make;
        this.model = builder.model;
        this.year = builder.year;
        this.mileage = builder.mileage;
        this.price = builder.price;
        validate();
    }

    public Bike create(String dataString) {
        return new Bike(dataString);
    }

    private void validate() {
        String errorType = "";
        if (year < minYear || year > currentYear) {
            errorType = "Netinkami gamybos metai, turi būti ["
                    + minYear + ":" + currentYear + "]";
        }
        if (price < minPrice || price > maxPrice) {
            errorType += " Kaina už leistinų ribų [" + minPrice
                    + ":" + maxPrice + "]";
        }
        
        if (!errorType.isEmpty()) {
            Ks.ern("Dviratis yra blogai sugeneruotas: " + errorType);
        }
    }

    @Override
    public void parse(String dataString) {
        try {   // duomenys, atskirti tarpais
            Scanner scanner = new Scanner(dataString);
            make = scanner.next();
            model = scanner.next();
            year = scanner.nextInt();
            setMileage(scanner.nextInt());
            setPrice(scanner.nextDouble());
        } catch (InputMismatchException e) {
            Ks.ern("Blogas duomenų formatas -> " + dataString);
        } catch (NoSuchElementException e) {
            Ks.ern("Trūksta duomenų -> " + dataString);
        }
    }

    @Override
    public String toString() {  // papildyta su bikeRegNr
        return getBikeRegNr() + "=" + make + "_" + model + ":" + year + " " + getMileage() + " " + String.format("%4.1f", price);
    }

    public String getMake() {
        return make;
    }

    public String getModel() {
        return model;
    }

    public int getYear() {
        return year;
    }

    public int getMileage() {
        return mileage;
    }

    public void setMileage(int mileage) {
        this.mileage = mileage;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public String getBikeRegNr() {  //** nauja.
        return bikeRegNr;
    }

    @Override
    public int compareTo(Bike bike) {
        return getBikeRegNr().compareTo(bike.getBikeRegNr());
    }

    public static Comparator<Bike> byMake = (Bike c1, Bike c2) -> c1.make.compareTo(c2.make); // pradžioje pagal markes, o po to pagal modelius

    public static Comparator<Bike> byPrice = (Bike c1, Bike c2) -> {
        // didėjanti tvarka, pradedant nuo mažiausios
        if (c1.price < c2.price) {
            return -1;
        }
        if (c1.price > c2.price) {
            return +1;
        }
        return 0;
    };

    public static Comparator<Bike> byYearPrice = (Bike c1, Bike c2) -> {
        // metai mažėjančia tvarka, esant vienodiems lyginama kaina
        if (c1.year > c2.year) {
            return +1;
        }
        if (c1.year < c2.year) {
            return -1;
        }
        if (c1.price > c2.price) {
            return +1;
        }
        if (c1.price < c2.price) {
            return -1;
        }
        return 0;
    };

    // Bike klases objektų gamintojas (builder'is)
    public static class Builder {

        private final static Random RANDOM = new Random(1949);  // Atsitiktinių generatorius
        private final static String[][] MODELS = { // galimų automobilių markių ir jų modelių masyvas
            {"Cannondale", "Jekyll291", "SynapseNeoSE", "SuperXForce1"},
            {"Colnago", "Classic", "Roland", "Dorato"},
            {"Cube", "Aerium", "Stereo", "Litening"},
            {"Diamondback", "DualSport", "Comfort", "Cruiser"},
            {"GT Bikes", "Grunge", "Virage", "Avalanche", "Pantera"},
            {"Tommaso", "Forza", "Masso", "Classico"}
        };

        private String make = "";
        private String model = "";
        private int year = -1;
        private int mileage = -1;
        private double price = -1.0;

        public Bike build() {
            return new Bike(this);
        }

        public Bike buildRandom() {
            int ma = RANDOM.nextInt(MODELS.length);        // markės indeksas  0..
            int mo = RANDOM.nextInt(MODELS[ma].length - 1) + 1;// modelio indeksas 1..
            return new Bike(MODELS[ma][0],
                    MODELS[ma][mo],
                    1990 + RANDOM.nextInt(20),// metai tarp 1990 ir 2009
                    500 + RANDOM.nextInt(10000),// rida tarp 6000 ir 228000
                    800 + RANDOM.nextDouble() * 8000);// kaina tarp 800 ir 88800
        }

        public Builder year(int year) {
            this.year = year;
            return this;
        }

        public Builder make(String make) {
            this.make = make;
            return this;
        }

        public Builder model(String model) {
            this.model = model;
            return this;
        }

        public Builder mileage(int mileage) {
            this.mileage = mileage;
            return this;
        }

        public Builder price(double price) {
            this.price = price;
            return this;
        }
    }
}

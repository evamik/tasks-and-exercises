package edu.ktu.ds.lab2.mikalauskas;

import edu.ktu.ds.lab2.gui.ValidationException;

import java.util.Arrays;
import java.util.Collections;
import java.util.stream.IntStream;
import java.util.stream.Stream;

public class BikesGenerator {

    private int startIndex = 0, lastIndex = 0;
    private boolean isStart = true;

    private Bike[] bikes;

    public Bike[] generateShuffle(int setSize,
                                  double shuffleCoef) throws ValidationException {

        return generateShuffle(setSize, setSize, shuffleCoef);
    }

    /**
     * @param setSize
     * @param setTake
     * @param shuffleCoef
     * @return Gražinamas aibesImtis ilgio masyvas
     * @throws ValidationException
     */
    public Bike[] generateShuffle(int setSize,
                                  int setTake,
                                  double shuffleCoef) throws ValidationException {

        Bike[] bikes = IntStream.range(0, setSize)
                .mapToObj(i -> new Bike.Builder().buildRandom())
                .toArray(Bike[]::new);
        return shuffle(bikes, setTake, shuffleCoef);
    }

    public Bike takeBike() throws ValidationException {
        if (lastIndex < startIndex) {
            throw new ValidationException(String.valueOf(lastIndex - startIndex), 4);
        }
        // Vieną kartą Dviratis imamas iš masyvo pradžios, kitą kartą - iš galo.
        isStart = !isStart;
        return bikes[isStart ? startIndex++ : lastIndex--];
    }

    private Bike[] shuffle(Bike[] bikes, int amountToReturn, double shuffleCoef) throws ValidationException {
        if (bikes == null) {
            throw new IllegalArgumentException("Dviračių nėra (null)");
        }
        if (amountToReturn <= 0) {
            throw new ValidationException(String.valueOf(amountToReturn), 1);
        }
        if (bikes.length < amountToReturn) {
            throw new ValidationException(bikes.length + " >= " + amountToReturn, 2);
        }
        if ((shuffleCoef < 0) || (shuffleCoef > 1)) {
            throw new ValidationException(String.valueOf(shuffleCoef), 3);
        }

        int amountToLeave = bikes.length - amountToReturn;
        int startIndex = (int) (amountToLeave * shuffleCoef / 2);

        Bike[] takeToReturn = Arrays.copyOfRange(bikes, startIndex, startIndex + amountToReturn);
        Bike[] takeToLeave = Stream
                .concat(Arrays.stream(Arrays.copyOfRange(bikes, 0, startIndex)),
                        Arrays.stream(Arrays.copyOfRange(bikes, startIndex + amountToReturn, bikes.length)))
                .toArray(Bike[]::new);

        Collections.shuffle(Arrays.asList(takeToReturn)
                .subList(0, (int) (takeToReturn.length * shuffleCoef)));
        Collections.shuffle(Arrays.asList(takeToLeave)
                .subList(0, (int) (takeToLeave.length * shuffleCoef)));

        this.startIndex = 0;
        this.lastIndex = takeToLeave.length - 1;
        this.bikes = takeToLeave;
        return takeToReturn;
    }
}

package edu.ktu.ds.lab2.mikalauskas;

import edu.ktu.ds.lab2.gui.ValidationException;
import edu.ktu.ds.lab2.utils.*;
import edu.ktu.ds.lab2.utils.SortedSet;

import java.util.*;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.Semaphore;

public class SimpleBenchmark {

    public static final String FINISH_COMMAND = "                       ";
    private static final ResourceBundle MESSAGES = ResourceBundle.getBundle("edu.ktu.ds.lab2.gui.messages");

    private static final String[] BENCHMARK_NAMES = {"tree contains", "hash contains", "tree containsAll", "hash containsAll"};
    private static final int[] COUNTS = {100000, 200000, 400000, 800000};

    private final Timekeeper timeKeeper;
    private final String[] errors;

    private final SortedSet<Bike> cSeries = new BstSet<>(Bike.byPrice);
    private final SortedSet<Bike> cSeries2 = new BstSetIterative<>(Bike.byPrice);
    private final SortedSet<Bike> cSeries3 = new AvlSet<>(Bike.byPrice);
    private final TreeSet<Integer> treeSet = new TreeSet<>();
    private final HashSet<Integer> hashSet = new HashSet<>();

    /**
     * For console benchmark
     */
    public SimpleBenchmark() {
        timeKeeper = new Timekeeper(COUNTS);
        errors = new String[]{
                MESSAGES.getString("badSetSize"),
                MESSAGES.getString("badInitialData"),
                MESSAGES.getString("badSetSizes"),
                MESSAGES.getString("badShuffleCoef")
        };
    }

    /**
     * For Gui benchmark
     *
     * @param resultsLogger
     * @param semaphore
     */
    public SimpleBenchmark(BlockingQueue<String> resultsLogger, Semaphore semaphore) {
        semaphore.release();
        timeKeeper = new Timekeeper(COUNTS, resultsLogger, semaphore);
        errors = new String[]{
                MESSAGES.getString("badSetSize"),
                MESSAGES.getString("badInitialData"),
                MESSAGES.getString("badSetSizes"),
                MESSAGES.getString("badShuffleCoef")
        };
    }

    public static void main(String[] args) {
        executeTest();
    }

    public static void executeTest() {
        // suvienodiname skaičių formatus pagal LT lokalę (10-ainis kablelis)
        Locale.setDefault(new Locale("LT"));
        Ks.out("Greitaveikos tyrimas:\n");
        new SimpleBenchmark().startBenchmark();
    }

    public void startBenchmark() {
        try {
            benchmark();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        } catch (Exception ex) {
            ex.printStackTrace(System.out);
        }
    }

    private void benchmark() throws InterruptedException {
        try {
            for (int k : COUNTS) {
                Random rand = new Random();
                int numbers = 100000;
                TreeSet<Integer> treeInts = new TreeSet<>();
                HashSet<Integer> hashInts = new HashSet<>();
                Bike[] bikes = new BikesGenerator().generateShuffle(k, 1.0);
                cSeries.clear();
                cSeries2.clear();

                // testavimo "užsikūrimas" tikslesniems rezultatams
                int integer = rand.nextInt();
                for(int i = 0; i < numbers; i++)
                    hashSet.contains(integer);
                for(int i = 0; i < numbers; i++)
                    treeSet.contains(integer);
                for(int i = 0; i < numbers; i++)
                    treeSet.containsAll(treeInts);
                for(int i = 0; i < numbers; i++)
                    hashSet.containsAll(hashInts);

                timeKeeper.startAfterPause();

                timeKeeper.start();
                //Arrays.stream(bikes).forEach(cSeries::add);
                //timeKeeper.finish(BENCHMARK_NAMES[0]);

                //Arrays.stream(bikes).forEach(cSeries::add);
                //timeKeeper.finish(BENCHMARK_NAMES[1]);

                for(int i = 0; i < k; i++)
                    treeSet.contains(integer);
                timeKeeper.finish(BENCHMARK_NAMES[0]);

                for(int i = 0; i < k; i++)
                    hashSet.contains(integer);
                timeKeeper.finish(BENCHMARK_NAMES[1]);

                for(int i = 0; i < k; i++)
                    treeSet.containsAll(treeInts);
                timeKeeper.finish(BENCHMARK_NAMES[2]);

                for(int i = 0; i < k; i++)
                    hashSet.containsAll(hashInts);

                timeKeeper.finish(BENCHMARK_NAMES[3]);
                timeKeeper.seriesFinish();
            }
            timeKeeper.logResult(FINISH_COMMAND);
        } catch (ValidationException e) {
            if (e.getCode() >= 0 && e.getCode() <= 3) {
                timeKeeper.logResult(errors[e.getCode()] + ": " + e.getMessage());
            } else if (e.getCode() == 4) {
                timeKeeper.logResult(MESSAGES.getString("allSetIsPrinted"));
            } else {
                timeKeeper.logResult(e.getMessage());
            }
        }
    }
}

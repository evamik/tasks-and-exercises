package edu.ktu.ds.lab3.mikalauskas;

import edu.ktu.ds.lab3.demo.Timekeeper;
import edu.ktu.ds.lab3.gui.ValidationException;
import edu.ktu.ds.lab3.utils.HashType;
import edu.ktu.ds.lab3.utils.Ks;
import edu.ktu.ds.lab3.utils.ParsableHashMap;
import edu.ktu.ds.lab3.utils.ParsableMap;

import java.io.*;
import java.util.*;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.Semaphore;

/**
 * @author eimutis
 */
public class SimpleBenchmark {

    public static final String FINISH_COMMAND = "                               ";
    private static final ResourceBundle MESSAGES = ResourceBundle.getBundle("edu.ktu.ds.lab3.gui.messages");

    private final Timekeeper timekeeper;

    private final String[] BENCHMARK_NAMES = {"l3.hm.contains(k)", "java.hm.contains(k)", "l3.hm.contains(v)", "java.hm.contains(v)"};
    private final int[] COUNTS = {1000, 2000, 4000, 8000};

    private final edu.ktu.ds.lab3.utils.Map<String, String> myMap
            = new edu.ktu.ds.lab3.utils.HashMap<>();
    private final Map<String, String> javaMap
            = new HashMap<>();

    /**
     * For console benchmark
     */
    public SimpleBenchmark() {
        timekeeper = new Timekeeper(COUNTS);
    }

    /**
     * For Gui benchmark
     *
     * @param resultsLogger
     * @param semaphore
     */
    public SimpleBenchmark(BlockingQueue<String> resultsLogger, Semaphore semaphore) {
        semaphore.release();
        timekeeper = new Timekeeper(COUNTS, resultsLogger, semaphore);
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

    public void benchmark() throws InterruptedException {
        String[] keys = new String[8000];
        String[] values = new String[8000];
        myMap.clear();
        javaMap.clear();
        Random rand = new Random();
        File file = new File(System.getProperty("user.dir")+"\\data\\zodynas.txt");

        BufferedReader br = null;
        try {
            br = new BufferedReader(new FileReader(file));
            String st;
            int i = 0;
            while((st = br.readLine())!=null && i < 8000){
                keys[i] = st;
                values[i] = st;
                myMap.put(keys[i], values[i]);
                javaMap.put(keys[i], values[i]);
                i++;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }


        List<String> keyList = Arrays.asList(keys);
        Collections.shuffle(keyList);
        keyList.toArray(keys);
        keyList = null;

        List<String> valueList = Arrays.asList(values);
        Collections.shuffle(valueList);
        valueList.toArray(values);
        valueList = null;

        rand.setSeed(2019);
        try {
            for (int k : COUNTS) {
                timekeeper.startAfterPause();
                timekeeper.start();

                for (int i = 0; i < k; i++) {
                    myMap.contains(keys[i]);
                }
                timekeeper.finish(BENCHMARK_NAMES[0]);

                for (int i = 0; i < k; i++) {
                    javaMap.containsKey(keys[i]);
                }
                timekeeper.finish(BENCHMARK_NAMES[1]);


                for (int i = 0; i < k; i++) {
                    myMap.containsVal(values[i]);
                }
                timekeeper.finish(BENCHMARK_NAMES[2]);

                for (int i = 0; i < k; i++) {
                    javaMap.containsValue(values[i]);
                }
                timekeeper.finish(BENCHMARK_NAMES[3]);
                timekeeper.seriesFinish();
            }

            timekeeper.logResult(FINISH_COMMAND);
        } catch (ValidationException e) {
            timekeeper.logResult(e.getMessage());
        }
    }
}

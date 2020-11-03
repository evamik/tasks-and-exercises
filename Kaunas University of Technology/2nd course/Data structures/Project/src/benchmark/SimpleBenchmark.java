package benchmark;

import utils.SparseArray;

import java.util.Random;

public class SimpleBenchmark {

    static final int OPERATION_COUNT = 1_000;
    static final int[] SIZES = {/*64_000,*/ 4_000, 8_000, 16_000, 32_000};

    public static void main(String[] args) {
        runBenchmark();
    }

    static void runBenchmark() {
        System.out.format("%1$8s%2$24s%3$30s%4$30s%n", "", "string[]", "SparseArray<string> 0.5", "SparseArray<string> 0.25");
        Random rnd = new Random();
        rnd.setSeed(2019);
        for (int size : SIZES) {
            String[] strings = new String[size];
            for (int i = size - 1; i >= 0; i--) {
                int n = rnd.nextInt(2);
                strings[i] = "";
                if (n == 0) {
                    strings[i] = "string" + i;
                }
            }

            SparseArray<String> sparseArray = new SparseArray<>(strings, "");

            String[] strings2 = new String[size];
            for (int i = size - 1; i >= 0; i--) {
                int n = rnd.nextInt(4);
                strings2[i] = "";
                if (n == 0) {
                    strings2[i] = "string" + i;
                }
            }

            SparseArray<String> sparseArray2 = new SparseArray<>(strings2, "");

            System.out.format("%1$8s%2$24s%3$24s%4$24s%n", size, bytesToString(objectArraySize(strings)), bytesToString(sparseArray.getMemorySize()), bytesToString(sparseArray2.getMemorySize()));
        }
    }

    static long measureTime(Runnable code) {
        long start = System.nanoTime();
        code.run();
        return System.nanoTime() - start;
    }

    static String bytesToString(long bytes){
        double b = bytes;
        String[] pref = {"", "K", "M", "G"};
        int prefIndex = 0;
        for(; b >= 1_000; b/=1_000){
            prefIndex++;
        }
        return String.format("%.2f%sB", b, pref[prefIndex]);
    }

    static long objectArraySize(Object[] array){
        long bytes = 12; // header
        bytes += array.length*16;
        return bytes;
    }
}

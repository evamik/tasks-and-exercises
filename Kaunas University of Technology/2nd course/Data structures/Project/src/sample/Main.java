package sample;

import benchmark.SimpleBenchmark;
import utils.SparseArray;

import java.util.Random;

public class Main {

    public static void main(String[] args) {
        String[] strings = new String[20];
        strings[0] = "Nulis";
        strings[5] = "Penki";
        strings[6] = "Šeši";
        strings[19] = "Devyniolika";

        SparseArray<String> sparseArray = new SparseArray<>(strings, "");
        println(sparseArray.toString());

        println(sparseArray.toArrayString(""));
        println("");

        println("remove(5): "+sparseArray.remove(5));
        println(sparseArray.toString());

        println("removeRange(0, 2): ");
        Object[] strs = sparseArray.removeRange(0, 2);
        for(int i = 0; i < strs.length; i++){
            println("     "+strs[i].toString());
        }

        new SimpleBenchmark();
    }

    public static void println(Object o){
        System.out.println(o);
    }
}

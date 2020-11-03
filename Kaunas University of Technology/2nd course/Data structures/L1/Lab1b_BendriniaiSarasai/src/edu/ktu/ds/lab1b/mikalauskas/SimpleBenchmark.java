package edu.ktu.ds.lab1b.mikalauskas;

import edu.ktu.ds.lab1b.demo.*;
import edu.ktu.ds.lab1b.util.Ks;
import edu.ktu.ds.lab1b.util.LinkedList;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Locale;
import java.util.Random;

public class SimpleBenchmark {

    Weapon[] weapons;
    LinkedList<Weapon> weaponSeries = new LinkedList<>();
    Random rg = new Random();  // atsitiktinių generatorius
   int[] counts = {2_000, 4_000, 8_000, 16_000, 32_000, 64_000};
//    pabandykite, gal Jūsų kompiuteris įveiks šiuos eksperimentus
//    paieškokite ekstremalaus apkrovimo be burbuliuko metodo
    static int[] counts2 = {1_000, 10_000, 100_000, 1_000_000};

    void generateWeapons(int count) {
        weapons = new Weapon[count];
        String[] types = { "Axe", "Sword", "Dagger", "Bow", "Spear", "Mace"};
        rg.setSeed(2019);
        for(int i = 0; i < count; i++){
            int typeIndex = rg.nextInt(types.length);
            weapons[i] = new Weapon(types[typeIndex], 
                    100 - rg.nextInt(110),
                    rg.nextInt(1000) - 100,
                    Math.round(rg.nextDouble() * 2_00) / 1_00.0,
                    rg.nextInt()==0 ? false : true
            );
        }
        Collections.shuffle(Arrays.asList(weapons));
        weaponSeries.clear();
        for (Weapon w : weapons) {
            weaponSeries.add(w);
        }
    }

    void generateAndExecute(int elementCount) {
        long t0 = System.nanoTime();
        generateWeapons(elementCount);
        LinkedList<Weapon> weaponSeries2 = weaponSeries.clone();
        LinkedList<Weapon> weaponSeries3 = weaponSeries.clone();
        LinkedList<Weapon> weaponSeries4 = weaponSeries.clone();
        long t1 = System.nanoTime();
        System.gc();
        System.gc();
        System.gc();
        long t2 = System.nanoTime();
        weaponSeries.sortSystem();
        long t3 = System.nanoTime();
        weaponSeries2.sortSystem(Weapon.byPrice);
        long t4 = System.nanoTime();
        weaponSeries3.sortBuble();
        long t5 = System.nanoTime();
        weaponSeries4.sortBuble(Weapon.byPrice);
        long t6 = System.nanoTime();
        Ks.ouf("%7d %7.4f %7.4f %7.4f %7.4f %7.4f %7.4f\n", elementCount,
                (t1 - t0) / 1e9, (t2 - t1) / 1e9, (t3 - t2) / 1e9,
                (t4 - t3) / 1e9, (t5 - t4) / 1e9, (t6 - t5) / 1e9);
    }

    void execute() {
        long memTotal = Runtime.getRuntime().totalMemory();
        Ks.oun("memTotal= " + memTotal/1048576);
        generateWeapons(20);
        for (Weapon w : weapons) {
            Ks.oun(w);
        }
        Ks.oun("1 - Pasiruošimas tyrimui - duomenų generavimas");
        Ks.oun("2 - Pasiruošimas tyrimui - šiukšlių surinkimas");
        Ks.oun("3 - Rūšiavimas sisteminiu greitu būdu be Comparator");
        Ks.oun("4 - Rūšiavimas sisteminiu greitu būdu su Comparator");
        Ks.oun("5 - Rūšiavimas List burbuliuku be Comparator");
        Ks.oun("6 - Rūšiavimas List burbuliuku su Comparator");
        Ks.ouf("%6d %7d %7d %7d %7d %7d %7d \n", 0, 1, 2, 3, 4, 5, 6);
        for (int n : counts) {
            generateAndExecute(n);
        }
    }
    
    double sqrt(double x, double y){
        return Math.sqrt(x*x+y*y);
    }
    
    void generateAndExecute2(int count){
        double x = rg.nextDouble()*1_00_000;
        double y = rg.nextDouble()*1_00_000;
        long t0 = System.nanoTime();
        System.gc();
        System.gc();
        System.gc();
        long t1 = System.nanoTime();
        for(int i = 0; i < count; i++){
            sqrt(x, y);
        }
        long t2 = System.nanoTime();
        for(int i = 0; i < count; i++){
            Math.hypot(x, y);
        }
        long t3 = System.nanoTime();
        Ks.ouf("%7d %7.4f %7.4f %7.4f \n", count, (t1-t0) / 1e9, (t2-t1) / 1e9,
                (t3-t2) / 1e9);
    }
    
    void execute2(){
        Ks.oun("0 - Pasiruošimas tyrimui - šiukšlių surinkimas");
        Ks.oun("1 - Operacijų skaičius");
        Ks.oun("2 - Math.sqrt(x*x + y*y)");
        Ks.oun("3 - Math.hypot(x, y)");
        Ks.ouf("%7d %7d %7d %7d \n", 0, 1, 2, 3);
        for(int n : counts2){
            generateAndExecute2(n);
        }
    }
    
    void generateAndExecute3(int count){
        LinkedList<Integer> linkedList = new LinkedList<>();
        ArrayList<Integer> arrayList = new ArrayList<>();
        long t0 = System.nanoTime();
        System.gc();
        System.gc();
        System.gc();
        long t1 = System.nanoTime();
        for(int i = 0; i < count; i++){
            arrayList.add(i/2, count);
        }
        long t2 = System.nanoTime();
        for(int i = 0; i < count; i++){
            linkedList.add(i/2, count);
        }
        long t3 = System.nanoTime();
        Ks.ouf("%7d %7.4f %7.4f %7.4f \n", count, (t1-t0) / 1e9, (t2-t1) / 1e9,
                (t3-t2) / 1e9);
    }
    
    void execute3(){
        Ks.oun("0 - Pasiruošimas tyrimui - šiukšlių surinkimas");
        Ks.oun("1 - Operacijų skaičius");
        Ks.oun("2 - ArrayList<Integer>");
        Ks.oun("3 - LinkedList<Integer>");
        Ks.ouf("%7d %7d %7d %7d \n", 0, 1, 2, 3);
        for(int n : counts){
            generateAndExecute3(n);
        }
    }

    public static void main(String[] args) {
        // suvienodiname skaičių formatus pagal LT lokalę (10-ainis kablelis)
        Locale.setDefault(new Locale("LT"));
//        new SimpleBenchmark().execute();
//        new SimpleBenchmark().execute2();
       new SimpleBenchmark().execute3();
    }
}

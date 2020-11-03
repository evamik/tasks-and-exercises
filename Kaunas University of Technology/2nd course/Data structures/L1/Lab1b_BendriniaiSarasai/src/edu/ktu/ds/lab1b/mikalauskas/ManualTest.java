/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package edu.ktu.ds.lab1b.mikalauskas;

import edu.ktu.ds.lab1b.util.Ks;
import java.util.Comparator;
import java.util.Locale;

/**
 *
 * @author mikal
 */
public class ManualTest {
    
    WeaponList weapons = new WeaponList();

    void execute() {
        //createWeapons();
        createWeaponList();
        //countAxe();
        //appendWeaponList();
        //checkWeaponMarketFilters();
        //checkWeaponMarketSorting();
        //checkContains();
        //checkRemoveFirst();
        checkRemoveRange();
    }
    
    void checkRemoveRange(){
        int from = 1;
        int to = 5;
        weapons.removeRange(from, to);
        weapons.println(String.format(" weapons.removeRange(%d, %d) ", from , to));
    }
    
    void checkRemoveFirst(){
        Ks.oun("Išimtas: " + weapons.removeFirst());
        Ks.oun("Išimtas: " + weapons.removeFirst());
        weapons.println();
    }
    
    void checkContains(){
        Ks.oun("weapons.contains(new Weapon(\"Mace\", 100, 980, 1.2, true)):");
        if(weapons.contains(new Weapon("Mace", 100, 980, 1.2, true)))
            Ks.oun(true);
        else Ks.oun(false);
        
        Ks.oun("weapons.contains(new Weapon(\"Macee\", 100, 980, 1.2, true)):");
        if(weapons.contains(new Weapon("Macee", 100, 980, 1.2, true)))
            Ks.oun(true);
        else Ks.oun(false);
    }

    void createWeapons() {
        Weapon w1 = new Weapon("Axe", 60, 300, 0.9, true);
        Weapon w2 = new Weapon("Sword", 75, 800, 0.8, true);
        Weapon w3 = new Weapon("Mace", 100, 980, 1.2, true);
        Weapon w4 = new Weapon();
        Weapon w5 = new Weapon();
        Weapon w6 = new Weapon();
        w4.parse("Dagger 20 150 0,5 true");
        w5.parse("Dagger 17 90 0,5 false");
        w6.parse("Bow 120 700 2,0 true");

        Ks.oun(w1);
        Ks.oun(w2);
        Ks.oun(w3);
        Ks.oun("Pirmų 3 ginklų žalos vidurkis= "
                + (w1.getDamage() + w2.getDamage() + w3.getDamage()) / 3);
        Ks.oun(w4);
        Ks.oun(w5);
        Ks.oun(w6);
        Ks.oun("Kitų 3 ginklų kainų suma= "
                + (w4.getPrice() + w5.getPrice() + w6.getPrice()));
    }

    void createWeaponList() {
        Weapon w1 = new Weapon("Axe", 60, 300, 0.9, true);
        Weapon w2 = new Weapon("Sword", 75, 800, 0.8, true);
        Weapon w3 = new Weapon("Mace", 100, 980, 1.2, true);
        weapons.add(w1);
        weapons.add(w2);
        weapons.add(w3);
        weapons.println("Pirmi 3 ginklai");
        weapons.add("Dagger 20 150 0,5 true");
        weapons.add("Spear 100 300 1,7 true");
        weapons.add("Bow 120 700 2,0 true");
        weapons.println("Pradiniai duomenys");

/*        weapons.println("Visi 6 ginklai");
        weapons.forEach(System.out::println);
        Ks.oun("Pirmų 3 ginklų žalos vidurkis= "
                + (weapons.get(0).getDamage() + weapons.get(1).getDamage()
                + weapons.get(2).getDamage()) / 3);

        Ks.oun("Kitų 3 ginklų kainų suma= "
                + (weapons.get(3).getPrice() + weapons.get(4).getPrice()
                + weapons.get(5).getPrice()));
       Ks.oun(weapons.size());
       weapons.add(0, new Weapon("Axe", 60, 300, 0.4, true));
        weapons.add(6, new Weapon("Mace", 150, 9800, 1.2, true));
*/        
//        weapons.set(4, w3);
//        weapons.println("Po įterpimų");
//        weapons.remove(7);
//        weapons.remove(0);
//        weapons.println("Po išmetimų");
//        weapons.remove(0); weapons.remove(0); weapons.remove(0);
//        weapons.remove(0); weapons.remove(0); weapons.remove(0);
//        weapons.println("Po visų išmetimų");
//        weapons.remove(0);
//        weapons.println("Po visų išmetimų");
    }

    void countAxe() {
        int sk = 0;
        for (Weapon w : weapons) {
            if (w.getType().compareTo("Axe") == 0) {
                sk++;
            }
        }
        Ks.oun("Kirvių ginklų yra = " + sk);
    }

    void appendWeaponList() {
        for (int i = 0; i < 8; i++) {
            weapons.add(new Weapon("Sword", 20+5*i,
                    200+50*i, 0.7 + 0.1*i, (i%3==0) ? false : true));
        }
        weapons.add("Dagger 20 90  0,7 true");
        weapons.add("Dagger 20 150 0,9 true");
        weapons.add("Dagger 10 150 0,5 false");
        weapons.add("Dagger -1 150 0,5 true");
        weapons.println("Testuojamų ginklų sąrašas");
        weapons.save("ban.txt");
    }
    
    void checkWeaponMarketFilters() {
        WeaponMarket market = new WeaponMarket();

        market.allWeapons.load("ban.txt");
        market.allWeapons.println("Bandomasis rinkinys");

        weapons = market.getHigherDamageWeapons(90);
        weapons.println("Žala nuo 90");

        weapons = market.getByPrice(300, 1000);
        weapons.println("Kaina tarp 300 ir 1000");

        weapons = market.getFastestWeapons();
        weapons.println("Greičiausi ginklai");

        weapons = market.getByType("S");
        weapons.println("Turi būti tik Swords ir Spears");

        weapons = market.getByType("Sw");

        weapons.println("Turi būti tik Sword");
        int n = 0;
        for (Weapon w : weapons) {
            n++;    // testuojame ciklo veikimą
        }
        Ks.oun("Sword kiekis = " + n);
    }

    // išbandykite veikimą, o po to pakeiskite į Lambda stiliaus komparatorius.
    void checkWeaponMarketSorting() {
        WeaponMarket market = new WeaponMarket();

        market.allWeapons.load("ban.txt");
        Ks.oun("========" + market.allWeapons.get(0));
        market.allWeapons.println("Bandomasis rinkinys");
        market.allWeapons.sortBuble(Weapon.byType);
        market.allWeapons.println("Rūšiavimas pagal tipą");
        market.allWeapons.sortBuble(Weapon.byPrice);
        market.allWeapons.println("Rūšiavimas pagal kainą");
        market.allWeapons.sortBuble(Weapon.byDps);
        market.allWeapons.println("Rūšiavimas pagal Damage per second");
        market.allWeapons.sortBuble(bySpeed);
        market.allWeapons.sortBuble((a, b) -> Double.compare(a.getSpeed(), b.getSpeed()));
        market.allWeapons.println("Rūšiavimas pagal greitį");
        market.allWeapons.sortBuble();
        market.allWeapons.println("Rūšiavimas pagal compareTo - Kainą");
    }

    static Comparator bySpeed = (Comparator) (Object obj1, Object obj2) -> {
        double s1 = ((Weapon) obj1).getSpeed();
        double s2 = ((Weapon) obj2).getSpeed();
        // greitis atvirkščia mažėjančia tvarka, pradedant nuo didžiausio
        if (s1 < s2) {
            return 1;
        }
        if (s1 > s2) {
            return -1;
        }
        return 0;
    };

    public static void main(String... args) {
        // suvienodiname skaičių formatus pagal LT lokalę (10-ainis kablelis)
        Locale.setDefault(new Locale("LT"));
        new ManualTest().execute();
    }
    
}

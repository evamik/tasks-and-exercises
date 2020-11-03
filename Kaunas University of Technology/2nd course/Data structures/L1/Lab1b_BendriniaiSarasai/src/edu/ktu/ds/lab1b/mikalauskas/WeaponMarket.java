/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package edu.ktu.ds.lab1b.mikalauskas;

/**
 *
 * @author mikal
 */
public class WeaponMarket {
    
    public WeaponList allWeapons = new WeaponList(20);
    
    // suformuojamas sąrašas ginklų, kurių žala ne mažesnė nei riba
    public WeaponList getHigherDamageWeapons(int fromDamage) {
        WeaponList weapons = new WeaponList();
        for (Weapon w : allWeapons) {
            if (w.getDamage() >= fromDamage) {
                weapons.add(w);
            }
        }
        return weapons;
    }

    // suformuojamas sąrašas ginklų, kurių kaina yra tarp ribų
    public WeaponList getByPrice(int fromPrice, int toPrice) {
        WeaponList weapons = new WeaponList();
        for (Weapon w : allWeapons) {
            if (w.getPrice() >= fromPrice && w.getPrice() <= toPrice) {
                weapons.add(w);
            }
        }
        return weapons;
    }

    // suformuojamas sąrašas greičiausių ginklų
    public WeaponList getFastestWeapons() {
        WeaponList weapons = new WeaponList();
        // formuojamas sąrašas su maksimalia reikšme vienos peržiūros metu
        double minSpeed = Double.MAX_VALUE;
        for (Weapon w : allWeapons) {
            double speed = w.getSpeed();
            if (speed <= minSpeed) {
                if (speed < minSpeed) {
                    weapons.clear();
                    minSpeed = speed;
                }
                weapons.add(w);
            }
        }
        return weapons;
    }

    // suformuojams sąrašas ginklų, kurių tipas atitinka nurodytą
    public WeaponList getByType(String type) {
        WeaponList weapons = new WeaponList();
        for (Weapon w : allWeapons) {
            String weaponType = w.getType();
            if (weaponType.startsWith(type)) {
                weapons.add(w);
            }
        }
        return weapons;
    }
}

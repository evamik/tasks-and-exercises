/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package edu.ktu.ds.lab1b.mikalauskas;

import edu.ktu.ds.lab1b.util.ParsableList;
import java.util.Random;

/**
 *
 * @author mikal
 */
public class WeaponList extends ParsableList<Weapon> {
    
    public WeaponList(){
        
    }
    
    public WeaponList(int count){
        super();
        String[] types = { "Axe", "Sword", "Dagger", "Bow", "Spear", "Mace"};
        Random rnd = new Random();
        rnd.setSeed(2019);
        for(int i = 0; i < count; i++){
            int typeIndex = rnd.nextInt(types.length);
            add(new Weapon(types[typeIndex], 
                    100 - rnd.nextInt(110),
                    rnd.nextInt(1000) - 100,
                    Math.round(rnd.nextDouble() * 2_00) / 1_00.0,
                    rnd.nextInt()==0 ? false : true
            ));
        }
    }
    
    @Override
    protected Weapon createElement(String data) {
        return new Weapon(data);
    }
}

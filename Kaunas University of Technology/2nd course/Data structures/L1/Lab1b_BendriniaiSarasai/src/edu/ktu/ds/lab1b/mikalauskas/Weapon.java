
package edu.ktu.ds.lab1b.mikalauskas;

import edu.ktu.ds.lab1b.util.Ks;
import edu.ktu.ds.lab1b.util.Parsable;
import java.util.Comparator;
import java.util.InputMismatchException;
import java.util.Locale;
import java.util.NoSuchElementException;
import java.util.Objects;
import java.util.Scanner;

/**
 *
 * @author mikal
 */
public class Weapon implements Parsable<Weapon>{

    final static private double minSpeed = 0.5;
    
    private String type;
    private int damage;
    private int price;
    private double speed;
    private boolean isInGoodCondition;
    
    public Weapon(){
        
    }
    
    public Weapon(String type, int damage, int price, double speed, boolean inGoodCondition){
        this.type = type;
        this.damage = damage;
        this.price = price;
        this.speed = speed;
        this.isInGoodCondition = inGoodCondition;
    }
    
    public Weapon(String data){
        parse(data);
    }
    
    @Override
    public final void parse(String data) {
        try {
            Scanner ed = new Scanner(data);
            type = ed.next();
            damage = ed.nextInt();
            price = ed.nextInt();
            speed = ed.nextDouble();
            isInGoodCondition = ed.nextBoolean();
        } catch (InputMismatchException e) {
            Ks.ern("Blogas duomenų formatas apie ginklą -> " + data);
        } catch (NoSuchElementException e) {
            Ks.ern("Trūksta duomenų apie ginklą -> " + data);
        }
    }
    
    public String validate() {
        String error = "";
        if(price < 0)
            error = " Ginklo kaina negali būti žemiau 0.";
        if(damage <= 0)
            error += "  Ginklo žala turi būti > 0.";
        if(speed < minSpeed)
            error += "  Ginklo greitis turi būti >= " + minSpeed + ".";
        if(!isInGoodCondition)
            error += "  Ginklas turi būti geros būklės.";
        return error;
    }
    
    @Override
    public String toString() {  // surenkama visa reikalinga informacija
        return String.format("%-8s %3d %4d %2.2f %5s %s",
                type, damage, price, speed, isInGoodCondition, validate());
    }
    
    String getType(){ return type; }
    int getDamage(){ return damage; }
    int getPrice(){ return price; }
    double getSpeed(){ return speed; }
    boolean getIsInGoodCondition() { return isInGoodCondition; }

    @Override
    public int compareTo(Weapon otherWeapon) {
        double otherPrice = otherWeapon.getPrice();
        if(price < otherPrice)
            return -1;
        if(price > otherPrice)
            return 1;
        return 0;
    }
    
    public final static Comparator<Weapon> byType =
            (Weapon w1, Weapon w2) -> w1.getType().compareTo(w2.getType());
    
    
    public final static Comparator<Weapon> byPrice =
            (Weapon w1, Weapon w2) -> Integer.compare(w1.getPrice(), w2.getPrice());
    
    public final static Comparator<Weapon> byDps =
            (Weapon w1, Weapon w2) -> Double.compare(w1.getDamage()/w1.getSpeed(), w2.getDamage()/w1.getSpeed());
    
    public static void main(String... args) {
        Locale.setDefault(new Locale("LT"));
        Weapon a1 = new Weapon("Axe", 60, 300, 0.9, true);
        Weapon a2 = new Weapon("Sword", 75, 800, 0.8, true);
        Weapon a3 = new Weapon();
        Weapon a4 = new Weapon();
        a3.parse("Dagger 20 150 0,5 true");
        a4.parse("Dagger 17 90 0,5 false");
        Ks.oun(a1);
        Ks.oun(a2);
        Ks.oun(a3);
        Ks.oun(a4);
    }
}

/****************************************************************
 * Tai įvadinė klasė žengiant į įvykiais valdomą programavimą.
 * Event-driven programming: tai plati tema- pradedame nuo paprastų pavyzdžių
 * DAUG KLAUSIMŲ APIE JAVĄ GALIMA IŠSIAIŠKINTI, ATLIEKANT
 * SAVARANKIŠKUS BANDYMUS, TYRIMUS IR ANALIZĘ :)
 * 
 * Visos demonstracinės klasės turi main metodą ir vykdomos Run File (Shift+F6)
 * 
 * Pradžioje vykdykite kodą ir stebėkite atliekamus veiksmus
 * Užduotis: Peržiūrėkite ir išsiaiškinkite pateiktas klases,
 * pakeiskite programinį kodą, stebėkite gautus rezultatus.
 * Gynimo metu atlikite dėstytojo nurodytas užduotis naujų metodų pagalba.
 *
 * @author Eimutis Karčiauskas, KTU programų inžinerijos katedra 2019 08 05
 **************************************************************************/
package demos.console;

import extendsFX.BaseConsole;
import javafx.stage.Stage;

public class Demo0_Hello extends BaseConsole{
    int clicks = 0;
    // demo pavyzdys: skaičiuojame x^2, x^3 ir fact(x)
    // nustatykite nuo kurios x reikšmės faktorialas blogai skaičiuojamas
    void calculate(){
        int x = readLastInt();
        int fact = 1;
        int k = 2;
        for(; k <= x; k++){
            fact *= k;
            if(fact < 0){
                fact = -1;
                break;
            }
        }
        print(" x="+ x +" x^2="+ x*x +" x^3="+ x*x*x);
        if(fact!=-1) printLn(" fact=" + fact);
        else printLn(String.format("   %d is too big for calculating factorial with Integer", k));
    }
    
    void calculateSin(){
        double x;
        try{
            x = Double.parseDouble(readLastLine());
        }
        catch(NumberFormatException e){
            printLn("Input a parsable number");
            return;
        }
        double rad = Math.toRadians(x);
        double sin = Math.sin(rad);
        double cos = Math.cos(rad);
        double tan = Math.tan(rad);
        printLn(String.format(" sin(x) = %.3f    cos(x) = %.3f    tan(x) = %.3f", sin, cos, tan));
    }
// UŽDUOTIS: sudarykite naują metodą funkcijų sin, cos ir tan skaičiavimui, 
//           kai duotas kampas yra nurodomas laipsniais
    @Override
    public void createControls() {
        super.createControls();    // sukuriame bazinius mygtukus
        addButton("Hello1", e -> ta1.appendText("Hello World!!! " +nL));
        addButton("Hello2", e -> ta2.appendText("Hello World!!! " 
                                 + "I can count clicks: " + ++clicks +nL));
        addButton("calc",   e -> calculate());
        addButton("calcSin",   e -> calculateSin());
    }
    @Override
    public void start(Stage stage) throws Exception {
        stage.setTitle("Hello World (KTU IF)");
//         išbandykite skirtingus ekranų nustatytumus 
        setTextAreas("green", "red", 600, 400);
//        setTextAreas("green", null, 300, 400);
//        setTextAreas(null, "red", 350, 400);
        super.start(stage);  
    }
    public static void main(String[] args) {
        launch();
    }
}
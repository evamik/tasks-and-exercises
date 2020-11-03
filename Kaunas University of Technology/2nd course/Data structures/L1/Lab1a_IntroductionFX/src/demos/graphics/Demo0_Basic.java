/****************************************************************
 * Šioje klasėje pateikamas įvadas į JavaFX grafiką
 * 
 * Pradžioje vykdykite kodą ir stebėkite atliekamus veiksmus
 * Užduotis atlikite sekdami nurodymus programinio kodo komentaruose
 * Gynimo metu atlikite dėstytojo nurodytas užduotis naujų metodų pagalba.
 *
 * @author Eimutis Karčiauskas, KTU programų inžinerijos katedra 2019 08 05
 **************************************************************************/
package demos.graphics;

import extendsFX.BaseGraphics;
import java.util.Random;
import javafx.scene.Group;
import javafx.scene.Scene;
import javafx.scene.paint.Color;
import javafx.scene.shape.ArcType;
import javafx.scene.shape.Polygon;
import javafx.scene.text.Font;
import javafx.stage.Stage;

public class Demo0_Basic extends BaseGraphics{
        
    private void drawCircle(){
        Random rng = new Random();
        rng.setSeed(System.nanoTime());
        gc.setStroke(Color.BLACK);
        gc.strokeOval(rng.nextDouble()*canvasW, rng.nextDouble()*canvasH, 50, 50);
    }
    
    // pradžioje brėšime horizontalias ir vertikalias linijas per centrą
    private void drawHVtoCenter() {  
        gc.setLineWidth(3);       // brėžimo linijos plotis
        gc.setStroke(Color.RED);  // ir tos linijos spalva
        gc.strokeLine(0, canvasH / 2, canvasW, canvasH / 2);
        gc.strokeLine(canvasW / 2, 0, canvasW / 2, canvasH);
    }
    // po to brėšime įstrižaines per centrą
    private void drawXtoCenter() {
        gc.setLineWidth(4);         // brėžimo linijos plotis
        gc.setStroke(Color.GREEN);  // ir tos linijos spalva
        gc.strokeLine(0, 0, canvasW, canvasH);
        gc.strokeLine(0, canvasH, canvasW, 0);
    }  
// UŽDUOTIS_1: plonomis linijomis su žingsniu step=50 nubrėžkite tinklelį
    private void drawGrid1() { 
        double step = 50;
        gc.setLineWidth(0.1);
        gc.setStroke(Color.BLACK);
        double max = Math.max(canvasW, canvasH);
        for(double i = step; i < max; i+=step){
            gc.strokeLine(i, 0, i, canvasH);
            gc.strokeLine(0, i, canvasW, i);
        }
    }
// https://examples.javacodegeeks.com/desktop-java/javafx/javafx-canvas-example/    
    private void drawExamples1() {
        double lw = 3.0;
        gc.setLineWidth(lw);        // brėžimo linijos plotis
        gc.setStroke(Color.BLUE);   // ir tos linijos spalva
        gc.setFill(Color.RED);      // dažymo spalva figūroms
        int x=10, y=10, w=80, h=50, 
            d=20, ax=10, ay=20; // d-tarpas tarp elementų, ax,ay-apvalinimai
        gc.strokeRoundRect(x, y, w, h, ax, ay);
        x+=w+d; // sekantis į dešinę
        gc.fillRoundRect(  x, y, w, h, ax, ay);
        gc.setLineWidth(0.5);
        gc.strokeText("Wolf and Bear", x, y+h);
        //-------------------
        gc.setLineWidth(2*lw);    // dvigubai pastoriname liniją      
        gc.setFill(Color.YELLOW);
        x = 10;    // grįžtame horizontaliai
        y += h+d;  // ir pereiname žemyn
        gc.strokeOval(x, y, w, h);
        x += w+d; // sekantis į dešinę
        gc.fillOval( x, y, h, w);
        x = 10;     // grįžtame horizontaliai
        y += h+2*d; // ir pereiname žemyn ir brėžiame lankus
        gc.strokeArc  (x, y, w, w, 30,  90, ArcType.ROUND);
        gc.fillArc(x+w+d, y, w, w, 45, 180, ArcType.OPEN);
    }  
    private void drawUnicode(){
        // išbandykite ir kitus simbolius
        // https://en.wikipedia.org/wiki/List_of_Unicode  skyrius 31
        StringBuilder sb = new StringBuilder();
        for(char ch = '\u2654'; ch <= '\u265F'; ch++)
            sb.append(ch);
        gc.setFont(Font.font("Lucida Console", 36));
        gc.setLineWidth(1);
        gc.setStroke(Color.BLACK);
        gc.strokeText(sb.toString(), 50, 350);
    }
    
    private void drawUnicode2(){
        StringBuilder sb = new StringBuilder();
        for(char ch = '\u2854'; ch <= '\u285F'; ch++)
            sb.append(ch);
        gc.setFont(Font.font("Lucida Console", 36));
        gc.setLineWidth(1);
        gc.setStroke(Color.BLACK);
        gc.strokeText(sb.toString(), 50, 350);
    }
// UŽDUOTIS_2: nubrėžkite polilinijas ir poligonus   
// https://www.tutorialspoint.com/javafx/2dshapes_polygon    
    private void drawExamples2() {  
        gc.setLineWidth(4);         // brėžimo linijos plotis
        gc.setStroke(Color.GREEN);  // ir tos linijos spalva
        gc.strokePolygon(
                new double[]{200.0, 400.0, 450.0, 400.0, 200.0, 150.0}, 
                new double[]{50.0, 50.0, 150.0, 250.0, 250.0, 150.0}, 
                6);
        gc.strokePolyline(
                new double[]{500, 450, 500, 450, 300, 300}, 
                new double[]{200, 250, 300, 350.0, 350, 100}, 
                6);
    }
    
// UŽDUOTIS_3: nubrėžkite taisyklingus 3, 4, 5, ..., 9-kampius  
    private int corners = 3;    
    private double centerX = 100;
    private double centerY = 100;
    private void drawExamples3() {
        double radius = 70;
        double degrees = -90;
        double[] xCoords = new double[corners];
        double[] yCoords = new double[corners];
        
        for(int i = 0; i < corners; i++){
            xCoords[i] = centerX + Math.cos(Math.toRadians(degrees))*radius;
            yCoords[i] = centerY + Math.sin(Math.toRadians(degrees))*radius;
            degrees += 360/corners;
        }
        gc.setStroke(Color.BLACK);
        gc.strokePolygon(xCoords, yCoords, corners);
        corners++;
        centerX+=150;
    }
// UŽDUOTIS_4: nubrėžkite žiedus https://en.wikipedia.org/wiki/Olympic_symbols
    private void drawOlympicRings() {      
        gc.setLineWidth(9);
        gc.setStroke(Color.rgb(0, 133, 199)); // blue ring
        gc.strokeOval(150, 150, 100, 100);
        
        gc.setStroke(Color.rgb(244, 195, 0)); // yellow ring
        gc.strokeOval(210, 210, 100, 100);
        
        gc.setStroke(Color.rgb(0, 133, 199)); // blue ring overlap
        gc.strokeArc(150, 150, 100, 100, 0, -45, ArcType.OPEN);
        
        gc.setStroke(Color.BLACK); // black ring
        gc.strokeOval(270, 150, 100, 100);
        
        gc.setStroke(Color.rgb(244, 195, 0)); // yellow ring overlap
        gc.strokeArc(210, 210, 100, 100, 90, -45, ArcType.OPEN);
        
        gc.setStroke(Color.rgb(0, 159, 61)); // green ring
        gc.strokeOval(330, 210, 100, 100);
        
        gc.setStroke(Color.BLACK); // black ring overlap
        gc.strokeArc(270, 150, 100, 100, -45, 90, ArcType.OPEN);
        
        gc.setStroke(Color.rgb(223, 0, 36)); // red ring
        gc.strokeOval(390, 150, 100, 100);
        
        gc.setStroke(Color.rgb(0, 159, 61)); // green ring overlap
        gc.strokeArc(330, 210, 100, 100, 90, -45, ArcType.OPEN);
    }
// UŽDUOTIS_5: pasirinktinai nubrėžkite savo tematiką:
// kelių valstybių sudėtingesnes vėliavas http://flagpedia.net/index
// pvz: Pietų Afrikos, Makedonijos, Norvegijos, Graikijos, Britanijos, ...
// arba futbolo, krepšinio ar ledo ritulio aikštes su žaidėjų pozicijomis  
    private void drawFreeThema() {   
        //gc.setStroke(Color.BLUE);
        //gc.strokeRect(50, 50, 400, 250);
        gc.setFill(Color.BLUE);
        gc.fillPolygon(
                new double[] {50.0, 500.0, 500.0, 50.0},
                new double[] {50.0, 50.0, 300.0, 300.0},
                4);
        
        gc.setFont(Font.font(28));
        gc.setFill(Color.YELLOW);
        double radius = 85;
        for(int deg = 0; deg < 360; deg+=360/12){
            gc.fillText(""+'\u2605', 270+Math.cos(Math.toRadians(deg))*radius, 180-Math.sin(Math.toRadians(deg))*radius);
        }
    }    
// kontrolinės užduotys gynimo metu:
// įvairios vėliavos, tiesiog tokios sudėtinės figūros kaip namukas,
// medis, eglė, sniego senio siluetas :-) ir t.t.    
    @Override
    public void createControls(){
        addButton("clear", e -> {gc.restore(); clearCanvas(); gc.save();}); 
        addButton("grid",  e -> baseGrid());
        addButton("HVC",   e -> drawHVtoCenter());
        addButton("XC",    e -> drawXtoCenter());
        addButton("pvz1",  e -> drawExamples1());
        addButton("UniCode",  e -> drawUnicode());
        addButton("UniCode2",  e -> drawUnicode2());
        addButton("ex1 grid",  e -> drawGrid1());
        addButton("ex2 poly",  e -> drawExamples2());
        addButton("ex3 poly",  e -> drawExamples3());
        addButton("olympic rings",  e -> drawOlympicRings());
        addButton("flag",  e -> drawFreeThema());
        addButton("circle", e -> drawCircle());
        addNewHBox();
    }
    @Override
    public void start(Stage stage) throws Exception {
        stage.setTitle("Braižymai Canvas lauke (KTU IF)");        
        setCanvas(Color.CYAN.brighter(), 1500, 400);
        super.start(stage);
        gc.save();
    }       
    public static void main(String[] args) {
        launch(args);
    }    
}

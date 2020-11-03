/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package trumpiausias.medis;

import java.io.*;
import java.util.*;

import javafx.animation.*;
import javafx.application.Application;
import javafx.geometry.Point2D;
import javafx.scene.Group;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.scene.shape.Ellipse;
import javafx.scene.shape.Line;
import javafx.scene.text.Font;
import javafx.stage.Stage;
import javafx.util.Duration;

/**
 *
 * @author evamik3
 */
public class main extends Application {

    int stageWidth = 800;
    int stageHeight = 700;
    int borderWidth = 50;
    int borderHeight = 100;
    int nodeCount = 100;
    double connTransparency = 0.3;
    boolean connectedToAll = true;
    double scale = 0.3;

    int absCount = 271;
    private List<String> regions = new ArrayList<String>();
    private HashMap<String, Double> distances = new HashMap<>();
    private HashMap<String, Double> flowsRoad = new HashMap<>();
    private HashMap<String, Double> flowsRail = new HashMap<>();
    private HashMap<String, Double> flowsRoadKMS = new HashMap<>();
    private HashMap<String, Double> flowsRailKMS = new HashMap<>();
    private Boolean[] warehouses = new Boolean[absCount];
    private Boolean[] prevWarehouses = warehouses;
    private Node[] regionNodes = new Node[absCount];
    private int prevWarehouseCount = 0;
    private int warehouseCount = 0;
    private int initialWarehouseCount = 13;
    private double initialCostSum = 0;
    private double currentCostSum = 0;
    private double gradeCoefficient = 1;

    private double P1 = 308525.05;
    private double P2 = 539.91;
    private double P3 = 8513.26;
    private double P4 = 6.31;
    private double D_Truck = 125.41;
    private double D_Train = 3.95;
    private double E_Truck = 3;
    private double E_Train = 5;

    ArrayList<Node> cNodes = new ArrayList<>();
    Group root;
    VBox textGroup;

    public double cheapestPath(String path, String destination){

        for(int i = 0; i < absCount; i++){
            if(warehouses[i]){
                String region = regions.get(i);
                if(!path.contains(region)){
                    cheapestPath(path+"-"+region, destination);
                }
            }
        }
        return 0.0;
    }

    public void bestPath(int reg1, int reg2){
        for(int i = 0; i < absCount; i++){

        }
    }

    public void initialWarehouses(int count){
        if(count > absCount)
            count = absCount;
        List<Integer> indexes = new LinkedList<>();
        for(int i = 0; i < absCount; i++){
            indexes.add(i);
            warehouses[i] = false;
        }
        Random rnd = new Random();
        rnd.setSeed(System.nanoTime());
        for(int i = 0; i < count; i++){
            warehouses[rnd.nextInt(indexes.size())] = true;
            warehouseCount++;
        }
        prevWarehouses = warehouses;
        prevWarehouseCount = warehouseCount;
        initialCostSum = allRegionsCostSum();
    }

    public boolean tryChange(){
        Random rnd = new Random();
        int span = rnd.nextInt(absCount/10);
        int startIndex = rnd.nextInt(absCount-span-1);
        for(int i = startIndex; i < startIndex+span; i++){
            if(rnd.nextInt(span) <= Math.ceil(span/10.0))
                if(warehouses[i])
                    warehouseCount--;
                else warehouseCount++;
                warehouses[i] = !warehouses[i];
        }
        double cost = allRegionsCostSum() + allWarehouseCost();
        if(cost < currentCostSum){
            println(cost/currentCostSum);
            currentCostSum = cost;
            prevWarehouses = warehouses;
            prevWarehouseCount = warehouseCount;
            return true;
        }

        warehouses = prevWarehouses;
        warehouseCount = prevWarehouseCount;
        return false;
    }

    public double regionCostSum(String path){
        double costSum = 0;
        String[] paths = path.split("-");
        //for (int ) {
//
        //}
        return costSum;
    }

    public double regionCostSum(int index){
        double costSum = 0;

        for(int i = 0; i < regions.size(); i++){
            String key = combinedRegions(regions.get(index), regions.get(i));
            double dist = distances.get(key);
            if(dist == 0) continue;
            Set<String> keys = flowsRailKMS.keySet();
            double flowRoadKMS = flowsRoadKMS.containsKey(key) ? flowsRoadKMS.get(key) : 9999999;
            double flowRailKMS = flowsRailKMS.containsKey(key) ? flowsRailKMS.get(key) : 9999999;

            // calculate cost of truck
            double truckCost = (flowRoadKMS * D_Truck * dist) + (dist * 3);

            //calculate cost of train
            double trainCost = Double.MAX_VALUE;
            if(warehouses[index] && warehouses[i])
                trainCost = (flowRailKMS * D_Train * dist) + (dist * 5);

            if(trainCost < truckCost)
                costSum += trainCost;
            else costSum += truckCost;
        }

        return costSum;
    }

    public double warehouseCost(int index){
        double cost = 0;
        String key = combinedRegions(regions.get(index), regions.get(index));
        double flow = flowsRail.containsKey(key) ? flowsRail.get(key) : flowsRoad.get(key);
        flow = 1;

        cost = P1 + P2*flow
                + P3*12 + P4*flow*365;

        return cost;
    }

    public double allWarehouseCost(){
        double cost = 0;

        for(int i = 0; i < absCount; i++)
            if(warehouses[i])
                cost += warehouseCost(i);

        return cost;
    }

    public double allRegionsCostSum(){
        double costSum = 0;

        for(int i = 0; i < absCount; i++){
            costSum += regionCostSum(i);
        }

        return costSum;
    }

    public void readDist(){
        File file = new File(System.getProperty("user.dir")+"\\data\\dist.csv");

        BufferedReader br = null;
        try {
            br = new BufferedReader(new FileReader(file));
            String st;
            br.readLine();
            while((st = br.readLine())!=null){
                String[] values = st.split(",");
                String region1 = values[1];
                String region2 = values[2];
                if(!regions.contains(region1))
                    regions.add(region1);
                distances.put(combinedRegions(region1, region2), Double.valueOf(values[3]));
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void readFlows(){
        File file = new File(System.getProperty("user.dir")+"\\data\\flows.csv");

        BufferedReader br = null;
        try {
            br = new BufferedReader(new FileReader(file));
            String st;
            br.readLine();


            while((st = br.readLine())!=null){
                String[] values = st.split(",");
                String key = combinedRegions(values[0], values[1]);
                switch (values[2]){
                    case "Rail":
                        flowsRail.put(key, Double.valueOf(values[3]));
                        flowsRailKMS.put(key, Double.valueOf(values[4]));
                        break;
                    case "Road":
                        flowsRoad.put(key, Double.valueOf(values[3]));
                        flowsRoadKMS.put(key, Double.valueOf(values[4]));
                        break;
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    
    public void placeNodes(){
        Random rnd = new Random();
        rnd.setSeed(2019);
        for(int i = 0; i < absCount; i++){
            double x = rnd.nextInt(stageWidth-borderWidth*2)+borderWidth;
            double y = rnd.nextInt(stageHeight-borderHeight*2)+borderHeight;
            Ellipse el = new Ellipse(x ,y ,5*scale, 5*scale);
            el.setFill(Color.BLACK);
            el.getProperties().put("x", x);
            el.getProperties().put("y", y);
            regionNodes[i] = el;
            root.getChildren().add(el);
        }

        double maxDist = Math.sqrt(Math.pow(stageWidth, 2) + Math.pow(stageHeight, 2))/8;

        for(int i = 0; i < absCount; i++){
            if(warehouses[i]){
                Ellipse el = ((Ellipse)regionNodes[i]);
                el.setRadiusX(10*scale);
                el.setRadiusY(10*scale);
                el.setFill(Color.GREEN);
                for(int j = i+1; j < absCount; j++) {
                    double x1 = (double) el.getProperties().get("x");
                    double y1 = (double) el.getProperties().get("y");
                    double x2 = (double) regionNodes[j].getProperties().get("x");
                    double y2 = (double) regionNodes[j].getProperties().get("y");
                    double dist = new Point2D(x1, y1).distance(x2, y2);
                    if(warehouses[j]){
                        if(dist < maxDist) {
                            Line line = new Line(x1, y1, x2, y2);
                            line.setStrokeWidth(scale * 5);
                            line.setStroke(Color.GREENYELLOW);
                            line.setOpacity(0.2);
                            root.getChildren().add(0, line);
                        }
                    }
                }
            }
        }
    }

    private void setup(BorderPane pane){
        Button btn = new Button();
        btn.getProperties().put("clicked", false);
        btn.setFont(Font.font(20));
        btn.setText("Optimizuoti");
        btn.setOnAction((event) -> {
            ((Button)event.getSource()).setDisable(true);
            KeyFrame kf = new KeyFrame(Duration.millis(150), event1 -> {
                int i = 0;
                while(i < 5) {
                    if(tryChange()) {
                        print("new better configuration! ");
                        print(currentCostSum);
                        println(" by " + 100 / initialCostSum * (initialCostSum - currentCostSum) + "%");
                        root.getChildren().clear();
                        placeNodes();
                        i = 5;
                        break;
                    }
                    i++;
                    return;
                }
            });
            Timeline timeline = new Timeline();
            timeline.setCycleCount(100);
            timeline.getKeyFrames().add(kf);
            timeline.play();
            timeline.setOnFinished(e -> ((Button)event.getSource()).setDisable(false));
        });

        HBox ui = new HBox();
        ui.getChildren().add(btn);
        pane.setBottom(ui);
    }

    String combinedRegions(String region1, String region2){
        if(region1.compareTo(region2) <= 0)
            return region1+"-"+region2;
        return region2+"-"+region1;
    }

    String combinedRegions(int reg1, int reg2){
        String region1 = regions.get(reg1);
        String region2 = regions.get(reg2);
        if(region1.compareTo(region2) <= 0)
            return region1+"-"+region2;
        return region2+"-"+region1;
    }

    void println(){
        System.out.println();
    }

    void println(Object o){
        System.out.println(o);
    }

    void println2(Object o){
        System.out.print("- ");
        println(o);
    }

    void print(Object o){
        System.out.print(o);
    }

    void println(double d){
        if(d >= 1E15)
            println(Math.round(d/1E13)/1E2 + " quadrillion");
        else if(d >= 1E12)
            println(Math.round(d/1E10)/1E2 + " trillion");
        else if(d >= 1E9)
            println(Math.round(d/1E7)/1E2 + " billion");
        else if(d >= 1E6)
            println(Math.round(d/1E4)/1E2 + " million");
    }

    void print(double d){
        if(d >= 1E15)
            print(Math.round(d/1E13)/1E2 + " quadrillion");
        else if(d >= 1E12)
            print(Math.round(d/1E10)/1E2 + " trillion");
        else if(d >= 1E9)
            print(Math.round(d/1E7)/1E2 + " billion");
        else if(d >= 1E6)
            print(Math.round(d/1E4)/1E2 + " million");
    }


    @Override
    public void start(Stage primaryStage) throws Exception {
        BorderPane pane = new BorderPane();

        println("Reading distances...");
        readDist();
        println2(regions.size()+" regions read");
        println2(distances.size()+" unique distances read");
        System.out.println("Distance between "+regions.get(132)+" and "+regions.get(56)+" = "+distances.get(combinedRegions(132, 56)));
        println();

        println("Reading flows");
        readFlows();
        println(flowsRail.size()+" unique rail flows read\n");
        println(flowsRoad.size()+" unique road flows read\n");

        initialWarehouses(initialWarehouseCount);
        print("initial cost=");
        initialCostSum += allWarehouseCost();
        println(initialCostSum);
        currentCostSum = initialCostSum;

        root = new Group();
        root.maxHeight(Double.MAX_VALUE);
        root.maxWidth(Double.MAX_VALUE);

        placeNodes();

        pane.setCenter(root);
        setup(pane);

        Scene scene = new Scene(pane, stageWidth, stageHeight);

        primaryStage.setTitle("");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);

    }
    
}

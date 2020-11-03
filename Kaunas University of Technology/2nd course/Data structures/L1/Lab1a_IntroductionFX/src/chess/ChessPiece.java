/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package chess;

import java.util.ArrayList;
import javafx.scene.Node;

/**
 *
 * @author mikal
 */
public abstract class ChessPiece {
    
    protected int x;
    protected int y;
    protected boolean white;
    protected Node node;
    protected Node node2;
    
    public int getX(){ return x; }
    public int getY(){ return y; }
    public boolean isWhite() { return white; }
    public Node getNode() { return node; }
    public void setNode(Node n) { node = n; }
    public void setNode2(Node n) { node2 = n; }
    
    public ChessPiece(int x, int y, boolean white, Node node){
        this.x = x;
        this.y = y;
        this.white = white;
        this.node = node;
    }
    
    protected int posToCellId(int x, int y){
        if(x < 1 || x > 8) 
            return -1;
        else if(y<1 || y > 8) 
            return -1;
        return (x-1) + 8*(y-1);
    }
    
    abstract ArrayList<Integer> getMoves(int[] states);
}

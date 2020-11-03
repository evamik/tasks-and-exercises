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
public class King extends ChessPiece{
    
    public King(int x, int y, boolean white, Node node){
        super(x, y, white, node);
    }
    
    @Override
    public ArrayList<Integer> getMoves(int[] states){
        ArrayList<Integer> moves = new ArrayList<>();
        int myState = white ? 1 : 2;
        
        
        if(x > 1 && y < 8){
            int state = states[posToCellId(x-1, y+1)];
            if(state == 0 || state != myState) 
                moves.add((x-1)*10 + (y+1)); // down and left
        }
        if(y < 8){
            int state = states[posToCellId(x, y+1)];
            if(state == 0 || state != myState) 
                moves.add((x)*10 + (y+1)); // down
        }
        if(x < 8 && y < 8){
            int state = states[posToCellId(x+1, y+1)];
            if(state == 0 || state != myState) 
                moves.add((x+1)*10 + (y+1)); // down and right
        }
        if(x < 8){
            int state = states[posToCellId(x+1, y)];
            if(state == 0 || state != myState) 
                moves.add((x+1)*10 + (y)); // right
        }
        if(x < 8 && y > 1){
            int state = states[posToCellId(x+1, y-1)];
            if(state == 0 || state != myState) 
                moves.add((x+1)*10 + (y-1)); // up and right
        }
        if(y > 1){
            int state = states[posToCellId(x, y-1)];
            if(state == 0 || state != myState) 
                moves.add((x)*10 + (y-1)); // up
        }
        if(x > 1 && y > 1){
            int state = states[posToCellId(x-1, y-1)];
            if(state == 0 || state != myState) 
                moves.add((x-1)*10 + (y-1)); // up and left
        }
        if(x > 1){
            int state = states[posToCellId(x-1, y)];
            if(state == 0 || state != myState) 
                moves.add((x-1)*10 + (y)); // left
        }
        
        return moves;
    }
}

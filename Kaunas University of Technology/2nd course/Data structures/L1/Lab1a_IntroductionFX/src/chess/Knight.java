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
public class Knight extends ChessPiece{
    
    public Knight(int x, int y, boolean white, Node node){
        super(x, y, white, node);
    }
    
    @Override
    public ArrayList<Integer> getMoves(int[] states){
        ArrayList<Integer> moves = new ArrayList<>();
        int myState = white ? 1 : 2;
        
        if(x < 7) {
            if(y < 8){
                int state = states[posToCellId(x+2, y+1)];
                if(state == 0 || state != myState) 
                    moves.add((x+2)*10 + (y+1)); // 1 down 2 right
            }
            if(y > 1){
                int state = states[posToCellId(x+2, y-1)]; 
                if(state == 0 || state != myState)
                    moves.add((x+2)*10 + (y-1)); // 1 down 2 right
            }
        }
        if(x > 2) {
            if(y < 8){
                int state = states[posToCellId(x-2, y+1)];
                if(state == 0 || state != myState) 
                    moves.add((x-2)*10 + (y+1)); // 1 up 2 left
            }
            if(y > 1){
                int state = states[posToCellId(x-2, y-1)]; 
                if(state == 0 || state != myState)
                    moves.add((x-2)*10 + (y-1)); // 1 up 2 left
            }
        }
        if(y < 7) {
            if(x < 8){
                int state = states[posToCellId(x+1, y+2)];
                if(state == 0 || state != myState) 
                    moves.add((x+1)*10 + (y+2)); // 2 down 1 right
            }
            if(x > 1){
                int state = states[posToCellId(x-1, y+2)]; 
                if(state == 0 || state != myState)
                    moves.add((x-1)*10 + (y+2)); // 2 down 1 left
            }
        }
        if(y > 2) {
            if(x < 8){
                int state = states[posToCellId(x+1, y-2)];
                if(state == 0 || state != myState) 
                    moves.add((x+1)*10 + (y-2)); // 2 up 1 right
            }
            if(x > 1){
                int state = states[posToCellId(x-1, y-2)]; 
                if(state == 0 || state != myState)
                    moves.add((x-1)*10 + (y-2)); // 2 up 1 left
            }
        }
        
        return moves;
    }
}

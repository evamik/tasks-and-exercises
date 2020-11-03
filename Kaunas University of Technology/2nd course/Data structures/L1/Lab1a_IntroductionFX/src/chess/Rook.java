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
public class Rook extends ChessPiece{
    
    public Rook(int x, int y, boolean white, Node node){
        super(x, y, white, node);
    }
    
    @Override
    public ArrayList<Integer> getMoves(int[] states){
        ArrayList<Integer> moves = new ArrayList<>();
        int myState = white ? 1 : 2;
        
        if(x < 8) {
            for(int i = 1; i < 8-x+1; i++){
                int state = states[posToCellId(x+i, y)];
                if(state == 0 || state != myState) 
                    moves.add((x+i)*10 + (y)); // right
                else break;
            }
        }
        if(x > 1) {
            for(int i = 1; i < x; i++){
                int state = states[posToCellId(x-i, y)];
                if(state == 0 || state != myState) 
                    moves.add((x-i)*10 + (y)); // left
                else break;
            }
        }
        if(y > 1) {
            for(int i = 1; i < y; i++){
                int state = states[posToCellId(x, y-i)];
                if(state == 0 || state != myState) 
                    moves.add((x)*10 + (y-i)); // up
                else break;
            }
        }
        if(y < 8) {
            for(int i = 1; i < 8-y+1; i++){
                int state = states[posToCellId(x, y+i)];
                if(state == 0 || state != myState) 
                    moves.add((x)*10 + (y+i)); // down
                else break;
            }
        }
        
        return moves;
    }
}

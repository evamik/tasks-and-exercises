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
public class Pawn extends ChessPiece{
    
    public Pawn(int x, int y, boolean white, Node node){
        super(x, y, white, node);
    }
    
    @Override
    public ArrayList<Integer> getMoves(int[] states){
        ArrayList<Integer> moves = new ArrayList<>();
        int dir = white ? -1 : 1;
        if(y > 1 && y < 8) 
            if(states[posToCellId(x, y+dir)] == 0) 
                moves.add(x*10 + (y+dir)); // move once
        if(white && y == 7) 
            moves.add(x*10 + (y + dir*2)); // move twice on starting position as white
        else if(!white && y == 2) 
            moves.add(x*10 + (y + dir*2)); // move twice on starting position as black
        if(x > 1 && x < 8 && y > 1 && y < 8){ // if not touching border
            if(white){
                if(states[posToCellId(x+1, y+dir)] == 2) moves.add((x+1)*10 + y+dir); // capture on right
                if(states[posToCellId(x-1, y+dir)] == 2) moves.add((x-1)*10 + y+dir); // capture on left
            }
            else if(!white){
                if(states[posToCellId(x+1, y+dir)] == 1) moves.add((x+1)*10 + y+dir); // capture on right
                if(states[posToCellId(x-1, y+dir)] == 1) moves.add((x-1)*10 + y+dir); // capture on left
            }
        }
        
        return moves;
    }
}

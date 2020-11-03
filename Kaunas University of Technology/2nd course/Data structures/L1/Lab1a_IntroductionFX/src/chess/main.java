/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package chess;

import extendsFX.BaseGraphics;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Random;
import javafx.event.EventHandler;
import javafx.scene.input.MouseButton;
import javafx.scene.input.MouseEvent;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.stage.Stage;
import javafx.scene.Node;
import javafx.scene.control.Button;
import javafx.scene.shape.Rectangle;
import javafx.scene.text.Text;
import javafx.scene.text.TextBoundsType;
/**
 *
 * @author mikal
 */
public class main extends BaseGraphics {

    Button BoardButton;
    Button ChessButton;
    Button MoveButton;
    
    private double BorderWidth = 32;
    private double BorderHeight = 32;
    private double BoardW;
    private double BoardH;
    private double CellWidth;
    private double CellHeight;
    
    private HashMap<String, Character> PiecesUnicode = new HashMap<>();
    private ChessPiece[] Pieces = new ChessPiece[32];
    private int[] CellsStates = new int[64];
    private int Count = 0;
    private int movesCount = 0;
    private Node selectedPiece;
    ArrayList<Integer> moves;
    
    private void drawChessBoard(){
        BoardButton.setDisable(true);
        boolean isBright = true;
        
        for(int i = 0; i < 8; i++){
            gc.setFont(Font.font("Lucida Sans Unicode", BorderHeight));
            gc.setStroke(Color.BLACK);
            gc.strokeText((char) (i+65) + "", BorderWidth + (i+0.3)*CellWidth, BorderHeight*0.85);
        }
        for(int i = 0; i < 8; i++){
            gc.setFont(Font.font("Lucida Sans Unicode", BorderHeight));
            gc.setStroke(Color.BLACK);
            gc.strokeText((char) (i+65) + "", BorderWidth + (i+0.3)*CellWidth, canvasH-BorderHeight*0.15);
        }
        for(int i = 0; i < 8; i++){
            gc.setFont(Font.font("Lucida Sans Unicode", BorderHeight));
            gc.setStroke(Color.BLACK);
            gc.strokeText((i+1) + "", BorderWidth*0.3, BorderHeight + (i+0.65)*CellHeight);
        }
        for(int i = 0; i < 8; i++){
            gc.setFont(Font.font("Lucida Sans Unicode", BorderHeight));
            gc.setStroke(Color.BLACK);
            gc.strokeText((i+1) + "", canvasW-BorderWidth*0.8, BorderHeight + (i+0.65)*CellHeight);
        }
        
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                gc.setFill(Color.rgb(25, 25, 25));
                if(isBright) gc.setFill(Color.rgb(230, 230, 230));
                isBright = !isBright;
                gc.fillPolygon(
                        new double[] {BorderWidth + i*CellWidth, BorderWidth + (i+1)*CellWidth, BorderWidth + (i+1)*CellWidth, BorderWidth + i*CellWidth},
                        new double[] {BorderHeight + j*CellHeight, BorderHeight + j*CellHeight, BorderHeight + (j+1)*CellHeight, BorderHeight + (j+1)*CellHeight},
                        4);
            }
            isBright = !isBright;
        }
    }
    
    private void drawChess(){
        ChessButton.setDisable(true);
        for(int i = 0; i < 64; i++)
            CellsStates[i] = 0;
        // black
        drawChessPiece(false, "pawn", 1, 4);
        drawChessPiece(false, "pawn", 2, 2);
        drawChessPiece(false, "pawn", 3, 4);
        drawChessPiece(false, "pawn", 4, 2);
        drawChessPiece(false, "pawn", 5, 2);
        drawChessPiece(false, "pawn", 6, 3);
        drawChessPiece(false, "rook", 1, 1);
        drawChessPiece(false, "rook", 8, 1);
        drawChessPiece(false, "knight", 2, 1);
        drawChessPiece(false, "knight", 7, 1);
        drawChessPiece(false, "bishop", 3, 1);
        drawChessPiece(false, "bishop", 6, 1);
        drawChessPiece(false, "queen", 4, 1);
        drawChessPiece(false, "king", 5, 1);
        
        //white
        drawChessPiece(true, "pawn", 1, 5);
        drawChessPiece(true, "pawn", 2, 7);
        drawChessPiece(true, "pawn", 3, 7);
        drawChessPiece(true, "pawn", 4, 5);
        drawChessPiece(true, "pawn", 5, 7);
        drawChessPiece(true, "pawn", 6, 6);
        drawChessPiece(true, "rook", 1, 8);
        drawChessPiece(true, "rook", 7, 8);
        drawChessPiece(true, "knight", 2, 8);
        drawChessPiece(true, "knight", 4, 6);
        drawChessPiece(true, "bishop", 6, 5);
        drawChessPiece(true, "bishop", 6, 8);
        drawChessPiece(true, "queen", 4, 8);
        drawChessPiece(true, "king", 5, 8);
        
    }
    
    private void drawChessPiece(boolean isWhite, String type, int x, int y){ 
        Text text = new Text(BorderWidth + (x-1)*CellWidth, BorderHeight + (y-0.15)*CellHeight, PiecesUnicode.get(type).toString());
        text.setStrokeWidth(0.5);
        text.setStroke(Color.WHITE);
        text.setFill(Color.BLACK);
        
        if(isWhite) {
            text.setStroke(Color.BLACK);
            text.setFill(Color.WHITE);
        }
        
        text.setFont(Font.font("Lucida Sans Unicode", CellHeight));
        
        text.setBoundsType(TextBoundsType.VISUAL);
        Rectangle rect = new Rectangle(BorderWidth + (x-1)*CellWidth, BorderHeight + (y-1)*CellHeight, CellWidth, CellHeight);
        rect.setFill(Color.TRANSPARENT);
        
        nodes.add(text);
        Node n2 = nodes.get(nodes.size()-1);
        nodes.add(rect);
        Node node = nodes.get(nodes.size()-1);
        node.setOnMouseClicked(clickInfo);
        node.getProperties().put("id", Count);
        node.getProperties().put("type", type);
        
        CellsStates[posToCellId(x, y)] = (isWhite) ? 1 : 2;
        
        ChessPiece p = new Pawn(x, y, isWhite, nodes.get(nodes.size()-1)); // default pawn
        
        switch(type){
            case "knight": 
                p = new Knight(x, y, isWhite, nodes.get(nodes.size()-1));
                break;
            case "rook": 
                p = new Rook(x, y, isWhite, nodes.get(nodes.size()-1));
                break;
            case "bishop": 
                p = new Bishop(x, y, isWhite, nodes.get(nodes.size()-1));
                break;
            case "queen": 
                p = new Queen(x, y, isWhite, nodes.get(nodes.size()-1));
                break;
            case "king": 
                p = new King(x, y, isWhite, nodes.get(nodes.size()-1));
                break;
        }
        
        p.setNode2(n2);
        Pieces[Count++] = p;
    }
    
    private EventHandler<MouseEvent> clickInfo = e -> {
        double x = e.getX();
        double y = e.getY();
        if(e.getButton() == MouseButton.PRIMARY)
        {
            if(movesCount > 0) removeLastMoves();
            MoveButton.setDisable(true);
            ChessPiece cp = Pieces[(int)((Node)e.getSource()).getProperties().get("id")];
            selectedPiece = cp.node;
            moves = cp.getMoves(CellsStates);
            
            moves.forEach((move) -> {
                char[] digits = move.toString().toCharArray();
                int moveX = Integer.valueOf(digits[0]+"");
                int moveY = Integer.valueOf(digits[1]+"");
                Rectangle rect = new Rectangle(BorderWidth + (moveX-1)*CellWidth, BorderHeight + (moveY-1)*CellHeight, CellWidth, CellHeight);
                rect.setFill(Color.rgb(20, 200, 20, 0.5));
                rect.setStroke(Color.rgb(20, 100, 20, 1));
                rect.setStrokeWidth(3);
                nodes.add(rect);
                Node n = nodes.get(nodes.size()-1);
                n.getProperties().put("x", moveX);
                n.getProperties().put("y", moveY);
                n.getProperties().put("id", (int)((Node)e.getSource()).getProperties().get("id"));
                n.getProperties().put("type", (String)((Node)e.getSource()).getProperties().get("type"));
                n.setOpacity(0.5);
                n.setOnMouseEntered(event -> {
                    ((Node)event.getTarget()).setOpacity(1);
                });
                n.setOnMouseExited(event -> {
                    ((Node)event.getTarget()).setOpacity(0.5);
                });
                movesCount++;
            });
            Rectangle rect = new Rectangle(BorderWidth + (cp.x-1)*CellWidth, BorderHeight + (cp.y-1)*CellHeight, CellWidth, CellHeight);
            rect.setFill(Color.rgb(20, 200, 20, 0.5));
            nodes.add(0, rect);
            Node n = nodes.get(0);
            n.setOpacity(0.5);
            movesCount++;
            if(movesCount > 1){
                MoveButton.setDisable(false);
            }
        }
    };
    
    private void removeLastMoves(){
        for(int i = 1; i < movesCount; i++)
            nodes.remove(nodes.size()-1);
        nodes.remove(0);
        movesCount = 0;
    }
    
    private int posToCellId(int x, int y){
        return (x-1) + 8*(y-1);
    }
    
    private void movePiece(){
        Random rng = new Random();
        rng.setSeed(System.currentTimeMillis());
        Node dest = nodes.get(nodes.size()-rng.nextInt(movesCount-1)-1);
        ChessPiece cp = Pieces[(int)dest.getProperties().get("id")];
        int x = (int)dest.getProperties().get("x");
        int y = (int)dest.getProperties().get("y");
        cp.x = x;
        cp.y = y;
        x = (int)(BorderWidth + (x-1)*CellWidth);
        y = (int)(BorderHeight + (y-1)*CellHeight);
        cp.node.relocate(x, y);
        cp.node2.relocate(x, y);
        
            if(movesCount > 0) removeLastMoves();
            MoveButton.setDisable(true);
        //drawChessPiece(cp.white, (String)dest.getProperties().get("type"), (int)dest.getProperties().get("x"), (int)dest.getProperties().get("y"));
    }
    
    private void clearScreen(){
        nodes.clear();
        clearCanvas();
        Count = 0;
        movesCount = 0;
        
        ChessButton.setDisable(false);
        BoardButton.setDisable(false);
        MoveButton.setDisable(true);
    }
    
    @Override
    public void createControls() {
        addButton("clear", e -> clearScreen());
        BoardButton = addButton("draw chessboard", e -> drawChessBoard());
        ChessButton = addButton("draw chess", e -> drawChess());
        MoveButton = addButton("move", e -> movePiece());
        
        addNewHBox();
    }
    @Override
    public void start(Stage stage) throws Exception {
        stage.setTitle("Šachmatai");        
        setCanvas(Color.WHITE, 600, 600);
        super.start(stage);
        
        MoveButton.setDisable(true);
        
        BoardW = canvasW-BorderWidth*2;
        BoardH = canvasH-BorderHeight*2;
        CellWidth = BoardW / 8.0;
        CellHeight = BoardH / 8.0;
        
        PiecesUnicode.put("pawn", '\u265F');
        PiecesUnicode.put("rook", '\u265C');
        PiecesUnicode.put("knight", '\u265E');
        PiecesUnicode.put("bishop", '\u265D');
        PiecesUnicode.put("queen", '\u265B');
        PiecesUnicode.put("king", '\u265A');
    }
    public static void main(String[] args) {
        launch(args);
    }  
}

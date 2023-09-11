using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    public Move Think(Board board, Timer timer){  

        int[,] Piece_pos = {{//pawns
			 0,   0,   0,   0,   0,   0,   0,   0,
			50,  50,  50,  50,  50,  50,  50,  50,
			10,  10,  20,  30,  30,  20,  10,  10,
			 5,   5,  10,  25,  25,  10,   5,   5,
			 0,   0,   0,  20,  20,   0,   0,   0,
			 5,  -5, -10,   0,   0, -10,  -5,   5,
			 5,  10,  10, -20, -20,  10,  10,   5,
			 0,   0,   0,   0,   0,   0,   0,   0
		},
        {//bishop
			0,  0,  0,  0,  0,  0,  0,  0,
			5, 10, 10, 10, 10, 10, 10,  5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			0,  0,  0,  5,  5,  0,  0,  0
		},
        {//Knights
			-50,-40,-30,-30,-30,-30,-40,-50,
			-40,-20,  0,  0,  0,  0,-20,-40,
			-30,  0, 10, 15, 15, 10,  0,-30,
			-30,  5, 15, 20, 20, 15,  5,-30,
			-30,  0, 15, 20, 20, 15,  0,-30,
			-30,  5, 10, 15, 15, 10,  5,-30,
			-40,-20,  0,  5,  5,  0,-20,-40,
			-50,-40,-30,-30,-30,-30,-40,-50,
		},
        {//Queen
			-20,-10,-10,-10,-10,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5, 10, 10,  5,  0,-10,
			-10,  5,  5, 10, 10,  5,  5,-10,
			-10,  0, 10, 10, 10, 10,  0,-10,
			-10, 10, 10, 10, 10, 10, 10,-10,
			-10,  5,  0,  0,  0,  0,  5,-10,
			-20,-10,-10,-10,-10,-10,-10,-20,
		},
        {//king
			-20,-10,-10, -5, -5,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5,  5,  5,  5,  0,-10,
			-5,  0,  5,  5,  5,  5,  0, -5,
			0,  0,  5,  5,  5,  5,  0, -5,
			-10,  5,  5,  5,  5,  5,  0,-10,
			-10,  0,  5,  0,  0,  0,  0,-10,
			-20,-10,-10, -5, -5,-10,-10,-20
		}};
        Move[] moves = board.GetLegalMoves();
        
        //Console.WriteLine("Move a: " + moves[0]);
        //should have
        Move[,] childmoves = {{moves[0], moves[1]}};
        Move[] grandchildmoves;
        bool ai_white = board.IsWhiteToMove;
        int eval(){
            PieceList[] pieces = board.GetAllPieceLists();

            int white_peices = 0;
            int black_peices = 0;
            
            //Pawns, Knights, Bishops,Rooks, Queen, King
            int[] weights = {1, 3, 3, 5, 9, 100};
            int board_weight = 0;
            //peices are listed white then black so loops through the whites then the black
            for(int i = 0; i < 6; i++){
                white_peices += (pieces[i].Count * weights[i]);
            }
            for(int y = 0; y <6; y++){
                black_peices += (pieces[y+6].Count * weights[y]);
            }
            if(ai_white){
                board_weight = white_peices - black_peices;
            }else{
                board_weight = black_peices - white_peices;
            }

            int[] Get_piece_pos_weights(PieceList[] pieces, bool ai_white){
                int[] indices = new int[64];
                int weight = 0;
                int i = 0;
                int x = 0;
                if(ai_white){
                    i = 0;
                    x = 5;
                }else{
                    i = 5;
                    x = 10;
                }
                Console.WriteLine("I: " + i);
                
                //loop through each piece list in the peicelist[] array
                for(int z = i; z < x; z++){
                    Console.WriteLine("Z: " + z);
                    for(int j = 0; j < pieces[z].Count(); j++){
                        //Get each piece, and get its position, where it should be etc
                        Piece piece = pieces[z].GetPiece(j);
                        Square pos = piece.Square;
                        int index = pos.Index;
                        
                        Console.WriteLine(index);
                        int y = z;
                        if(y > 5){
                            y -= 5;
                        }
                        Console.WriteLine("y: " +  y + " array len:  " +  Piece_pos.Length/64);
                        Console.WriteLine("index: " + index);
                        weight += Piece_pos[y, index];
                        Console.WriteLine("Piece pos weight: " + Piece_pos[y, index]);
                    }

                }
                
                Console.WriteLine(" Weight: "+ weight);

                return indices;
            }
            Get_piece_pos_weights(board.GetAllPieceLists(), ai_white);
        
            return board_weight;
        
        }
        //for each move, write it into an array with its children, then do the same with the grandkids
        //THEN go and evaluate it all!

        Move[,] get_children(Move parent){
            //set all of the child results to {parent, null}
            //THEN index through the arrray and add the child
            //Run this for a single move, it goes and gives the info for all the children!
            if(parent == null){

                return null;
            }else{

                board.MakeMove(parent);
                Move[] new_moves = board.GetLegalMoves();
                
                childmoves = new Move[new_moves.Length,  2];
                for(int i=0; i < new_moves.Length; i++){
                    childmoves[i, 0] = parent;
                    childmoves[i, 1] = new_moves[i];
                    //Console.Write(childmoves[i, 0]);
                    //Console.WriteLine(childmoves[i, 1]);
                    
                }
                board.UndoMove(parent);
                return(childmoves);
            }
        }

        int[] evaluate_children(Move[,] child){
            int[] final_evals = new int[child.Length];
            for(int i=0; i < child.Length/2; i++){

                board.MakeMove(child[i,0]);
                board.MakeMove(child[i,1]);
                final_evals[i] = eval();
                board.UndoMove(child[i,1]);
                board.UndoMove(child[i,0]);
            }
            return(final_evals);
 

        }

        //loop throughh possible moves and get their children, evaluate those children
        //Once we evaluate the children, we get the best child and add it to the list with its relevant move
        //Then we just go to the next one.
        Move[] available_moves = board.GetLegalMoves();
        int[] move_tops = new int[available_moves.Length];
        Move[] top_moves = new Move[available_moves.Length];
        for(int i=0; i<available_moves.Length; i++){
            Move[,] children = get_children(available_moves[i]);
            int[] child_evals = evaluate_children(children);
            // we return the top eval from this move
            move_tops[i] = child_evals.Max();
            int index = Array.IndexOf(child_evals, child_evals.Max());
            Console.WriteLine("index : "+index + "legnth: " + available_moves.Length);
            top_moves[i] = available_moves[index];
        }
        int max_eval = move_tops.Max();
        int indexx = Array.IndexOf(move_tops, max_eval);
        Console.WriteLine(""+indexx+"");
        return(moves[indexx]);        
    }
}


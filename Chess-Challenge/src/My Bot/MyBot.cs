using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    public Move Think(Board board, Timer timer){  
        
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
            Console.WriteLine("Length: " + child.Length);
            for(int i=0; i < child.Length/2; i++){
                Console.WriteLine("i: "+i);

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
        for(int i=0; i<moves.Length; i++){
            Move[,] children = get_children(available_moves[i]);
            Console.WriteLine("Passing in legnth: "+children.Length);
            int[] child_evals = evaluate_children(children);
            // we return the top eval from this move
            move_tops[i] = child_evals.Max();
            int index = Array.IndexOf(child_evals, child_evals.Max());
            top_moves[i] = moves[index];
        }
        int max_eval = move_tops.Max();
        int indexx = Array.IndexOf(move_tops, max_eval);
        Console.WriteLine(""+indexx+"");
        return(moves[indexx]);        
    }
}


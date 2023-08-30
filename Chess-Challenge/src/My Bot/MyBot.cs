using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    public Move Think(Board board, Timer timer){  
        bool AI_is_white = board.IsWhiteToMove;
        //Fucntion to get the board weight in the favour/dis Favour of the AI 
        int get_board_weight(){

            //loop throguh the piece list and get white and black values

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
            if(AI_is_white){
                board_weight = white_peices - black_peices;
            }else{
                board_weight = black_peices - white_peices;
            }

            return board_weight;
        
        }
        Console.WriteLine(get_board_weight());

        //Func to get the current best move, basically looks through the moves and runs
        //board weight for each move and returns the best move


        //The main loop, gets the best move, makes it, starts again for depth x
        //Then runs a final board countup function for that branch, and adds 
        // that to the array 
    
        // call the main loop




        Move[] moves = board.GetLegalMoves();
        return moves[0];
    }
}
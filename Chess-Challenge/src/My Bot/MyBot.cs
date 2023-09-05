using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    public Move Think(Board board, Timer timer){  

        bool AI_is_white = board.IsWhiteToMove;
        Random random = new Random();
        Move[] moves = board.GetLegalMoves();
        //Fucntion to get the board weight in the favour/dis Favour of the AI 
        int get_board_weight(bool white){

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
            if(white){
                board_weight = black_peices-white_peices;
            }else{
                board_weight = white_peices-black_peices;
            }

            return board_weight;
        
        }
        int [] final_move_weights = new int[board.GetLegalMoves().Length];

        int max(int depth){
            if(depth == 0){
                if(board.IsWhiteToMove){
                    Console.WriteLine(get_board_weight(true));
                    return get_board_weight(true);
                }else{
                    Console.WriteLine(get_board_weight(false));
                    return get_board_weight(false);
                }
            }
            //does it ever make the moves??
            int max = -10000000;
            Move[] moves = board.GetLegalMoves();
            for(int i = 0; i < moves.Length; i++){
                int score = min(depth-1);
                if(score > max){
                    max = score;
                }
            }
            
            return max;

        }
        int min(int depth){
            if(depth ==0){
                if(board.IsWhiteToMove){
                    Console.WriteLine(get_board_weight(false));
                    return(get_board_weight(false));
                }else{
                    Console.WriteLine(get_board_weight(true));
                    return(get_board_weight(true));
                }
            }
            int min = -1000000;
            Move[] moves = board.GetLegalMoves();
            for(int i=0; i < moves.Length; i++){
                int score = max(depth-1);
                if(score < min){
                    min = score;
                }
            }
            return min;
        }

        Move[] possible_moves = board.GetLegalMoves();
        int[] board_weights = new int[possible_moves.Length];
        for(int i = 0; i < possible_moves.Length; i++){
               board_weights[i] = max(4);
        }
        int index_of_best_move=0;
        if(final_move_weights.Max() == 0){
            index_of_best_move = random.Next(0, possible_moves.Length);
            Console.WriteLine("Random");
        }else{
            index_of_best_move = Array.IndexOf(board_weights, final_move_weights.Max());
        }

        Console.WriteLine("move: " + index_of_best_move);
        // call the main loop
        Move[] legal_moves = board.GetLegalMoves();
        if(legal_moves[index_of_best_move].IsNull == true){
            return(legal_moves[random.Next(0, legal_moves.Length)]);
        }
        return legal_moves[index_of_best_move];
    }
}


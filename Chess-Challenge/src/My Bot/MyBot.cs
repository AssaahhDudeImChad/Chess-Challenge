using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    public Move Think(Board board, Timer timer){  

        int depth = 5;
        bool AI_is_white = board.IsWhiteToMove;
        Move[] done_moves = new Move[depth];
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

        //Func to get the current best move, basically looks through the moves and runs
        //board weight for each move and returns the best move
        Move get_best_move(bool white){
            Move[] legal_moves = board.GetLegalMoves();
            //For each move, make it and get the board value and unmake it
            int run_all_moves(){
                move legal_moves = board.GetLegalMoves()
                for(int i = 0; i < board.GetLegalMoves(); i++){
                    move move = board.GetLegalMoves()[i]
                    board.MakeMove(move)
                    done_moves[i] = move

            }
            for(int i = 0; i < legal_moves.Length; i++){
                board.MakeMove(legal_moves[i]);
                done_moves[i] = legal
            }
                //undo all the moves
                for(int j = done_moves.Length; j > 0; j--){
                    board.UndoMove(done_moves[j]);
                }
            } 
        }        // call the main loop
        Move move = get_best_move(true);
        Move[] legal_moves = board.GetLegalMoves();
        if(legal_moves[index_of_best_move].IsNull == true){
            return(legal_moves[random.Next(0, legal_moves.Length)]);
        }
        return legal_moves[index_of_best_move];
    }


//test for github

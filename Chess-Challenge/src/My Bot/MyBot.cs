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

        //Func to get the current best move, basically looks through the moves and runs
        //board weight for each move and returns the best move
        Move get_best_move(bool white){
            Move[] legal_moves = board.GetLegalMoves();
            
            int[] move_weights = new int[legal_moves.Length];
            for(int i = 0; i < legal_moves.Length; i++){
                //get the current weight, make the move, get the weight after, and compare
                int before_weight = get_board_weight(white);
                board.MakeMove(legal_moves[i]);
                int after_weight = get_board_weight(white);
                if(board.IsInCheck()){
                    //see if black king is in check or mate
                    after_weight += 100;
                }
                board.UndoMove(legal_moves[i]);

                int move_weight = (after_weight-before_weight);
                move_weights[i] = move_weight;
            }
            for(int x = 0; x < move_weights.Max(); x++){
                Console.WriteLine(move_weights[x]); 
            }
            int best_move_index = Array.IndexOf(move_weights, move_weights.Max());
            if(move_weights.Max() == 0 && legal_moves.Length > 16){
                best_move_index = random.Next(13, 16);//somthig between 13 and 16
            }
            return legal_moves[best_move_index];
        }   
        //main loop, for EACH move currentlt available, make it, then look at the best
        //make it and continue for depth x
        //Then runs a final board countup function for that branch, and adds 
        // that to the array 

        //for loop:
        //get best move, and make it
        //Get best move for white/ black relative to what the AI is and who's turn it is
        //Do this for x times, and do a final board countup, append that to the main list
        int depth = 10;
        int [] final_move_weights = new int[board.GetLegalMoves().Length];

        for(int x = 0; x < board.GetLegalMoves().Length; x++){
            //board.MakeMove(board.GetLegalMoves()[x]);
            //Console.WriteLine("Starting move: " + board.GetLegalMoves()[x]);
            Move[] done_moves = new Move[depth];
            Move picked_move = board.GetLegalMoves()[0];
            done_moves[0] = board.GetLegalMoves()[x];
            for(int i = 1; i < depth; i++){
                
                //if it's odd, look at OUR best move, if othersie look at THEIR best move
                if(i % 2 == 0){
                    //even
                    //find THEIR best move and make it
                    if(AI_is_white){
                        //find best move white=false
                       picked_move = get_best_move(false);

                    }else{
                        //find best move white=true
                       picked_move =get_best_move(true);
                    }
                    }else{
                    //odd
                    //find OUR best move and make it
                    if(AI_is_white){
                        //find best move white=true
                       picked_move = get_best_move(true);
                    }else{
                        //fine best move whtie=false
                       picked_move = get_best_move(false);
                    }
                        
                    }
                //make that move
                board.MakeMove(picked_move);
                done_moves[i]  = picked_move;
                //re-start the loop and at the END run a countup for the loop 
            }
            //countup the board values
            //Something is wrong here.... they are blank ints...
            final_move_weights[x] += get_board_weight(AI_is_white);
            for(int i = done_moves.Length-1; i > 0; i--){
                board.UndoMove(done_moves[i]);
            }

        } 
        //TODO;
        //ITs not undoing the moves in the right order
        //this means it registers the WRONG current avaialbel mvoes, it's picking the best ones but 
        //it isnt executing the right ones if that make sense
        //find highest score in array
        int index_of_best_move = Array.IndexOf(final_move_weights, final_move_weights.Max());
        // call the main loop
        Move move = get_best_move(true);
        Move[] legal_moves = board.GetLegalMoves();
        //Console.WriteLine("moves availabel: "+legal_moves.Length);
        //Console.WriteLine("index of best move: " +(index_of_best_move-1));
        
        return legal_moves[index_of_best_move];
    }
}

//test for github
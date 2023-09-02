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

        //Okay this is probably wrong but I need to just get the final board values for every single possible board
        //looking x moves ahead
        Move minimax(depth, child, white){

            if(depth == 0 || board.IsInCheckmate){
                return null;
            }

            if(white){
                best_board = -10000000000;

                for(int i = 0; i < board.GetLegalMoves().Legnth; i++){
                    board_eval = minimax(board.GetLegalMoves()[i], depth-1, false)
                }
            }else{
                
            }
            
        }
        }
    }


//test for github

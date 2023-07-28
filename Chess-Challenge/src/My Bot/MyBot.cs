using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    //null, pawn, knight, bishop, rook, queen, king
    public Move Think(Board board, Timer timer){
        bool me_white = true;
        bool they_white = false;
        if(board.IsWhiteToMove){
            me_white = true;
            they_white = false;
        }else{
            they_white = true;
            me_white = false;
        }
        Move[] moves = board.GetLegalMoves();
        int[] scores = new int[moves.Length];
        int[] values = {30, 50, 70, 60, 90, 100};
        int move_weight(Board board, int[] values, Move[] moves, bool they_white){
            //args = the board, the values, if the opponent is white, 
            //and who needs to be counted, T for us, F for opponent
            int countthey = 0;
            int countme = 0;

            PieceList[] pieces = board.GetAllPieceLists();
            int countup(int[] values, bool they_white){
                int white_count = 0;
                int black_count = 0;
                for(int i=0; i<6; i++){
                    white_count +=(values[i]*pieces[i].Count);     
                }
            
                for(int x=0; x<6; x++){
                    black_count +=(values[x]*pieces[x+6].Count);   
                }
            
            if(they_white){
                countme = black_count;
                countthey = white_count;
            }
            return(countthey);
            }


            int before = countup(values, they_white);
            for(int i=0; i++<moves.Length;){
                board.MakeMove(moves[i]);
            }
            int after = countup(values, they_white);
            bool ischeck = board.IsInCheck();
            bool ismate = board.IsInCheckmate();
            //the weight modifier from captures
            int capture_weight = (before-after);
            Console.WriteLine(capture_weight);
            //getting the weight modifier from checks
            int check_weight = ((Convert.ToInt32(ischeck)+Convert.ToInt32(ismate))*100);
            int[]
            return(check_weight+capture_weight);
            
        }

        Move search_for_moves(Board board, bool they_white, int depth){
            
            Move[] moves = board.GetLegalMoves();
            int [] weights = new int[moves.Length];
            for(int i=0; i< moves.Length;){
                Move[] done_moves =new Move[depth];
                for(int j=0; j++ <depth;){
                    board.MakeMove(moves[move_weight(board, moves)]);
                    done_moves[j] = moves[GetHighestScore(board, moves)];
                    
                }

                weights[i] = move_weight(board, values, done_moves, they_white);
                for(int k=depth; k-- >depth;){
                    board.UndoMove(done_moves[k]);
                }  
            }
            int highest_score = weights.Max();
            int highest_index = Array.IndexOf(weights, highest_score);
            
            if(weights[highest_index] < 0){
                Random random = new Random();
                int random_move = random.Next(0, moves.Length);
                return moves[random_move];
            }
            else{
                return moves[highest_index];
            }
        }
        return(search_for_moves(board, they_white, 3));
        }


}


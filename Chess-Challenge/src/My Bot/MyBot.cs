using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
 
    //null, pawn, knight, bishop, rook, queen, king
    public Move Think(Board board, Timer timer){   
        
        bool color = board.IsWhiteToMove;
        Move[] moves = board.GetLegalMoves();
        int[] scores = new int[moves.Length];
        int[] values = {30, 50, 70, 60, 90, 100};

        int move_weight(Board board, int[] values, Move move){
            int count = 0;
            PieceList[] pieces = board.GetAllPieceLists();
            int countup(int[] values){
                for(int i=0; i<6; i++){
                    count +=(values[i]*pieces[i].Count);     
                }
                for(int x=0; x<6; x++){
                    count -=(values[x]*pieces[x+6].Count);
                    
                }
                return count;
                
            }
            
            int before = countup(values);
            board.MakeMove(move);
            int after = countup(values);
            board.UndoMove(move);
            //the weight modifier from captures
            int capture_weight = (before-after);
            //getting the weight modifier from checks

            board.MakeMove(move);
            bool ischeck = board.IsInCheck();
            bool ismate = board.IsInCheckmate();
            board.UndoMove(move);
            int check_weight = ((Convert.ToInt32(ischeck)+Convert.ToInt32(ismate))*100);

            return(check_weight+capture_weight);
            
        }
        int GetHighestScore(Board board, Move[] moves, int[] scores, int[] values){
           
            for(int i = 0; i++ < moves.Length-1;){
                scores[i] += move_weight(board, values, moves[i]);
            }
            int highest_score = scores.Max();
            int highest_index = Array.IndexOf(scores, highest_score);
            return(highest_index);
        }
        
        Move check_moves(Board board, Move[] moves, bool Whiteturn){
            if(scores[GetHighestScore(board, moves, scores, values)] == 0){
                Random random = new Random();
                int random_move = random.Next(0, moves.Length);

                return moves[random_move];
            }else{
            
                return moves[GetHighestScore(board, moves, scores, values)];
            }
        }
        return(check_moves(board, moves, true));

    }
}

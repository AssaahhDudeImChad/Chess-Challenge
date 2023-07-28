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
        int move_weight(Board board, int[] values, Move move, bool they_white){
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
            board.MakeMove(move);
            int after = countup(values, they_white);
            board.UndoMove(move);
            //the weight modifier from captures
            int capture_weight = (before-after);
            Console.WriteLine(capture_weight);
            //getting the weight modifier from checks
     
            board.MakeMove(move);
            bool ischeck = board.IsInCheck();
            bool ismate = board.IsInCheckmate();
            board.UndoMove(move);
            int check_weight = ((Convert.ToInt32(ischeck)+Convert.ToInt32(ismate))*100);
            return(check_weight+capture_weight);
            
        }
        int GetHighestScore(Board board, Move[] moves, int[] scores, int[] values, bool they_white){
           
            for(int i = 0; i++ < moves.Length-1;){
                scores[i] += move_weight(board, values, moves[i], they_white);
            }
            int highest_score = scores.Max();
            int highest_index = Array.IndexOf(scores, highest_score);
            return(highest_index);
        }
        int index = GetHighestScore(board, moves, scores, values, they_white);
        if(scores[index] ==0){
            Random random = new Random();
            int random_move = random.Next(0, moves.Length);

            return moves[random_move];
        }else{
        
            return moves[GetHighestScore(board, moves, scores, values, they_white)];
        }
    


    }
}

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
                   
                for(int x=6; x<12; x++){
                    count -=(values[x-6]*pieces[x].Count);
                }
                }
                Console.WriteLine(count);
                return count;
                
            }
            int before = countup(values);
            board.MakeMove(move);
            int after = countup(values);
            board.UndoMove(move);
            return(after-before);
            
            
        }

        static int ischeck_or_mate(Board board, Move move){
            board.MakeMove(move);
            bool ischeck = board.IsInCheck();
            bool ismate = board.IsInCheckmate();
            board.UndoMove(move);
            if(ischeck){
                return(1);
            }else if(ismate){
                return(2);
            }else{
                return(0);
            }

        }
        int GetHighestScore(Board board, Move[] moves, int[] scores, int[] values){
           
            for(int i = 0; i++ < moves.Length-1;){
                scores[i] += move_weight(board, values, moves[i]);
                int check_mate = ischeck_or_mate(board, moves[i]);
                if(check_mate == 1){
                    scores[i] += 250;
                }else if(check_mate == 2){
                    scores[i] += 1000000000;
                }
            }
            int highest_score = scores.Max();
            int highest_index = Array.IndexOf(scores, highest_score);
            return(highest_index);
        }
       
        int index = GetHighestScore(board, moves, scores, values);
        
        if(scores[index] == 0){
            Random random = new Random();
            int random_move = random.Next(0, moves.Length);

            return moves[random_move];
        }else{
         
            return moves[index];
        }

    }
}

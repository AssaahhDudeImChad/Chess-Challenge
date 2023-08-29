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
            Console.WriteLine("checking move: "+ move);
            board.MakeMove(move);
            int after = countup(values);
            board.UndoMove(move);
            //the weight modifier from captures
            int capture_weight = before-after;
            //getting the weight modifier from checks

            board.MakeMove(move);
            bool ischeck = board.IsInCheck();
            bool ismate = board.IsInCheckmate();
            board.UndoMove(move);
            int check_weight = ((Convert.ToInt32(ischeck)+Convert.ToInt32(ismate))*100);
            return check_weight+capture_weight;
            
        }
        int GetHighestScore(Board board, int[] scores, int[] values){
            Move[] moves = board.GetLegalMoves();
            for(int i = 0; i++ < moves.Length-1;){
                scores[i] += move_weight(board, values, moves[i]);
            }
            int highest_score = scores.Max();
            int highest_index = Array.IndexOf(scores, highest_score);
            return(highest_index);
        }


        int[] Search_moves(int depth){
            Move[] done_moves = new Move[depth];
            int[] scores = new int[board.GetLegalMoves().Length];

            for(int i = 0;i < depth;i++){

                Move[] moves = board.GetLegalMoves();
                int index = GetHighestScore(board,scores, values);
                Console.WriteLine("In depth" + i +"looking through move" + moves[index]);
                board.MakeMove(moves[index]);

                done_moves[i] = moves[index];
                scores[i] += move_weight(board, values, moves[index]);
                
            }
            for(int j = depth; j >  0;j--){
                board.UndoMove(done_moves[j]);
            }
            return(scores);

        }
        Search_moves(1);
        int highest_score = scores.Max();
        if(highest_score ==0){
            Random random = new Random();
            int random_move = random.Next(0, moves.Length);

            return moves[random_move];
        }else{
            int index_of_highest = Array.IndexOf(scores, highest_score);
            return moves[index_of_highest]; 
    


    }
}
}

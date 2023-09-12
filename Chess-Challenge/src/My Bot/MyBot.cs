using ChessChallenge.API;
using System.Collections;
using System;
using System.Linq;


public class MyBot : IChessBot
{   
    int num_of_turns = 0;
    public Move Think(Board board, Timer timer){  
        num_of_turns += 1;

        int[,] Piece_pos = {{//pawns
	        0,   0,   0,   0,   0,   0,   0,   0,
			50,  50,  50,  50,  50,  50,  50,  50,
			10,  10,  20,  30,  30,  20,  10,  10,
			 5,   5,  10,  25,  25,  10,   5,   5,
			 0,   0,   0,  35,  35,   0,   0,   0,
			 5,  -5, -10,   -10,   -10, -10,  -5,   5,
			 5,  10,  10, -20, -20,  10,  10,   5,
			 0,   0,   0,   0,   0,   0,   0,   0
		},
        {//bishop
			0,  0,  0,  0,  0,  0,  0,  0,
			5, 10, 10, 10, 10, 10, 10,  5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			0,  0,  0,  5,  5,  0,  0,  0
		},
        {//Knights
			-50,-40,-30,-30,-30,-30,-40,-50,
			-40,-20,  0,  0,  0,  0,-20,-40,
			-30,  0, 10, 15, 15, 10,  0,-30,
			-30,  5, 15, 20, 20, 15,  5,-30,
			-30,  0, 15, 20, 20, 15,  0,-30,
			-30,  5, 10, 15, 15, 10,  5,-30,
			-40,-20,  0,  5,  5,  0,-20,-40,
			-50,-40,-30,-30,-30,-30,-40,-50,
		},
        {//Queen
			-20,-10,-10,-10,-10,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5, 10, 10,  5,  0,-10,
			-10,  5,  5, 10, 10,  5,  5,-10,
			-10,  0, 10, 10, 10, 10,  0,-10,
			-10, 10, 10, 10, 10, 10, 10,-10,
			-10,  5,  0,  0,  0,  0,  5,-10,
			-20,-10,-10,-10,-10,-10,-10,-20,
		},
        {//king
			-20,-10,-10, -5, -5,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5,  5,  5,  5,  0,-10,
			-5,  0,  5,  5,  5,  5,  0, -5,
			0,  0,  5,  5,  5,  5,  0, -5,
			-10,  5,  5,  5,  5,  5,  0,-10,
			-10,  0,  5,  0,  0,  0,  0,-10,
			-20,-10,-10, -5, -5,-10,-10,-20
		}};
        Move[] moves = board.GetLegalMoves();
        
        //Console.WriteLine("Move a: " + moves[0]);
        //should have
        Move[,] childmoves = {{moves[0], moves[1]}};
        Move[] grandchildmoves;
        bool ai_white = board.IsWhiteToMove;
        int eval(){
            //if there are no available moves because of a mate then obviously pick that move
            PieceList[] pieces = board.GetAllPieceLists();

            int white_peices = 0;
            int black_peices = 0;
            
            //Pawns, Knights, Bishops,Rooks, Queen, King
            int[] weights = {10, 30, 30, 50, 90, 100};
            int board_weight = 0;
            //peices are listed white then black so loops through the whites then the black
            for(int i = 0; i < 6; i++){
                white_peices += (pieces[i].Count * weights[i]);
            }
            for(int y = 0; y <6; y++){
                black_peices += (pieces[y+6].Count * weights[y]);
            }
            if(ai_white){
                board_weight = white_peices - black_peices;
            }else{
                board_weight = black_peices - white_peices;
            }

            int Get_piece_pos_weights(PieceList[] pieces, bool ai_white){
                
                int weight = 0;
                int i = 0;
                int x = 0;
                if(ai_white){
                    i = 0;
                    x = 5;
                }else{
                    i = 5;
                    x = 10;
                }
                
                //loop through each piece list in the peicelist[] array
                for(int z = i; z < x; z++){
                    for(int j = 0; j < pieces[z].Count(); j++){
                        //Get each piece, and get its position, where it should be etc
                        Piece piece = pieces[z].GetPiece(j);
                        Square pos = piece.Square;
                        int index = pos.Index;
                        
                        int y = z;
                        if(y > 4){
                            y -= 5;
                        }
                        weight += Piece_pos[y, index];
                    }

                }

                return weight;
            }
            int pos_weights = Get_piece_pos_weights(board.GetAllPieceLists(), ai_white);
            board_weight += pos_weights;
            return board_weight;
        
        }
        //We need to see all of the moves, make them, get the evaluation of that board 
        //and pick the move with the best evaluation in their favour, and pick that, make it one and then 
        //see what our best move from THAT board is


        Move find_best_move(){
            Move[] done_moves = new Move[3];
            //set all of the child results to {parent, null}
            //THEN index through the arrray and add the child
            //Run this for a single move, it goes and gives the info for all the children!
            Move[] moves = board.GetLegalMoves();
            int[] max_evals = new int[moves.Length];
            for(int i = 0; i < moves.Length; i++){
                //we look at all of the availabel moves now(moves for us) and make them
                board.MakeMove(moves[i]);
                done_moves[0] = moves[i];
                Move[] new_moves= board.GetLegalMoves();
                int[] new_evals = new int[new_moves.Length];
                for(int j = 0; j < new_moves.Length; j ++){
                    //then we do the same with all the oponants moves and save the result from the best one
                    board.MakeMove(new_moves[j]); 
                    new_evals[j] = eval();
                    board.UndoMove(new_moves[j]);
                }
                //NEW EVALS is doesnt have it??? 
                //then we finally do it for the best moves from that, and boom.
                int index_best_oppopnant_move = Array.IndexOf(new_evals, new_evals.Min());
                Console.WriteLine("possile op moves: " + new_moves.Length + "best: " + index_best_oppopnant_move);
                board.MakeMove(new_moves[index_best_oppopnant_move]);
                done_moves[1] = new_moves[index_best_oppopnant_move];
                Move[] newer_moves = board.GetLegalMoves();
                int[] evals = new int[newer_moves.Length];
                for(int j = 0; j < newer_moves.Length; j++){
                    board.MakeMove(newer_moves[j]);
                    evals[j] = eval();
                    board.UndoMove(newer_moves[j]);
                }
                max_evals[i] = evals.Max();
                for(int k = 2; k > -1; k--){
                    board.UndoMove(done_moves[k]);
                }     
            }
            
            int max_eval = max_evals.Max();
            return moves[Array.IndexOf(max_evals, max_eval)];
        }

        return find_best_move();

        //loop throughh possible moves and get their children, evaluate those children
        //Once we evaluate the children, we get the best child and add it to the list with its relevant move
        //Then we just go to the next one
    }

}


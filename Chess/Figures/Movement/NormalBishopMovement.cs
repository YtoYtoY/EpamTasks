﻿using Chess.Classes;
using Chess.Enum;
using Chess.Interfaces;
using Chess.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Figures.Movement
{
    public class NormalBishopMovement : IMovement
    {
        private const string BishopInvalidMove = "Figure can move only diagonally!";

        public void ValidateMove(IFigure figure, IBoard board, Move move)
        {
            var rowDistance = Math.Abs(move.From.Row - move.To.Row);
            var colDistance = Math.Abs(move.From.Col - move.To.Col);

            var other = figure.Color == ChessColor.White ? ChessColor.Black : ChessColor.White;

            if (rowDistance != colDistance)
            {
                throw new InvalidOperationException(BishopInvalidMove);
            }

            var from = move.From;
            var to = move.To;

            int rowIndex = from.Row;
            char colIndex = from.Col;

            int rowDirection = from.Row < to.Row ? 1 : -1;
            char colDirection = (char)(from.Col < to.Col ? 1 : -1);

            while (true)
            {
                rowIndex += rowDirection;
                colIndex += colDirection;

                if (to.Row == rowIndex && to.Col == colIndex)
                {
                    var figureAtPositon = board.GetFigureAtPosition(to);

                    if (figureAtPositon != null && figureAtPositon.Color == figure.Color)
                    {
                        throw new InvalidOperationException(GlobalErrorMessages.FigureOnTheWayErrorMessage);
                    }
                    else
                    {
                        return;
                    }
                }

                var position = Position.FromChessCoordinates(rowIndex, colIndex);
                var figureAtPosition = board.GetFigureAtPosition(position);

                if (figureAtPosition != null)
                {
                    throw new InvalidOperationException(GlobalErrorMessages.FigureOnTheWayErrorMessage);
                }
            }
        }
    }
}

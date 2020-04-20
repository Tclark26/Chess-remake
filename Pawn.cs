using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private bool mIsFirstMove = true;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        //base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        //Reset
        mIsFirstMove = true;


        //Pawn stuff
        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        //GetComponent<Image>().spite = Resources.Load<Sprite>("T_Pawn");
    }

    protected override void Move()
    {
        base.Move();

        mIsFirstMove = false;
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState cellState = cellState.None;
        cellState = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);

        if (cellState == targetState)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }
        return false;
    }

    private void CheckForPromotion()
    {
   
    }

    protected override void CheckPathing()
    {
        //Target postion
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Top left
        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellState.Enemy);

        //Forward
        if (MatchesState(currentX,currentY + mMovement.y, CellState.Free))
        {
            //If the first forward cell is freem and the first move, check for next
            if(mIsFirstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellState.Free);
            }
        }
        //Top right
        MatchesState(currentX + mMovement.z, currentY + mMovement.z, CellState.Enemy);
    }

}

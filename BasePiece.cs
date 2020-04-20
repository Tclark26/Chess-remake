using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GotComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();

    }

    public virtual void Place(Cell newCell)
    {
        //Cell stuff
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        
        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();

        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        //Clear current cell
		mCurrentCell.mCurrentPiece = null;

        //Remove piece
        gameObject.SetActive(false);
    }

    
    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        //Target positon
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Check each cell
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            //Get the state ofthe target cell
            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            //If emeny, add to list, break
            if (cellState == CellState.Ememy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }

            //If the cell is not free, break
            if (cellState != cellState.Free)
                break;

            //Add to list
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }

    protected virtual void CheckPathing()
    {
        //Horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        //Vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        //Upper diagonal
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        //Lower diagonal
        CreateCellPath(-1, -1, mMovement.z); 
        CreateCellPath(1, -1, mMovement.z);

    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            tell.mOutlineImage.enabled = true;
    }

    protected void ClearCells()
    {

    }

    protected virtual void Move()
    {
        //If there is an emeny piece, remove it
		mTargetCell.RemovePiece();

        //Clear current
		mCurrentCell.mCurrentPiece = null;

        //Switch cells
		mCurrentCell = mTargetCell;
		mCurrentCell.mCurrentPiece = this;

        //Move on board
		transform.position = mCurrentCell.transform.position;
		mTargetCell = null;
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        //Test for cells
        CheckPathing();

        //Show valid cells
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        //follow pointer
        transform.position += (Vector3)eventData.delta;

        //Check for overlapping available squares
        foreach(Cell cell in mHighlightedCells)
		{
            if(RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
			{
				mTargetCell = cell;
				break;
			}
            // If mouse is not in highlighted, no valid move 
			mTargetCell = null;
		}

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
		base.OnEndDrag(eventData);

		ClearCells();

        // Return to orginal position

		if (!mTargetCell)
		{
			mRectTransform.position = mCurrentCell.gameObject.transform.position;
			return;
		}
        // Move to new cell
		Move();

        //End turn
        mPieceManager.SwitchSides(mColor);
    }
   
}

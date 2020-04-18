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

    }

    public virtual void Place(Cell newCell)
    {

    }

    public void Reset()
    {

    }

    public virtual void Kill()
    {
		mCurrentCell.mCurrentPiece = null;

        gameObject.SetActive
    }

    
    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
     
    }

    protected virtual void CheckPathing()
    {
 
    }

    protected void ShowCells()
    {

    }

    protected void ClearCells()
    {

    }

    protected virtual void Move()
    {
		mTargetCell.RemovePiece();
		mCurrentCell.mCurrentPiece = null;

		mCurrentCell = mTargetCell;
		mCurrentCell.mCurrentPiece = this;

		transform.position = mCurrentCell.transform.position;
		mTargetCell = null;
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {

    }

    public override void OnDrag(PointerEventData eventData)
    {
        foreach(Cell cell in mHighlightedCells)
		{
            if(RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
			{
				mTargetCell = cell;
				break;
			}
			mTargetCell = null;
		}

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
		base.OnEndDrag(eventData);
		ClearCells();
		if (!mTargetCell)
		{
			mRectTransform.position = mCurrentCell.gameObject.transform.position;
			return;
		}
		Move();
    }
   
}

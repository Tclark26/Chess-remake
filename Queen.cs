using UnityEngine;
using UnityEngine.UI;

public class Queen : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        //base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        //Bishop stuff
        mMovement = new Vector3Int(7, 7, 7);
        //GetComponent<Image>().spite = Resources.Load<Sprite>("T_Queen");
    }
}

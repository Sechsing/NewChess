using UnityEngine;
using ChessSharp.Pieces;

public class PromotionOption : MonoBehaviour
{
    public PawnPromotion promotionType;
    private System.Action<PawnPromotion> callback;

    public void Init(PawnPromotion type, System.Action<PawnPromotion> onClick)
    {
        promotionType = type;
        callback = onClick;
    }

    private void OnMouseDown()
    {
        callback?.Invoke(promotionType);
    }
}

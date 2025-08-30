using UnityEngine;
using UnityEngine.EventSystems;
using ChessSharp.Pieces;

public class PromotionOption : MonoBehaviour, IPointerClickHandler
{
    public PawnPromotion promotionType;
    private System.Action<PawnPromotion> callback;

    public void Init(PawnPromotion type, System.Action<PawnPromotion> onClick)
    {
        promotionType = type;
        callback = onClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        callback?.Invoke(promotionType);
    }
}

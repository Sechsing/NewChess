using UnityEngine;
using UnityEngine.EventSystems;

public class BoardCell : MonoBehaviour, IPointerClickHandler
{
    public int row;
    public int col;
    public BoardInputHandler boardInputHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (boardInputHandler != null)
        {
            boardInputHandler.OnCellClicked(col, row);
        }
    }
}

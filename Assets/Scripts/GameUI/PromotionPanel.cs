using System;
using UnityEngine;
using UnityEngine.UI;
using ChessSharp;
using ChessSharp.Pieces;

public class PromotionPanel : MonoBehaviour
{
    [Header("Panel Setup")]
    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;

    [Header("Piece Prefabs")]
    public GameObject whiteQueenPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteKnightPrefab;

    public GameObject blackQueenPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackKnightPrefab;

    public Transform panelParent; // Set this to a UI panel or world-space container

    private Action<PawnPromotion> onPromotionChosen;
    private Player currentPlayer;

    public void Show(Player player, Action<PawnPromotion> callback)
    {
        gameObject.SetActive(true);
        currentPlayer = player;
        onPromotionChosen = callback;

        // Clear previous children
        foreach (Transform child in panelParent)
            Destroy(child.gameObject);

        bool isWhite = player == Player.White;

        GameObject[] piecePrefabs = isWhite
            ? new[] { whiteQueenPrefab, whiteRookPrefab, whiteBishopPrefab, whiteKnightPrefab }
            : new[] { blackQueenPrefab, blackRookPrefab, blackBishopPrefab, blackKnightPrefab };

        PawnPromotion[] promotionTypes = {
            PawnPromotion.Queen,
            PawnPromotion.Rook,
            PawnPromotion.Bishop,
            PawnPromotion.Knight
        };

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            GameObject bg = Instantiate(i % 2 == 0 ? lightSquarePrefab : darkSquarePrefab, panelParent);
            GameObject piece = Instantiate(piecePrefabs[i], bg.transform);
            piece.transform.localPosition = Vector3.zero;

            Button btn = bg.AddComponent<Button>();
            PawnPromotion promotion = promotionTypes[i];
            btn.onClick.AddListener(() =>
            {
                onPromotionChosen?.Invoke(promotion);
                Hide();
            });
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

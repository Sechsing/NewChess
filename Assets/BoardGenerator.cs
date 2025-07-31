using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;
    public float squareSize = 1f;

    void Start()
    {
        SpriteRenderer sr = lightSquarePrefab.GetComponent<SpriteRenderer>();
        float squareSize = sr.bounds.size.x;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                GameObject squarePrefab = (row + col) % 2 == 0 ? lightSquarePrefab : darkSquarePrefab;
                Vector2 position = new Vector2(col * squareSize, row * squareSize);
                Instantiate(squarePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
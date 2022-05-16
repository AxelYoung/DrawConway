using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBoard : MonoBehaviour {
    CellGame cellGame;
    Texture2D boardTexture;
    SpriteRenderer spriteRenderer;

    public Color backgroundColor;

    void Start() {
        cellGame = GetComponent<CellGame>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CreateBoard();
    }

    void CreateBoard() {
        boardTexture = new Texture2D(cellGame.boardSize, cellGame.boardSize);
        for (int x = 0; x < cellGame.boardSize; x++) {
            for (int y = 0; y < cellGame.boardSize; y++) {
                boardTexture.SetPixel(x, y, backgroundColor);
            }
        }
        CreateTexture();
    }

    public void UpdateBoard(List<Vector2Int> cells, bool playerUpdate = false) {
        foreach (Vector2Int cellPos in cells) {
            Cell cell = cellGame.cells[cellPos.x, cellPos.y];
            if (cell.state) {
                boardTexture.SetPixel(cellPos.x, cellPos.y, cell.color);
            } else {
                boardTexture.SetPixel(cellPos.x, cellPos.y, backgroundColor);
            }
        }
        CreateTexture();
    }

    void CreateTexture() {
        boardTexture.Apply();
        Sprite sprite = Sprite.Create(boardTexture, new Rect(0, 0, boardTexture.width, boardTexture.height), new Vector2(0.5f, 0.5f), 1);
        sprite.texture.filterMode = FilterMode.Point;
        spriteRenderer.sprite = sprite;
    }
}

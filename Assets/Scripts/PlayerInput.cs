using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    CellGame cellGame;
    CellBoard cellBoard;

    public Color32 color;

    // Start is called before the first frame update
    void Start() {
        cellGame = GetComponent<CellGame>();
        cellBoard = GetComponent<CellBoard>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (mouseBoardPosition().x != -1) {
                cellGame.SetCell(mouseBoardPosition().x, mouseBoardPosition().y, true, color, true);
            }
        }
    }

    public void SetColor(Color newColor) {
        color = newColor;
    }

    Vector2Int mouseBoardPosition() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int relativePosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x) + cellGame.boardSize / 2, Mathf.FloorToInt(mousePosition.y) + cellGame.boardSize / 2);
        if (relativePosition.x >= 0 && relativePosition.y >= 0 && relativePosition.x < cellGame.boardSize && relativePosition.y < cellGame.boardSize) {
            return relativePosition;
        } else {
            return new Vector2Int(-1, -1);
        }
    }
}

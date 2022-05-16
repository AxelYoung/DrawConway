using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGame : MonoBehaviour {

    CellBoard cellBoard;

    public int boardSize = 256;
    public bool[] cellDeathParam;
    public bool[] cellGrowthParam;

    public Cell[,] cells;

    List<Vector2Int> cellChanges = new List<Vector2Int>();
    List<Color> cellColors = new List<Color>();

    bool running;
    float curTime;
    float frameTime = 0.01f;

    PlayerInput playerInput;

    int currentDeathParamIndex = 0;
    int currentGrowthParamIndex = 0;

    public Image pausePlayImage;

    public Sprite playSprite;
    public Sprite pauseSprite;

    void Awake() {
        Camera.main.orthographicSize = boardSize / 2;
        cells = new Cell[boardSize, boardSize];
        cellBoard = GetComponent<CellBoard>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void ToggleRunning() {
        running = !running;
        if (running) {
            pausePlayImage.sprite = pauseSprite;
        } else {
            pausePlayImage.sprite = playSprite;
        }
    }

    void FixedUpdate() {
        if (running) {
            curTime += 0.01f;
            if (curTime >= frameTime) {
                RunGame();
                curTime = 0;
            }
        }
    }

    public void RunGame() {
        for (int x = 0; x < boardSize; x++) {
            for (int y = 0; y < boardSize; y++) {
                if (cells[x, y].state) {
                    for (int i = 0; i < 9; i++) {
                        if (cellDeathParam[i]) {
                            if (neighborCount(x, y) == i) {
                                cellChanges.Add(new Vector2Int(x, y));
                                cellColors.Add(cells[x, y].color);
                            }
                        }
                    }
                } else {
                    for (int i = 0; i < 9; i++) {
                        if (cellGrowthParam[i]) {
                            if (neighborCount(x, y) == i) {
                                cellChanges.Add(new Vector2Int(x, y));
                                if (i != 0) {
                                    cellColors.Add(colorByNeighbors(x, y));
                                } else {
                                    cellColors.Add(playerInput.color);
                                }
                            }
                        }
                    }
                }

            }
        }

        for (int i = 0; i < cellChanges.Count; i++) {
            SetCell(cellChanges[i].x, cellChanges[i].y, !cells[cellChanges[i].x, cellChanges[i].y].state, cellColors[i]);
        }
        cellBoard.UpdateBoard(cellChanges);
        cellChanges.Clear();
        cellColors.Clear();
    }

    public void SetCell(int x, int y, bool state, Color color, bool updateBoard = false) {
        cells[x, y].state = state;
        cells[x, y].color = color;
        if (updateBoard) {
            List<Vector2Int> cell = new List<Vector2Int>();
            cell.Add(new Vector2Int(x, y));
            cellBoard.UpdateBoard(cell, true);
        }
    }

    byte neighborCount(int x, int y) {
        byte cellCount = 0;
        for (int row = -1; row < 2; row++) {
            for (int column = -1; column < 2; column++) {
                if (row != 0 || column != 0) {
                    if (x + row >= 0 && x + row < boardSize && y + column >= 0 && y + column < boardSize) {
                        if (cells[x, y].state) {
                            if (cells[x + row, y + column].state) {
                                if (cells[x, y].color == cells[x + row, y + column].color) {
                                    cellCount++;
                                }
                            }
                        } else {
                            if (cells[x + row, y + column].state) {
                                cellCount++;
                            }
                        }
                    }
                }
            }
        }
        return cellCount;
    }

    Color colorByNeighbors(int x, int y) {

        List<Color> colors = new List<Color>();
        List<int> amounts = new List<int>();

        for (int row = -1; row < 2; row++) {
            for (int column = -1; column < 2; column++) {
                if (row != 0 || column != 0) {
                    if (x + row >= 0 && x + row < boardSize && y + column >= 0 && y + column < boardSize) {
                        if (cells[x + row, y + column].state) {
                            if (!colors.Contains(cells[x + row, y + column].color)) {
                                colors.Add(cells[x + row, y + column].color);
                                amounts.Add(1);
                            } else {
                                amounts[colors.IndexOf(cells[x + row, y + column].color)]++;
                            }
                        }
                    }
                }
            }
        }

        Color highestFreqColor = colors[0];
        int highestAmount = amounts[0];

        for (int i = 1; i < amounts.Count; i++) {
            if (amounts[i] > highestAmount) {
                highestFreqColor = colors[i];
                highestAmount = amounts[i];
            }
        }
        return highestFreqColor;
    }

    public void SetDeathParamIndex(int index) {
        currentDeathParamIndex = index;
    }

    public void SetDeathParam(bool state) {
        cellDeathParam[currentDeathParamIndex] = state;
    }

    public void SetGrowthParamIndex(int index) {
        currentGrowthParamIndex = index;
    }

    public void SetGrowthParam(bool state) {
        cellGrowthParam[currentGrowthParamIndex] = state;
    }
}

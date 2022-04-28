using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this.gameObject)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private int remainChoosing;

    private int idFirstChoosing;
    private int firstChoosingRowIndex;
    private int firstChoosingColIndex;

    private int idLastChoosing;
    private int lastChoosingRowIndex;
    private int lastChoosingColIndex;

    [SerializeField] ContentController contentController;

    [SerializeField] int[] list = {1,1,2,2,1,2};
    [SerializeField] int[] listControl = {1,1,2,2,1,2};
    [SerializeField] int[] listId = {1,2,3,4,5,6,7,8};
    [SerializeField] int row = 6;
    [SerializeField] int col = 6;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        ListMaker();
        contentController.SpawItems(list,row);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        remainChoosing = 2;
        idFirstChoosing = -1;
        idLastChoosing = -1;

    }

    public void UserChooseItem(int idItem, int row, int col)
    {
        if(remainChoosing == 2)
        {
            idFirstChoosing = idItem;
            firstChoosingRowIndex = row;
            firstChoosingColIndex = col;
        }
        else if(remainChoosing == 1)
        {
            idLastChoosing = idItem;
            lastChoosingRowIndex = row;
            lastChoosingColIndex = col;

            if(idFirstChoosing == idLastChoosing)
            {
                if(Checking(firstChoosingRowIndex, firstChoosingColIndex, lastChoosingRowIndex, lastChoosingColIndex))
                {
                    contentController.HideItem(firstChoosingRowIndex, firstChoosingColIndex);
                    listControl[firstChoosingRowIndex * this.col + firstChoosingColIndex] = 0;
                    contentController.HideItem(lastChoosingRowIndex, lastChoosingColIndex);
                    listControl[lastChoosingRowIndex * this.col + lastChoosingColIndex] = 0;
                }
            }
        }

        remainChoosing--;
        if (remainChoosing < 1)
        {
            contentController.UnTicked(firstChoosingRowIndex, firstChoosingColIndex);
            contentController.UnTicked(lastChoosingRowIndex, lastChoosingColIndex);
            StartNewTurn();
        }
    }

    public void ListMaker()
    {
        int[] temp = new int[row * col];

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j< col; j++)
            {
                int index = i * col + j;
                if (temp[index] != 0) continue;
                int id = listId[UnityEngine.Random.Range(0, listId.Length - 1)];
                temp[index] = id;
                temp[GetRandomIndex(temp)] = id;
            }
        }

        list = temp;
        listControl = temp;
    }

    public int GetRandomIndex (int[] array)
    {
        List<int> remainingSlot = new List<int>();
        for(int i =0; i< array.Length;i++)
        {
            if(array[i]==0)
            {
                remainingSlot.Add(i);
            }
        }
        if (remainingSlot.Count == 0) return -1;
        return remainingSlot[Random.Range(0, remainingSlot.Count-1)];
    }

    public bool Checking(int row1, int col1, int row2, int col2)
    {
        if(CheckBorderLine(row1, col1, row2, col2))
        {
            return true;
        }
        int indexFirst = (row1 * this.col + col1);
        int indexLast = (row2 * this.col + col2);
        List<int> list = new List<int>();
        list = CheckVerticalBetweenTwoPoints(row1, row2, indexFirst, indexLast);

        if (list.Count == 0) return false;
        for(int it = 0; it < list.Count; it++)
        {
            if(CheckHorizontalBetweenTwoPoints(row1, list[it], col1, indexFirst, indexLast) && CheckHorizontalBetweenTwoPoints(row2, list[it], col2, indexFirst, indexLast))
            {
                return true;
            }
        }

        return false;
    }

    public List<int> CheckVerticalBetweenTwoPoints(int rowStart, int rowEnd, int exceptFirst, int exceptLast)
    {
        List<int> listVertical = new List<int> ();  
        for(int j = 0; j < this.col; j++)
        {
            bool check = true;
            if (rowEnd > rowStart)
            {
                for (int i = rowStart; i <= rowEnd; i++)
                {
                    int index = i * this.col + j;
                    if (exceptFirst == index || exceptLast == index)
                    {
                        if (i == rowEnd) break;
                        continue;
                    }

                    if (listControl[index] != 0)
                    {
                        check = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = rowEnd; i <= rowStart; i++)
                {
                    int index = i * this.col + j;
                    if (exceptFirst == index || exceptLast == index)
                    {
                        if (i == rowEnd) break;
                        continue;
                    }

                    if (listControl[index] != 0)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if(check)
            {
                listVertical.Add(j);
            }
        }

        return listVertical;
    }

    public bool CheckHorizontalBetweenTwoPoints(int selectRow , int colStart, int colEnd, int exceptFirst, int exceptLast)
    {
        if (colStart == colEnd) return true;
        if (colEnd > colStart)
        {
            for (int j = colStart; j <= colEnd; j++)
            {
                int index = selectRow * this.col + j;
                if (exceptFirst == index || exceptLast == index) continue;
                if (listControl[index] != 0)
                {
                    return false;
                }
            }
        }
        else
        {
            for (int j = colEnd; j <= colStart; j++)
            {
                int index = selectRow * this.col + j;
                if (exceptFirst == index || exceptLast == index) continue;
                if (listControl[index] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool CheckHorizontalBetweenTwoPoints(int row, int col1, int col2)
    {
        int slot = 0;
        int dir = col2 - col1;
        for (int i = col1;; i += dir)
        {
            if (listControl[row * col + i] != 0)
            {
                slot++;
            }
            if (i == col2) break;
        }

        return (slot<=2)?true:false;
    }

    public bool CheckBorderLine(int row1, int col1, int row2, int col2)
    {
        if(row1 == row2 && (row1 == 0 || row1 == row-1))
        {
            return true;
        }
        else if (col1 == col2 && (col1 == 0 || col1 == col - 1))
        {
            return true;
        }
        return false;
    }

    public bool CheckRemainingCouple()
    {
        List<int> remainingSlot = new List<int>();
        for (int i = 0; i < listControl.Length; i++)
        {
            if (listControl[i] != 0)
            {
                remainingSlot.Add(i);
            }
        }

        for(int i = 0; i < remainingSlot.Count - 1; i++)
        {
            for(int j = i+1; j < remainingSlot.Count; j++)
            {
                if (Checking(remainingSlot[i] % col, remainingSlot[i] % row, remainingSlot[j] % col, remainingSlot[j] % row))
                    return true;
            }
        }

        return false;
    }

    public void Shuffe()
    {
        List<int> remainingSlot = new List<int>();
        for (int i = 0; i < listControl.Length; i++)
        {
            if (listControl[i] != 0)
            {
                remainingSlot.Add(i);
            }
        }

        for(int i = 0; i < remainingSlot.Count; i++)
        {
            int switchItem = Random.Range(0, remainingSlot.Count - 1);
            int currentItemRow = remainingSlot[i] / col;
            int currentItemCol = (remainingSlot[i] - currentItemRow * col) % col;
            int nextItemRow = remainingSlot[switchItem] / col;
            int nextItemCol = (remainingSlot[switchItem] - nextItemRow * col) % col;

            int idTemp = listControl[currentItemRow * col + currentItemCol];
            listControl[currentItemRow * col + currentItemCol] = listControl[nextItemRow * col + nextItemCol];
            listControl[nextItemRow * col + nextItemCol] = idTemp;

            contentController.ChangeSibling(currentItemRow, currentItemCol, nextItemRow, nextItemCol+1);
            contentController.ChangeSibling(nextItemRow, nextItemCol, currentItemRow, currentItemCol);

            contentController.UpdatePosItems();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] Button button;
    [SerializeField] GameObject BG;

    [SerializeField] bool isTicked;
    public bool isKilled { set; get; }

    private int row;
    private int col;

    // Start is called before the first frame update
    void Start()
    {
        isTicked = false;
        button.onClick.AddListener(() => ChoosingItem());
    }

    public void ChoosingItem()
    {
        isTicked = !isTicked;

        if (isTicked)
        {
            button.GetComponent<Image>().color = new Color(0, 0, 0, 0.4f);
            GameController.Instance.UserChooseItem(id, row, col);
        }
        else
        {
            button.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdatePos(int rowValue, int colValue)
    {
        row = rowValue;
        col = colValue;
    }

    public void Hide(bool isHide)
    {
        isKilled = isHide;
        BG.SetActive(isHide);
    }

    public void ChangeSibling(int index)
    {
        transform.SetSiblingIndex(index);
    }
}

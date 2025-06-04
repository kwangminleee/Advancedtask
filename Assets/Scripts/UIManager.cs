using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text cashText;
    public TMP_Text balanceText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        cashText.text = AddComma(GameManager.Instance.userData.cash);
        balanceText.text = AddComma(GameManager.Instance.userData.balance);
    }

    private string AddComma(ulong number)
    {
        return string.Format("{0:#,###}", number);
    }
}


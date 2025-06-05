using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopupBank : MonoBehaviour
{
    public GameObject atmScreen;
    public GameObject depositScreen;
    public GameObject withdrawScreen;
    public GameObject popupErrorScreen;
    public UIManager uiManager;
    public TMP_InputField depositInputField;
    public TMP_InputField withdrawInputField;

    public void ShowDepositScreen()
    {
        atmScreen.SetActive(false);
        depositScreen.SetActive(true);
        withdrawScreen.SetActive(false);
    }

    public void ShowWithdrawScreen()
    {
        atmScreen.SetActive(false);
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(true);
    }

    public void BackScreen()
    {
        atmScreen.SetActive(true);
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(false);
    }

    public void ErrorScreen()
    {
        popupErrorScreen.SetActive(true);
    }

    public void DepositButton()
    {
        if (ulong.TryParse(depositInputField.text, out ulong depositAmount))
        {
            Deposit(depositAmount);
        }
        else
        {
            Debug.LogWarning("입력한 금액이 숫자가 아닙니다.");
        }
    }

    public void DepositButtonFixed(int amount)
    {
        Deposit((ulong)amount);
    }

    public void Deposit(ulong value)
    {
        if (value > 0)
        {
            if (GameManager.Instance.userData.cash < value)
            {
                ErrorScreen();
            }
            else
            {
                GameManager.Instance.userData.balance += value;
                GameManager.Instance.userData.cash -= value;
                GameManager.Instance.SaveUserData();
            }
            uiManager.Refresh();
        }
    }

    public void WithdrawButton()
    {
        if (ulong.TryParse(withdrawInputField.text, out ulong withdrawAmount))
        {
            Withdraw(withdrawAmount);
        }
        else
        {
            Debug.LogWarning("입력한 금액이 숫자가 아닙니다.");
        }
    }

    public void WithdrawButtonFixed(int amount)
    {
        Withdraw((ulong)amount);
    }

    public void Withdraw(ulong value)
    {
        if (value > 0)
        {
            if (GameManager.Instance.userData.balance < value)
            {
                ErrorScreen();
            }
            else
            {
                GameManager.Instance.userData.balance -= value;
                GameManager.Instance.userData.cash += value;
                GameManager.Instance.SaveUserData();
            }
            uiManager.Refresh();
        }
    }
}

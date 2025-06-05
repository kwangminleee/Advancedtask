using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopupBank : MonoBehaviour
{
    public GameObject atmScreen;
    public GameObject depositScreen;
    public GameObject withdrawScreen;
    public GameObject popupErrorScreen;
    public GameObject remittanceScreen;
    public GameObject remittanceErrorScreen;
    public TMP_Text remittanceErrorText;
    public UIManager uiManager;
    public TMP_InputField depositInputField;
    public TMP_InputField withdrawInputField;
    public TMP_InputField remittanceIdInputField;
    public TMP_InputField remittanceAmountInputField;

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

    public void RemittanceWindow()
    {
        remittanceScreen.SetActive(!remittanceScreen.activeSelf);
        atmScreen.SetActive(!atmScreen.activeSelf);
    }

    public void RemittanceButton()
    {
        remittanceErrorScreen.SetActive(false); // 에러 화면 초기화

        string targetId = remittanceIdInputField.text;
        bool parsed = ulong.TryParse(remittanceAmountInputField.text, out ulong amount);

        if (string.IsNullOrEmpty(targetId))
        {
            remittanceErrorText.text = "입력 정보를 확인해주세요.";
            remittanceErrorScreen.SetActive(true);
            return;
        }

        if (!parsed || amount <= 0)
        {
            remittanceErrorText.text = "입력 정보를 확인해주세요.";
            remittanceErrorScreen.SetActive(true);
            return;
        }

        if (GameManager.Instance.userData.balance < amount)
        {
            remittanceErrorText.text = "잔액이 부족합니다.";
            remittanceErrorScreen.SetActive(true);
            return;
        }

        bool success = Remittance(targetId, amount);

        if (!success)
        {
            remittanceErrorText.text = "대상이 없습니다.";
            remittanceErrorScreen.SetActive(true);
            return;
        }

        uiManager.Refresh();
        remittanceIdInputField.text = "";
        remittanceAmountInputField.text = "";
        remittanceErrorScreen.SetActive(false);
    }

    public bool Remittance(string targetId, ulong amount)
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            var targetData = JsonUtility.FromJson<UserData>(json);

            if (targetData.id == GameManager.Instance.currentUserId)
                continue;

            if (targetData.id == targetId || targetData.name == targetId)
            {
                targetData.balance += amount;
                GameManager.Instance.userData.balance -= amount;

                File.WriteAllText(file, JsonUtility.ToJson(targetData, true));
                GameManager.Instance.SaveUserData();

                return true;
            }
        }

        return false;
    }

    public void ErrorWindowClose()
    {
        remittanceErrorScreen.SetActive(!remittanceErrorScreen.activeSelf);
    }
}

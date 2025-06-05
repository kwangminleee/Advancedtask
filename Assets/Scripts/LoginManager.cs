using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField passwordInput; 
    public TMP_InputField loginIDInput;
    public TMP_InputField loginPWInput;
    public TMP_InputField nameInput;
    public TMP_InputField passwordConfirmInput;
    public TextMeshProUGUI errorText;

    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject mainUI;
    public GameObject errorPanel;

    public UIManager uimanager;

    public void TryLogin()
    {
        string id = loginIDInput.text;
        string pw = loginPWInput.text;

        if (string.IsNullOrEmpty(id))
        {
            errorPanel.SetActive(true);
            return;
        }

        if (string.IsNullOrEmpty(pw))
        {
            errorPanel.SetActive(true);
            return;
        }

        if (!GameManager.Instance.IsUserExist(id))
        {
            errorPanel.SetActive(true);
            return;
        }

        if (GameManager.Instance.LoginUser(id, pw))
        {
            loginPanel.SetActive(false);
            mainUI.SetActive(true);
            uimanager.Refresh();
        }
        else
        {
            errorPanel.SetActive(true);
        }
    }

    public void TrySignup()
    {
        string id = idInput.text;
        string name = nameInput.text;
        string pw = passwordInput.text;
        string pwConfirm = passwordConfirmInput.text;

        if (string.IsNullOrEmpty(id))
        {
            errorText.text = "ID를 입력해주세요.";
            errorPanel.SetActive(true);
            return;
        }
        if (string.IsNullOrEmpty(name))
        {
            errorText.text = "이름을 입력해주세요.";
            errorPanel.SetActive(true);
            return;
        }
        if (string.IsNullOrEmpty(pw))
        {
            errorText.text = "비밀번호를 입력해주세요.";
            errorPanel.SetActive(true);
            return;
        }
        if (string.IsNullOrEmpty(pwConfirm))
        {
            errorText.text = "비밀번호 확인을 입력해주세요.";
            errorPanel.SetActive(true);
            return;
        }
        if (pw != pwConfirm)
        {
            errorText.text = "비밀번호가 일치하지 않습니다.";
            errorPanel.SetActive(true);
            return;
        }
        if (GameManager.Instance.IsUserExist(id))
        {
            errorText.text = "이미 존재하는 ID입니다.";
            errorPanel.SetActive(true);
            return;
        }

        GameManager.Instance.RegisterUser(id, pw, name);
        uimanager.Refresh();
        signupPanel.SetActive(false);
        mainUI.SetActive(true);

    }

    public void SignUpPanel()
    {
        signupPanel.SetActive(!signupPanel.activeSelf);
    }

    public void ErrorClose()
    {
        errorPanel.SetActive(false);
    }
}

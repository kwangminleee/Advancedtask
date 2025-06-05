using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UserData userData;

    public string currentUserId;

    private string GetUserPath(string id) =>
        Path.Combine(Application.persistentDataPath, $"{id}.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public bool IsUserExist(string id)
    {
        return File.Exists(GetUserPath(id));
    }

    public void RegisterUser(string id, string password, string name)
    {
        userData = new UserData(id, password, name, 1000000, 5000000);
        currentUserId = id;
        SaveUserData();
    }

    public bool LoginUser(string id, string password)
    {
        if (IsUserExist(id))
        {
            string path = GetUserPath(id);
            string json = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<UserData>(json);

            if (loadedData.password == password)
            {
                userData = loadedData;
                currentUserId = id;
                return true;
            }
        }
        return false;
    }
    public void SaveUserData()
    {
        if (userData != null && !string.IsNullOrEmpty(currentUserId))
        {
            string json = JsonUtility.ToJson(userData, true);
            File.WriteAllText(GetUserPath(currentUserId), json);
        }

        //PlayerPrefs.SetString("Name", userData.name);
        //PlayerPrefs.SetString("Cash", userData.cash.ToString());
        //PlayerPrefs.SetString("Balance", userData.balance.ToString());
    }

    public void Logout()
    {
        userData = null;
        currentUserId = null;
    }

    //public void LoadUserData()
    //{
    //    if (File.Exists(savePath))
    //    {
    //        string json = File.ReadAllText(savePath);
    //        userData = JsonUtility.FromJson<UserData>(json);
    //    }
    //    else
    //    {
    //        userData = new UserData("이광민", 100000, 50000, "1234", "1234");
    //        SaveUserData();
    //    }

    //    //if (PlayerPrefs.HasKey("Name"))
    //    //{
    //    //    userData = new UserData();
    //    //    userData.name = PlayerPrefs.GetString("Name");
    //    //    userData.cash = ulong.Parse(PlayerPrefs.GetString("Cash"));
    //    //    userData.balance = ulong.Parse(PlayerPrefs.GetString("Balance"));
    //    //}
    //    //else
    //    //{
    //    //    userData = new UserData("이광민", 100000, 50000);
    //    //    SaveUserData();
    //    //}
    //}
}

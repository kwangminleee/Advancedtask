using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string id;
    public string password;
    public string name;
    public ulong cash;
    public ulong balance;
    

    public UserData(string id, string password, string name, ulong cash, ulong balance)
    {
        this.id = id;
        this.password = password;
        this.name = name;
        this.cash = cash;
        this.balance = balance;
    }
}

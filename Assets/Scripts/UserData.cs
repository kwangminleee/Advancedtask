using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string name;
    public ulong cash;
    public ulong balance;

    public UserData(string name, ulong cash, ulong balance)
    {
        this.name = name;
        this.cash = cash;
        this.balance = balance;
    }
}

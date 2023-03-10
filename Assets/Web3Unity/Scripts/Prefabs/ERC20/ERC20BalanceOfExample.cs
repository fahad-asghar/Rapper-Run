using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ERC20BalanceOfExample : MonoBehaviour
{
    [SerializeField] Text balanceText;
    async void Start()
    {
        string chain = "binance";
        string network = "mainnet";
        string contract = "0xbc0b35bfefcc3d91ea782d93eb5b82cb11cad7a0";
        string account = PlayerPrefs.GetString("Account");

        BigInteger temp = 1000000000000000000;
        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        BigInteger currentBalanceThunder = balanceOf / temp;

        balanceText.text = "Thunder  Tokens: " + currentBalanceThunder;
    }
}

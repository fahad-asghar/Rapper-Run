using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "binance";
        string network = "mainnet";
        string contract = "0xbc0b35bfefcc3d91ea782d93eb5b82cb11cad7a0";
        string account = "0x7c07DE73718cd7Fbb2413d6cFD31Bcd4D4336D03";

        int balance = await ERC721.BalanceOf(chain, network, contract, account);
        print(balance);
    }
}

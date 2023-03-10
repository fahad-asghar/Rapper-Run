using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "binance";
        string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
        string account = "0x7c07DE73718cd7Fbb2413d6cFD31Bcd4D4336D03";

        string balance = await EVM.BalanceOf(chain, network, account);
        print(balance);
    }
}

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Numerics;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    [SerializeField] GameObject popUp;

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");
        GetBalance();
    }

    async void GetBalance()
    {
        string chain = "binance";
        string network = "mainnet";
        string contract = "0xbc0b35bfefcc3d91ea782d93eb5b82cb11cad7a0";
        string account = PlayerPrefs.GetString("Account");
        
        BigInteger temp = 1000000000000000000;
        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        BigInteger currentBalanceThunder = balanceOf / temp;

        if(currentBalanceThunder >= 100000)
            SceneManager.LoadScene("Main");
        else
        {
            popUp.SetActive(true);
            popUp.GetComponent<Animator>().Play("PopUpFade", -1, 0);
        }
    }

    public void ExitPopUp()
    {
        popUp.SetActive(false);
    }
}
#endif

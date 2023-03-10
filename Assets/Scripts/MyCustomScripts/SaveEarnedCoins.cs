using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveEarnedCoins : MonoBehaviour
{
    public static SaveEarnedCoins instance;
    private string postURL = "https://playthunderrun.com/Save_Coins.php";

    private void Awake()
    {
        instance = this;
    }

    public void PushData(int coins)
    {
        //StartCoroutine(EnterData("0x0FE9f7fDf7cA835B556fd70823122808927670Ff", coins.ToString()));
        StartCoroutine(EnterData(PlayerPrefs.GetString("Account"), coins.ToString()));
    }

    public IEnumerator EnterData(string wallet, string coins)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("coins", coins));
        wwwForm.Add(new MultipartFormDataSection("wallet_address", wallet));

        UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            Debug.Log(www.downloadHandler.text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveWalletAddress : MonoBehaviour
{
    private string postURL = "https://playthunderrun.com/Save_Wallet.php";

    private void Start()
    {
        //StartCoroutine(EnterData("0x0FE9f7fDf7cA835B556fd70823122808927670Ff"));
        StartCoroutine(EnterData(PlayerPrefs.GetString("Account")));
    }

    private IEnumerator EnterData(string wallet)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("wallet_address", wallet));

        UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
        {  
            print(www.downloadHandler.text);          
        }
    }
}

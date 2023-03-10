using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopOperations : MonoBehaviour
{
    private string getURL = "https://playthunderrun.com/Get_Data.php";
    private string postURL = "https://playthunderrun.com/Buy_Item.php";

    [SerializeField] TrackManager manager;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject charactersList;
    [SerializeField] GameObject themeList;

    int coins;

    [Header("TEXTS")]
    [SerializeField] Text status;
    [SerializeField] Text totalCoins;

    [Header("BUTTONS")]
    [SerializeField] Button chadUseButton;
    [SerializeField] Button dogeBuyButton;
    [SerializeField] Button dogeUseButton;
    [SerializeField] Button elonBuyButton;
    [SerializeField] Button elonUseButton;
    [SerializeField] Button messiahBuyButton;
    [SerializeField] Button messiahUseButton;
    [SerializeField] Button nightBuyButton;
    [SerializeField] Button nightUseButton;
    [SerializeField] Button dayUseButton;

    public List<string> fetchedData = new List<string>();

    Button pressedButton;
    Button buyPressedButton;

    /// <summary>
    /// Opens the shop on clicking the shop button.
    /// Fetches the data (Coins, Items Unlocked) from database
    /// </summary>
    public void OpenShop()
    {
        pressedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        pressedButton.interactable = false;

        StartCoroutine(GetData());
    }

    public void CharactersList()
    {
        themeList.SetActive(false);
        charactersList.SetActive(true);
    }

    public void ThemesList()
    {
        themeList.SetActive(true);
        charactersList.SetActive(false);
    }

    public void CloseShop()
    {
        status.gameObject.SetActive(false);
        themeList.SetActive(false);
        charactersList.SetActive(true);

        fetchedData.Clear();
        shop.SetActive(false);
    }

    private IEnumerator GetData()
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        //wwwForm.Add(new MultipartFormDataSection("wallet_address", "0x0FE9f7fDf7cA835B556fd70823122808927670Ff"));
        wwwForm.Add(new MultipartFormDataSection("wallet_address", PlayerPrefs.GetString("Account")));

        UnityWebRequest www = UnityWebRequest.Post(getURL, wwwForm);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            pressedButton.interactable = true;
        }
        else
        {
            string result = www.downloadHandler.text.ToString();
            string[] array = result.Split('-');

            for (int i = 0; i < array.Length; i++)
                fetchedData.Add(array[i]);

            coins = int.Parse(fetchedData[0]);
            totalCoins.text = fetchedData[0];

            if (fetchedData[1].Equals("1"))
            {
                dogeBuyButton.interactable = false;
                dogeBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                dogeUseButton.gameObject.SetActive(true);               
            }
            if (fetchedData[2].Equals("1"))
            {
                elonBuyButton.interactable = false;
                elonBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                elonUseButton.gameObject.SetActive(true);
            }
            if (fetchedData[3].Equals("1"))
            {
             
            }
            if (fetchedData[4].Equals("1"))
            {
                nightBuyButton.interactable = false;
                nightBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                nightUseButton.gameObject.SetActive(true);
            }

            pressedButton.interactable = true;

            shop.SetActive(true);
        }
    }

    public void BuyItem(string nameprice)
    {    
        string[] array = nameprice.Split('-');
        string name = array[0];
        int price = int.Parse(array[1]);

        if (coins < price)
        {
            status.text = "NOT ENOUGH COINS";
            status.gameObject.SetActive(true);
            status.GetComponent<Animator>().Play("StatusText", -1, 0);
            return;
        }

        StartCoroutine(EnterData(name, price));
    }

    public void UseItem(string name)
    {
        if (name.Equals("chad"))
        { 
            chadUseButton.interactable = false;
            elonUseButton.interactable = true;
            dogeUseButton.interactable = true;
            manager.SetCharacter("chad");

        }
        if (name.Equals("doge"))
        {    
            chadUseButton.interactable = true;
            elonUseButton.interactable = true;
            dogeUseButton.interactable = false;
            manager.SetCharacter("doge");
        }
        if (name.Equals("elon"))
        {
            chadUseButton.interactable = true;
            elonUseButton.interactable = false;
            dogeUseButton.interactable = true;
            manager.SetCharacter("elon");
        }
        if (name.Equals("messiah"))
        {

        }

        if (name.Equals("day"))
        {    
            nightUseButton.interactable = true;
            dayUseButton.interactable = false;
            manager.SetTheme("day");
        }

        if (name.Equals("night"))
        {
            PlayerPrefs.SetInt("night", 1);
            PlayerPrefs.SetInt("day", 0);

            nightUseButton.interactable = false;
            dayUseButton.interactable = true;
            manager.SetTheme("night");
        }
    }


    private IEnumerator EnterData(string name, int price)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        //wwwForm.Add(new MultipartFormDataSection("wallet_address", "0x0FE9f7fDf7cA835B556fd70823122808927670Ff"));
        wwwForm.Add(new MultipartFormDataSection("wallet_address", PlayerPrefs.GetString("Account")));
        wwwForm.Add(new MultipartFormDataSection("name", name));
        wwwForm.Add(new MultipartFormDataSection("price", price.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
        {
            Debug.Log(www.downloadHandler.text);
            coins = coins - price;
            totalCoins.text = coins + "";

            if (name.Equals("doge"))
            {
                dogeBuyButton.interactable = false;
                dogeBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                dogeUseButton.gameObject.SetActive(true);
            }
            else if (name.Equals("elon"))
            {
                elonBuyButton.interactable = false;
                elonBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                elonUseButton.gameObject.SetActive(true);
            }
            else if (name.Equals("messiah"))
            {
                
            }
            else if (name.Equals("night"))
            {
                nightBuyButton.interactable = false;
                nightBuyButton.transform.GetChild(0).GetComponent<Text>().text = "OWNED";
                nightUseButton.gameObject.SetActive(true);
            }
        }
    }
}

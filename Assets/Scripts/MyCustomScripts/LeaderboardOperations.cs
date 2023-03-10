using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderboardOperations : MonoBehaviour
{
    public static LeaderboardOperations instance;
    private string postURL = "https://playthunderrun.com/826714_Enter_Score.php";
    private string getURL = "https://playthunderrun.com/827019_Get_Score.php";
    public List<string> fetchedData = new List<string>();


    [SerializeField] GameObject leaderboard;
    [SerializeField] GameObject scorePrefab;
    [SerializeField] Transform layout;

    Button pressedButton;

    private void Awake()
    {
        instance = this;
    }

    public void ShowLeaderboard()
    {
        pressedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        pressedButton.interactable = false;
        StartCoroutine(GetData());
    }

    public void HideLeaderboard()
    {
        fetchedData.Clear();
        for (int i = 0; i < layout.childCount; i++)
            Destroy(layout.GetChild(i).gameObject);

        leaderboard.SetActive(false);
    }

    private IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(getURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            //Show error popUP;
            Debug.Log(www.error);
            pressedButton.interactable = true;    
        }
        else
        {
            string result = www.downloadHandler.text.ToString();
            string[] array = result.Split('-');

            for (int i = 0; i < array.Length; i++)
                fetchedData.Add(array[i]);

            pressedButton.interactable = true;
            PrintDataToLeaderBoard();
        }
    }

    private void PrintDataToLeaderBoard()
    {
        int noOfPrefabs = fetchedData.Count / 2;

        for (int i = 0; i < noOfPrefabs; i++)
            Instantiate(scorePrefab, Vector3.zero, Quaternion.identity, layout);

        int temp = 1;
        int temp1 = 0;
        for (int i = 0; i < layout.childCount; i++)
        {
            layout.GetChild(i).GetChild(0).GetComponent<Text>().text = temp.ToString() + ".";
            temp++;
            layout.GetChild(i).GetChild(1).GetComponent<Text>().text = fetchedData[temp1];
            temp1++;
            layout.GetChild(i).GetChild(2).GetComponent<Text>().text = fetchedData[temp1];
            temp1++;

            if (layout.GetChild(i).GetChild(1).GetComponent<Text>().text == PlayerPrefs.GetString("Account"))
            {
                layout.GetChild(i).GetComponent<Image>().color = new Color32(47, 147, 8, 255);
                layout.GetChild(i).GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                layout.GetChild(i).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                layout.GetChild(i).GetChild(2).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
        }


        leaderboard.SetActive(true);
    }

    public void PushData(int score)
    {
        //string wallet = "0x0FE9f7fDf7cA835B556fd70823122808927670Ff";
        string wallet = PlayerPrefs.GetString("Account");
        int trackScore = score;
        StartCoroutine(EnterData(wallet, trackScore));
    }

    private IEnumerator EnterData(string wallet, int trackScore)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("score", trackScore.ToString()));
        wwwForm.Add(new MultipartFormDataSection("wallet_address", wallet));

        UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);

        else
            Debug.Log(www.downloadHandler.text);
    }
}

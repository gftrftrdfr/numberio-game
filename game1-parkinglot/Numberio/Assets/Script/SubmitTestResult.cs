using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmitTestResult : MonoBehaviour
{
    public string resourceURL;
    public string credential;
    public GameObject gameInfomationPanel;
    public GameObject answerInput;
    public string answer;

    public void SubmitData()
    {
        StartCoroutine(PostResult());
    }
    public IEnumerator PostResult()
    {
        GameInformation gameInfo = GameInformation.Instance;
        WWWForm form = new();
        form.AddField("answer", string.Join("", gameInfo.answers));
        form.AddField("testId", gameInfo.testId);
        form.AddField("userId", gameInfo.userId);


        using (UnityWebRequest request = UnityWebRequest.Post(resourceURL, form))

        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
               int score = JsonUtility.FromJson<int>(request.downloadHandler.text);
                Debug.Log(score);
                
            }
        }
    }
}

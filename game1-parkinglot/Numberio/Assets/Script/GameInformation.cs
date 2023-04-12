using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour {
    [Serializable]
    private class FetchDataModel
    {
        public string testId;
        public string userId;
        public string gameName;
        public string score;
        public List<Question> questions;
    }

    public static GameInformation Instance;

    public string testId;
    public string userId;
    public string gameName;
    public string score;
    public int curQuestion = 0;

    //public string token;
    public List<Question> questions;
    public List<string> answers;

    private void Awake()
    {
       Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void setData(string jsonData)
    {
        Debug.Log(jsonData);
        FetchDataModel newData = new FetchDataModel();
        newData = JsonUtility.FromJson<FetchDataModel>(jsonData);
        this.testId = newData.testId;
        this.userId = newData.userId;
        this.gameName = newData.gameName;
        this.score = newData.score;
        this.questions = newData.questions;

    }
    public Question getQuestion(int idx)
    {
        return questions[idx];
    }

    public void nextQuestion()
    {
        curQuestion+= 1;
    }

    public int getTotalQuestion()
    {
        return questions.Count;
    }

    public Question getCurQuestion()
    {
        return questions[curQuestion];
    }

    public void saveAnswers(int idx, string answer)
    {
        if (this.answers == null)
        {
            answers = new List<string>();
        }

        if (idx > answers.Count - 1)
        {
            answers.Add(answer);
        }
        else
        {
            answers[idx] = answer;
        }

    }
}



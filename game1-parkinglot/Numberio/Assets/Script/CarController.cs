using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public Transform[] WaypointA;
    public Transform[] WaypointB;
    public Transform[] WaypointC;
    public Transform[] WaypointD;
    public Transform[] Waypoint;
    public LoadSceneQuestion loadSceneQuestion = new LoadSceneQuestion();
    public GameObject scenePanel;
    public string type;
    public string resourceURL;
    Transform Target;
    Vector3 pos;
    Quaternion rot;
    bool Check;
    int index = 0;
    public float speed=10f;


    // Start is called before the first frame update
    void Start()
    {
        loadSceneQuestion.scenePanel = scenePanel;
        loadSceneQuestion.setData();
        Check = false;
        index = 0;
        pos = transform.position;
        rot = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if(Check)
        {
            Vector3 dir = Target.position - transform.position;
            transform.right = Target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            if(Vector3.Distance(transform.position,Target.position)<0.2f)
            {
                GetNext();
            }
        }
    }
    void GetNext()
    {
        if(index>=Waypoint.Length-1)
        {
            Check = false;
            StartCoroutine(ResetLevel());
            index = 0;
            return;
        }
        index++;
        Target = Waypoint[index];
    }

    IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(3f);

        transform.position = pos;
        transform.rotation = rot;

        if (type != "dummy")
        {
            if (GameInformation.Instance.curQuestion == GameInformation.Instance.questions.Count - 1)
            {
                SubmitData();
            }
            else
            {
                GameInformation.Instance.nextQuestion();
                loadSceneQuestion.setData();
            }
        }

    }

    public void updateAnswer(string answer)
    {
        if (type != "dummy")
        {
            GameInformation.Instance.saveAnswers(GameInformation.Instance.curQuestion, answer);
        }

    }

    public void ButtonA()
    {
        Waypoint = WaypointA;
        Target = Waypoint[0];
        Check = true;
        updateAnswer("A");
    }
    public void ButtonB()
    {
        Waypoint = WaypointB;
        Target = Waypoint[0];
        Check = true;
        updateAnswer("B");

    }
    public void ButtonC()
    {
        Waypoint = WaypointC;
        Target = Waypoint[0];
        Check = true;
        updateAnswer("C");
    }
    public void ButtonD()
    {
        Waypoint = WaypointD;
        Target = Waypoint[0];
        Check = true;
        updateAnswer("D");
    }

    public void SubmitData()
    {
        StartCoroutine(PostResult());
    }
    public IEnumerator PostResult()
    {
        WWWForm form = new();
        form.AddField("answers", string.Join("", GameInformation.Instance.answers.ToArray()));
        form.AddField("testId", GameInformation.Instance.testId);
        form.AddField("userId", GameInformation.Instance.userId);

        using (UnityWebRequest request = UnityWebRequest.Post(resourceURL, form))

        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Result testResult = JsonUtility.FromJson<Result>(request.downloadHandler.text);
                Debug.Log(testResult.score);
            }
        }
    }
}

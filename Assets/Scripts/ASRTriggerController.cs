using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class CleverbotResponse
{
    public string cs;
    public string output;
}

public class CharadesQuestion
{
    public string solution;
    public string[] hints;
}

public class ASRTriggerController : MonoBehaviour
{
    private bool cleverbotConversationStarted = false;
    private string cleverbotCS;

    private bool askingQuestion = false;
    private int hintIndex = 0;
    private int questionIndex = 0;
    private CharadesQuestion[] charadeQuestions = new CharadesQuestion[] {
        new CharadesQuestion {
            solution = "apple",
            hints = new string [] {
                "It can be a lady or a granny",
                "It's a treat for teacher",
                "It keeps doctors away",
                "It's delicious in pie & cobbler",
                "You pick it in the fall",
                "It can be Red Delicious"
            }
        },
        new CharadesQuestion {
            solution = "zoo",
            hints = new string [] {
                "Dr. Seuss wanted to run one",
                "It might be filled with lions, tigers, and bears, oh my",
                "The national one has a panda cam",
                "It's a popular field trip spot",
                "It might be the only place you get to see an elephant",
                "It's filled with animals"
            }
        }
    };

    bool onCharadesPlane = false;

    void Start()
    {

    }

    void Update()
    {
        GameObject lobbyPlane = GameObject.Find("LobbyPlane");
        GameObject charadesPlane = GameObject.Find("CharadesPlane");
        GameObject cleverbotPlane = GameObject.Find("CleverbotPlane");

        if (Vector3.Distance(Camera.main.transform.position, charadesPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (!onCharadesPlane)
            {
                onCharadesPlane = true;
                StartCoroutine(PlayTextAsAudio("this is speech charades, you have to guess what I am thinking of based on a series of hints, you can say next to hear the next hint or skip to move on to the next question. say question to begin"));
            }
        }
        else
        {
            onCharadesPlane = false;
            askingQuestion = false;
        }

    }

    public void HandleASR(string message)
    {
        Debug.Log("HandleASR: " + message);

        GameObject lobbyPlane = GameObject.Find("LobbyPlane");
        GameObject cleverbotPlane = GameObject.Find("CleverbotPlane");

        if (Vector3.Distance(Camera.main.transform.position, lobbyPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (message.Contains("cube"))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Rigidbody cubeRigidbody = cube.AddComponent<Rigidbody>();
                cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
            }
        }

        if (onCharadesPlane)
        {
            if (message.Contains(charadeQuestions[questionIndex].solution) && askingQuestion)
            {
                StartCoroutine(PlayTextAsAudio("you are correct, say question to hear the next question"));
                askingQuestion = false;
            }

            if (message.Contains("question") && !askingQuestion)
            {
                hintIndex = 0;
                questionIndex += 1;
                if (questionIndex >= charadeQuestions.Length)
                {
                    questionIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
                askingQuestion = true;
            }

            if (message.Contains("next") && askingQuestion)
            {
                hintIndex += 1;
                if (hintIndex >= charadeQuestions[questionIndex].hints.Length)
                {
                    hintIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
            }

            if (message.Contains("skip") && askingQuestion)
            {
                hintIndex = 0;
                questionIndex += 1;
                if (questionIndex >= charadeQuestions.Length)
                {
                    questionIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
            }
        }

        if (Vector3.Distance(Camera.main.transform.position, cleverbotPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (message.Length > 0)
            {
                StartCoroutine(GetCleverbotResponse(message));
            }
        }
    }

    IEnumerator GetCleverbotResponse(string text)
    {
        string url = "https://www.cleverbot.com/getreply?key=" + "INSERT_YOUR_CLEVERBOT_API_KEY";
        if (cleverbotConversationStarted)
        {
            url += "&cs=" + cleverbotCS;
        }
        url += "&input=" + text;

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            CleverbotResponse cleverbotResponse = JsonUtility.FromJson<CleverbotResponse>(uwr.downloadHandler.text);
            StartCoroutine(PlayTextAsAudio(cleverbotResponse.output));
            cleverbotCS = cleverbotResponse.cs;
            cleverbotConversationStarted = true;
        }
    }

    IEnumerator PlayTextAsAudio(string text)
    {
        string url = "https://dgversetts.deepgram.com/text-to-speech/polly?text=" + text;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);

                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = myClip;
                audioSource.Play();
            }
        }
    }
}
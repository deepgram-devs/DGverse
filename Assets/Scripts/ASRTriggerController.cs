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

public class ASRTriggerController : MonoBehaviour
{
    private bool cleverbotConversationStarted = false;
    private string cleverbotCS;

    void Start()
    {

    }

    void Update()
    {

    }

    public void HandleASR(string message)
    {
        Debug.Log("HandleASR: " + message);

        GameObject lobbyPlane = GameObject.Find("LobbyPlane");
        GameObject cleverbotPlane = GameObject.Find("CleverbotPlane");

        Debug.Log("Calculating distance");
        Debug.Log(Vector3.Distance(Camera.main.transform.position, lobbyPlane.transform.position).ToString());

        if (Vector3.Distance(Camera.main.transform.position, lobbyPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (message.Contains("cube"))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Rigidbody cubeRigidbody = cube.AddComponent<Rigidbody>();
                cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
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
        string url = "https://55bd-75-172-104-200.ngrok.io/text-to-speech?text=" + text;
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
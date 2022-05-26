using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ASRTriggerController : MonoBehaviour
{	
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
				StartCoroutine(PlayTextAsAudio(message));
			}
		}
	}
 
	IEnumerator PlayTextAsAudio(string text)
	{
		string url = "https://ad1f-75-172-104-200.ngrok.io/text-to-speech?text=" + text;
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
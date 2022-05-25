using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

using NativeWebSocket;

[System.Serializable]
public class DeepgramResponse
{
	public int[] channel_index;
	public bool is_final;
	public Channel channel;
}

[System.Serializable]
public class Channel
{
	public Alternative[] alternatives;
}

[System.Serializable]
public class Alternative
{
	public string transcript;
}

public class DeepgramInstance : MonoBehaviour
{
	WebSocket websocket;

	public TMP_Text textField;
	public ASRTriggerController asrTriggerController;

	async void Start()
	{
		var headers = new Dictionary<string, string>
		{
			{ "Authorization", "Token 5d40ca35876475dde099b00dd23847478e1039f6" }
        };
		websocket = new WebSocket("wss://api.deepgram.com/v1/listen?encoding=linear16&sample_rate=" + AudioSettings.outputSampleRate.ToString(), headers);

		websocket.OnOpen += () =>
		{
			Debug.Log("Connected to Deepgram!");
			textField.text = "Connected to Deepgram!".ToString();
		};

		websocket.OnError += (e) =>
		{
			Debug.Log("Error: " + e);
			textField.text = "Error".ToString();
		};

		websocket.OnClose += (e) =>
		{
			Debug.Log("Connection closed!");
			textField.text = "Connection closed!".ToString();
		};

		websocket.OnMessage += (bytes) =>
		{
			var message = System.Text.Encoding.UTF8.GetString(bytes);
			Debug.Log("OnMessage: " + message);
			DeepgramResponse deepgramResponse = JsonUtility.FromJson<DeepgramResponse>(message);
			if (deepgramResponse.is_final)
			{
				var transcript = deepgramResponse.channel.alternatives[0].transcript;
				Debug.Log(transcript);
				textField.text = transcript.ToString();
				asrTriggerController.HandleASR(transcript.ToString());
			}
		};

		await websocket.Connect();
	}
	void Update()
	{
    #if !UNITY_WEBGL || UNITY_EDITOR
		websocket.DispatchMessageQueue();
    #endif
	}

	private async void OnApplicationQuit()
	{
		await websocket.Close();
	}

	public async void ProcessAudio(byte[] audio)
	{
		if (websocket.State == WebSocketState.Open)
		{
			await websocket.Send(audio);
		}
	}
}

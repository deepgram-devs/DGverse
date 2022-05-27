using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class SentimentResponse
{
    public Dictionary<string, string> metaData;
    public SentimentResults results;
}

[System.Serializable]
public class SentimentResults
{
    public SentimentChannel[] channels;
}

[System.Serializable]
public class SentimentChannel
{
    public SentimentAlternative[] alternatives;
}

[System.Serializable]
public class SentimentAlternative
{
    public Word[] words;
}

[System.Serializable]
public class Word
{
    public string sentiment;
}

public class BatchDeepgramHandler : MonoBehaviour
{
    public ASRTriggerController asrTriggerController;
    public TMP_Text textField;

    async void Start() { }

    void Update() { }

    public IEnumerator SendRequest(byte[] audioData, byte[] wavHeader)
    {
        textField.text = "Sending Batch Request";

        var audioFile = new byte[audioData.Length + wavHeader.Length];
        System.Buffer.BlockCopy(wavHeader, 0, audioFile, 0, wavHeader.Length);
        System.Buffer.BlockCopy(audioData, 0, audioFile, wavHeader.Length, audioData.Length);

        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(audioFile);
        UnityEngine.WWWForm form = new UnityEngine.WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post("https://api.beta.deepgram.com/v1/listen?sentiment=true&language=en&model=general-enhanced", form))
        {
            www.uploadHandler = uploadHandler;
            www.SetRequestHeader("content-type", "audio/wav");
            www.SetRequestHeader("Authorization", "Token INSERT_YOUR_DEEPGRAM_API_KEY");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                textField.text = "Batch request error occured!";
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Batch response: " + www.downloadHandler.text.ToString());

                SentimentResponse response = JsonUtility.FromJson<SentimentResponse>(www.downloadHandler.text);
                var words = response.results.channels[0].alternatives[0].words;

                int positiveCount = 0;
                int negativeCount = 0;
                foreach (var word in words)
                {
                    var sentiment = word.sentiment;
                    Debug.Log(sentiment);
                    if (sentiment == "positive") positiveCount += 1;
                    if (sentiment == "negative") negativeCount += 1;
                }
                Debug.Log("positive: " + positiveCount);
                Debug.Log("negative: " + negativeCount);
                textField.text = "positive count: " + positiveCount.ToString() + "; negative count: " + negativeCount.ToString();

                asrTriggerController.HandleSentimentASR(words);
            }
        }
    }

    public byte[] GenerateWavHeader(int channels, int frequency)
    {
        byte[] header = new byte[44];
        // The following values are based on http://soundfile.sapp.org/doc/WaveFormat/
        var bitsPerSample = (ushort)16;
        var chunkID = "RIFF";
        var format = "WAVE";
        var subChunk1ID = "fmt ";
        var subChunk1Size = (uint)16;
        var audioFormat = (ushort)1;
        var numChannels = (ushort)channels;
        var sampleRate = (uint)frequency;
        var byteRate = (uint)(sampleRate * channels * bitsPerSample / 8); // SampleRate * NumChannels * BitsPerSample/8
        var blockAlign = (ushort)(numChannels * bitsPerSample / 8); // NumChannels * BitsPerSample/8
        var subChunk2ID = "data";
        var subChunk2Size = (uint)(0); // NumSamples * NumChannels * BitsPerSample/8
        var chunkSize = (uint)(36 + subChunk2Size); // 36 + SubChunk2Size
        // Start writing the header
        // chunkID
        header[0] = (byte)chunkID[0];
        header[1] = (byte)chunkID[1];
        header[2] = (byte)chunkID[2];
        header[3] = (byte)chunkID[3];
        // chunkSize
        header[4] = (byte)(chunkSize & 0xFF);
        header[5] = (byte)((chunkSize >> 8) & 0xFF);
        header[6] = (byte)((chunkSize >> 16) & 0xFF);
        header[7] = (byte)((chunkSize >> 24) & 0xFF);
        // format
        header[8] = (byte)format[0];
        header[9] = (byte)format[1];
        header[10] = (byte)format[2];
        header[11] = (byte)format[3];
        // subChunk1ID
        header[12] = (byte)subChunk1ID[0];
        header[13] = (byte)subChunk1ID[1];
        header[14] = (byte)subChunk1ID[2];
        header[15] = (byte)subChunk1ID[3];
        // subChunk1Size
        header[16] = (byte)(subChunk1Size & 0xFF);
        header[17] = (byte)((subChunk1Size >> 8) & 0xFF);
        header[18] = (byte)((subChunk1Size >> 16) & 0xFF);
        header[19] = (byte)((subChunk1Size >> 24) & 0xFF);
        // audioFormat
        header[20] = (byte)(audioFormat & 0xFF);
        header[21] = (byte)((audioFormat >> 8) & 0xFF);
        // numChannels
        header[22] = (byte)(numChannels & 0xFF);
        header[23] = (byte)((numChannels >> 8) & 0xFF);
        // sampleRate
        header[24] = (byte)(sampleRate & 0xFF);
        header[25] = (byte)((sampleRate >> 8) & 0xFF);
        header[26] = (byte)((sampleRate >> 16) & 0xFF);
        header[27] = (byte)((sampleRate >> 24) & 0xFF);
        // byteRate
        header[28] = (byte)(byteRate & 0xFF);
        header[29] = (byte)((byteRate >> 8) & 0xFF);
        header[30] = (byte)((byteRate >> 16) & 0xFF);
        header[31] = (byte)((byteRate >> 24) & 0xFF);
        // blockAlign
        header[32] = (byte)(blockAlign & 0xFF);
        header[33] = (byte)((blockAlign >> 8) & 0xFF);
        // bitsPerSample
        header[34] = (byte)(bitsPerSample & 0xFF);
        header[35] = (byte)((bitsPerSample >> 8) & 0xFF);
        // subChunk2ID
        header[36] = (byte)subChunk2ID[0];
        header[37] = (byte)subChunk2ID[1];
        header[38] = (byte)subChunk2ID[2];
        header[39] = (byte)subChunk2ID[3];
        // subChunk2Size
        header[40] = (byte)(subChunk2Size & 0xFF);
        header[41] = (byte)((subChunk2Size >> 8) & 0xFF);
        header[42] = (byte)((subChunk2Size >> 16) & 0xFF);
        header[43] = (byte)((subChunk2Size >> 24) & 0xFF);
        return header;
    }
}

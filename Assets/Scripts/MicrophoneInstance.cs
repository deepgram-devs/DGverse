using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInstance : MonoBehaviour
{
    public XRNode rightHandSource;

    bool rightPrimaryPressedPrevious = false;
    bool rightPrimaryPressedCurrent = false;

    AudioSource _audioSource;
    int lastPosition, currentPosition;

    public DeepgramInstance _deepgramInstance;

    byte[] samplesForBatch = new byte[0];
    byte[] wavHeader = new byte[44];
    public BatchDeepgramHandler _batchDeepgramInstance;

    void Start()
    {
        wavHeader = _batchDeepgramInstance.GenerateWavHeader(1, AudioSettings.outputSampleRate);
        _audioSource = GetComponent<AudioSource>();
        if (Microphone.devices.Length > 0)
        {
            _audioSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.Log("This will crash!");
        }

        _audioSource.Play();
    }

    void Update()
    {
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightHandSource);
        rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimaryPressedCurrent);

        // CHANGE FOR 3D/VR: use "if (Input.GetKeyUp("space"))" for 3D and "if (rightPrimaryPressedPrevious == true && rightPrimaryPressedCurrent == false)" for VR
        if (Input.GetKeyUp("space"))
        {
            StartCoroutine(_batchDeepgramInstance.SendRequest(samplesForBatch, wavHeader));
            samplesForBatch = new byte[0];
        }
        rightPrimaryPressedPrevious = rightPrimaryPressedCurrent;

        if ((currentPosition = Microphone.GetPosition(null)) > 0)
        {
            if (lastPosition > currentPosition)
                lastPosition = 0;

            if (currentPosition - lastPosition > 0)
            {
                float[] samples = new float[(currentPosition - lastPosition) * _audioSource.clip.channels];
                _audioSource.clip.GetData(samples, lastPosition);

                short[] samplesAsShorts = new short[(currentPosition - lastPosition) * _audioSource.clip.channels];
                for (int i = 0; i < samples.Length; i++)
                {
                    samplesAsShorts[i] = f32_to_i16(samples[i]);
                }

                var samplesAsBytes = new byte[samplesAsShorts.Length * 2];
                System.Buffer.BlockCopy(samplesAsShorts, 0, samplesAsBytes, 0, samplesAsBytes.Length);

                // CHANGE FOR 3D/VR: use "if (Input.GetKey("space"))" for 3D and "if (rightPrimaryPressedCurrent)" for VR
                if (Input.GetKey("space"))
                {
                    AddToBatchSamples(samplesAsBytes);
                }

                _deepgramInstance.ProcessAudio(samplesAsBytes);

                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
                lastPosition = currentPosition;
            }
        }
    }

    void AddToBatchSamples(byte[] newSamples)
    {
        var newSamplesForBatch = new byte[samplesForBatch.Length + newSamples.Length];
        System.Buffer.BlockCopy(samplesForBatch, 0, newSamplesForBatch, 0, samplesForBatch.Length);
        System.Buffer.BlockCopy(newSamples, 0, newSamplesForBatch, samplesForBatch.Length, newSamples.Length);
        samplesForBatch = newSamplesForBatch;
    }

    short f32_to_i16(float sample)
    {
        sample = sample * 32768;
        if (sample > 32767)
        {
            return 32767;
        }
        if (sample < -32768)
        {
            return -32768;
        }
        return (short)sample;
    }
}

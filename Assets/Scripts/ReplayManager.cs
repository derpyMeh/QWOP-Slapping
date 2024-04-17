using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    public Camera recCam;

    public float delayBetweenFrames = 0.05f;
    private List<Texture2D> frames = new List<Texture2D>();
    private bool isRecording = false;
    private bool isPlaying = false;
    public Renderer quadRenderer;

    private void Start()
    {
        //recCam = GetComponent<Camera>();
    }

    void StartRecording()
    {
        frames.Clear();
        isRecording = true;
    }

    void StopRecording()
    {
        isRecording = false;
    }

    void RecordFrame()
    {
        if (isRecording)
        {
            Texture2D frame = CaptureFrame();
            frames.Add(frame);
        }
    }

    Texture2D CaptureFrame()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = recCam.targetTexture;

        Texture2D frame = new Texture2D(recCam.targetTexture.width, recCam.targetTexture.height);
        frame.ReadPixels(new Rect(0,0,recCam.targetTexture.width, recCam.targetTexture.height), 0, 0);
        frame.Apply();

        RenderTexture.active = currentRT;

        return frame;
    }

    void DisplayFrame(Texture2D frame)
    {
        quadRenderer.material.mainTexture = frame;
    }

    void StartPlayback()
    {
        if (!isPlaying && frames.Count > 0)
        {
            StartCoroutine(PlayBack());
        }
    }

    IEnumerator PlayBack()
    {
        isPlaying = true;

        for (int i = 0; i < frames.Count; i++)
        {
            DisplayFrame(frames[i]);
            yield return new WaitForSeconds(delayBetweenFrames);
        }

        isPlaying = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPlayback();
        }
    }

    private void LateUpdate()
    {
        RecordFrame();
    }
}

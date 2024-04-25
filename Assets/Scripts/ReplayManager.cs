using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ReplayManager : MonoBehaviour
{
    public Camera recCam;

    public float delayBetweenFrames = 0.05f;
    private List<Texture2D> frames = new List<Texture2D>();
    private List<Texture2D> lastFramesRecording = new List<Texture2D>();
    private bool isRecording = false;
    private bool isPlaying = false;
    private bool saveLastRecording;
    public Renderer TV1_quadRenderer;
    public Renderer TV2_quadRenderer;
    public Renderer TV3_quadRenderer;
    public Renderer TV4_quadRenderer;
    
    //public Renderer quadRenderer1;
    //public Renderer quadRenderer2;

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
        /*if (saveLastRecording == true)
        {
            lastFramesRecording = frames;
        }*/
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

    void DisplayFrame(Texture2D frame, Renderer quadRenderer)
    {
        quadRenderer.material.mainTexture = frame;
    }

    void StartPlayback(List<Texture2D> framesRecording, Renderer quadRenderer)
    {
        if (framesRecording.Count > 0)
        {
            StartCoroutine(PlayBack(framesRecording, quadRenderer));
            //framesRecording = lastFramesRecording;
        }
    }

    IEnumerator PlayBack(List<Texture2D> framesRecording, Renderer quadRenderer)
    {
        //isPlaying = true;
        for (int i = 0; i < framesRecording.Count; i++)
        {
            DisplayFrame(framesRecording[i], quadRenderer);
            yield return new WaitForSeconds(delayBetweenFrames);
        }

        //isPlaying = false;
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

        if (Input.GetKeyDown(KeyCode.P))        //Play currently stored replay in frames list
        {
            StartPlayback(frames, TV1_quadRenderer);
            /*StartPlayback(frames, TV3_quadRenderer);
            StartPlayback(frames, TV4_quadRenderer);*/
        }
        if (Input.GetKeyDown(KeyCode.O))        //Play last saved recorded replay derived from frames list
        {
            StartPlayback(lastFramesRecording, TV4_quadRenderer);
            /*StartPlayback(frames, TV3_quadRenderer);
            StartPlayback(frames, TV4_quadRenderer);*/
        }
        if (Input.GetKeyDown(KeyCode.M))        //Save the latest recording derived from frames list
        {
            lastFramesRecording.Clear();
            foreach (var frame in frames)
            {
                lastFramesRecording.Add(frame);
            }
        }
        
    }

    private void LateUpdate()
    {
        RecordFrame();
    }
}

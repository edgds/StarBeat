using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    
    public AudioSource audioSource;
    public Lane[] lanes;
    
    public string fileLocation;
    public float noteTime;
    

    public static MidiFile midiFile;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
        ReadFromFile();
        
    }


    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();  //get notes from midi
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; 
        notes.CopyTo(array, 0); //copy to array

        //assign notes to lanes:
        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), 0f);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
       
    {
        
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {

    }
}


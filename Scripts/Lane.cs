using Melanchall.DryWetMidi.Interaction; //midi
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; //note that a certain lane is constrained to (ex: A notes for Lane 1)
    public GameObject notePrefab; //star shaped note
    //list of when notes are to be spawned:
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>(); 

    int spawnIndex = 0; //keeps track of note spawned
   
    
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array) //go through all midi notes in Song Manager array
        {
            if (note.NoteName == noteRestriction) //only get notes from a certain key for a certain lane in array
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap()); //get time of midi note
               timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); //add it to array in metric time
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count) //if there are notes, keep on running
        {
            //check if note in midi song is at note spawn time (keep in account extra time for player to press note (.noteTime)
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime) 
            {
                var note = Instantiate(notePrefab, transform); //instantiate note if it's its time to be spawned
                
                notes.Add(note.GetComponent<Note>()); //add note to list
             
                
                spawnIndex++; //increment spawn index (if bigger than the amount of timestamps, then no more notes are spawned)
            }
        }

        

    }
   
}

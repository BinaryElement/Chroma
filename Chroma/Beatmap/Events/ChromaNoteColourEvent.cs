﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomJSONData.CustomBeatmap;
using CustomJSONData;
using UnityEngine;
using Chroma.Extensions;

namespace Chroma.Beatmap.Events {

    class ChromaNoteColourEvent {

        public static Dictionary<NoteType, Dictionary<float, Color>> CustomNoteColours = new Dictionary<NoteType, Dictionary<float, Color>>();
        public static Dictionary<INoteController, Color> SavedNoteColours = new Dictionary<INoteController, Color>();
        
        // Creates dictionary loaded with all _noteColor custom events and indexs them with the event's time
        public static void Activate(List<CustomEventData> eventData) {
            if (!ChromaBehaviour.LightingRegistered) return;
            foreach (CustomEventData d in eventData) {
                try {
                    dynamic dynData = d.data;
                    int id = (int)Trees.at(dynData, "_id");
                    float r = (float)Trees.at(dynData, "r");
                    float g = (float)Trees.at(dynData, "g");
                    float b = (float)Trees.at(dynData, "b");
                    Color c = new Color(r, g, b);

                    // Dictionary of dictionaries!
                    Dictionary<float, Color> dictionaryID;
                    if (!CustomNoteColours.TryGetValue((NoteType)id, out dictionaryID)) {
                        dictionaryID = new Dictionary<float, Color>();
                        CustomNoteColours.Add((NoteType)id, dictionaryID);
                    }
                    dictionaryID.Add(d.time, c);

                    //ColourManager.TechnicolourBarriersForceDisabled = true;
                }
                catch (Exception e) {
                    ChromaLogger.Log("INVALID CUSTOM EVENT", ChromaLogger.Level.WARNING);
                    ChromaLogger.Log(e);
                }
            }
        }

        public static void SaberColour(NoteController noteController, NoteCutInfo noteCutInfo) {
            if (SavedNoteColours.TryGetValue(noteController, out Color c)) {
                foreach (SaberColourizer saber in SaberColourizer.saberColourizers) {
                    if (saber.warm == (noteController.noteData.noteType == NoteType.NoteA)) {
                        saber.Colourize(c);
                    }
                }
            }
            noteController.noteWasCutEvent -= SaberColour;
        }
    }
}
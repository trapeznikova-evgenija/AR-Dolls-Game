using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Replay {
    private List<Frame> frames;

    public Replay() {
        frames = new List<Frame>();
    }

    public Replay(List<Frame> frames) {
        this.frames = frames;
    }

    public void AddFrame(Frame frame) {
        frames.Add(frame);
    }

    public Frame GetFrame(int frameNumber) {
        return frames[frameNumber];
    }

    public int numberOfAllFrame() {
        return frames.Count;
    }

    public List<string> GetObjectsNamesForReplays() {
        return frames[0].GetObjectNames();
    }
}

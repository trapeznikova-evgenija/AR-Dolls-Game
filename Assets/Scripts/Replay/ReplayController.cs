using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class ReplayController : MonoBehaviour
{
    public enum Mode {
        Play, Record, Sleep
    }

    public Mode mode;
    
    public List<GameObject> objectsForActions;
    
    private Replay _replay = new Replay();
    
    //Добавить выбор файла из файловой системы
    private Dictionary<string, GameObject> currentObjects = new Dictionary<string, GameObject>();

    private int _frameNumber = 0;

    public void StartRecord() {
        mode = Mode.Record;
        _replay = new Replay();
    }

    public void StopRecord(string filePath) {
        mode = Mode.Sleep;
        SaveToFile(filePath);
    }

    public void StartPlay(String filePath) {
        ReadFromFile(filePath);
        Debug.Log(_replay.numberOfAllFrame());
        mode = Mode.Play;
        InitGameObjectsForActions();
    }

    public void StopPlay() {
        mode = Mode.Sleep;
        _frameNumber = 0;
    }

    private void FixedUpdate() {
        if (mode == Mode.Sleep) return;
        if (mode == Mode.Record) {
            _replay.AddFrame(GetFrameFromScene());
        }

        if (mode == Mode.Play) {
            Debug.Log("frame: " + _frameNumber);
            SetFrameToScene();
        }
    }

    private void InitGameObjectsForActions() {
        var objNames = _replay.GetObjectsNamesForReplays();
        objNames.ForEach(objName => {
            var obj = GameObject.Find(objName);
            currentObjects.Add(objName, obj);
        });
    }

    private void SetFrameToScene() {
        var frame = _replay.GetFrame(_frameNumber);
        var namePosRots = frame.NamePosRots;
        foreach (var namePosRot in namePosRots) {
            var obj = currentObjects[namePosRot.Name];
            var objTransform = obj.transform;
            objTransform.position = namePosRot.Pos;
            objTransform.rotation = namePosRot.Rot;
        }

        if (_frameNumber >= _replay.numberOfAllFrame() - 1) {
            mode = Mode.Sleep;
            _frameNumber = 0;
        }

        _frameNumber++;
    }

    private Frame GetFrameFromScene() {
        List<NamePosRot> namesPosRots = new List<NamePosRot>();
        foreach (GameObject objectForAction in objectsForActions) {
            Transform transf = objectForAction.transform;
            NamePosRot namePosRot = new NamePosRot(objectForAction.name, objectForAction.transform);
            namesPosRots.Add(namePosRot);
        }
        return new Frame(namesPosRots);
    }

    private void SaveToFile(string filePath) {
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(filePath);
        FileStream outputStream = File.OpenWrite(filePath);
        formatter.Serialize(outputStream, _replay);
        outputStream.Close();
    }

    private void ReadFromFile(String filePath) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream inputStream = File.OpenRead(filePath);
        _replay = (Replay)formatter.Deserialize(inputStream);
        inputStream.Close();
    }
}

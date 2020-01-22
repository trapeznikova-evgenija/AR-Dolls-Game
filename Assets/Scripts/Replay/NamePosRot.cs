using System;
using UnityEngine;

[Serializable]
public class NamePosRot {
    private string name;
    private float posX;
    private float posY;
    private float posZ;
    
    private float rotW;
    private float rotX;
    private float rotY;
    private float rotZ;

    public NamePosRot(string name, Vector3 position, Quaternion rotation) {
        this.name = name;
        this.posX = position.x;
        this.posY = position.y;
        this.posZ = position.z;
        
        this.rotW = rotation.w;
        this.rotX = rotation.x;
        this.rotY = rotation.y;
        this.rotZ = rotation.z;
    }

    public NamePosRot(string name, Transform transform) {
        this.name = name;
        this.posX = transform.position.x;
        this.posY = transform.position.y;
        this.posZ = transform.position.z;
        
        this.rotW = transform.rotation.w;
        this.rotX = transform.rotation.x;
        this.rotY = transform.rotation.y;
        this.rotZ = transform.rotation.z;
    }

    public string Name {
        get => name;
        set => name = value;
    }

    public Vector3 Pos => new Vector3(posX, posY, posZ);

    public Quaternion Rot {
        get => new Quaternion(rotX, rotY, rotZ, rotW);
    }
}

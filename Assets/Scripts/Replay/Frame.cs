using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Frame
{
    private List<NamePosRot> _namePosRots;

    public Frame(List<NamePosRot> namePosRots) {
        _namePosRots = namePosRots;
    }

    public List<NamePosRot> NamePosRots {
        get => _namePosRots;
        set => _namePosRots = value;
    }

    public List<string> GetObjectNames() {
        return _namePosRots.Select(namePosRot => namePosRot.Name).ToList();
    }
}

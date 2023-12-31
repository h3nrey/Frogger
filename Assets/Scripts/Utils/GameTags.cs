using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GameTags {

    public static readonly string[] tags = {
        "log",
        "car",
        "endPoint",
        "fly"
    };
}

public enum GameTagsMapper {
    LOG = 0,
    CAR = 1,
    ENDPOINT = 2,
    FLY = 3,
}

public enum GameLayers {
    plataform = 6,
    lake = 7,
    car = 8,
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//helps unpack Json arrays, lets us write a json as an array of objects instead of one large object
public class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
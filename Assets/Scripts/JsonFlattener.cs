using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class JsonFlattener
{
    public static Dictionary<string, string> FlattenJsonText(string jsonText)
    {
        var jsonObject = JObject.Parse(jsonText);
        var flatDictionary = new Dictionary<string, string>();
        FlattenJToken(jsonObject, flatDictionary, null);
        return flatDictionary;
    }

    public static Dictionary<string, List<string>> FlattenJsonTextArray(string jsonText)
    {
        var jsonObject = JObject.Parse(jsonText);
        var flatDictionary = new Dictionary<string, List<string>>();
        FlattenJToken(jsonObject, flatDictionary, null);
        return flatDictionary;
    }

    private static void FlattenJToken(JToken token, Dictionary<string, string> flatDictionary, string prefix)
    {
        if (token.Type == JTokenType.Object)
        {
            foreach (var prop in token.Children<JProperty>())
            {
                string propertyName = prefix != null ? $"{prefix}.{prop.Name}" : prop.Name;
                FlattenJToken(prop.Value, flatDictionary, propertyName);
            }
        }
        else if (token.Type == JTokenType.String)
        {
            flatDictionary[prefix] = token.ToString();
        }
    }

    private static void FlattenJToken(JToken token, Dictionary<string, List<string>> flatDictionary, string prefix)
    {
        if (token.Type == JTokenType.Object)
        {
            foreach (var prop in token.Children<JProperty>())
            {
                string propertyName = prefix != null ? $"{prefix}.{prop.Name}" : prop.Name;
                FlattenJToken(prop.Value, flatDictionary, propertyName);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            flatDictionary[prefix] = new List<String>();
            foreach (var value in token.Children())
            {
                if (value.Type == JTokenType.String)
                {
                    flatDictionary[prefix].Add(value.ToString());
                }
            }
        }
    }
}
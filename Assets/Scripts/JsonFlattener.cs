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

    private static void FlattenJToken(JToken token, Dictionary<string, string> flatDictionary, string prefix)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (var prop in token.Children<JProperty>())
                {
                    string propertyName = prefix != null ? $"{prefix}.{prop.Name}" : prop.Name;
                    FlattenJToken(prop.Value, flatDictionary, propertyName);
                }
                break;
            case JTokenType.Array:
                int index = 0;
                foreach (var value in token.Children())
                {
                    FlattenJToken(value, flatDictionary, $"{prefix}.{index}");
                    index++;
                }
                break;
            default:
                flatDictionary[prefix] = token.ToString();
                break;
        }
    }
}
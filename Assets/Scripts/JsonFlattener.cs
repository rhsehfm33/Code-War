using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class JsonFlattener
{
    public static Dictionary<string, string> FlattenJson(string jsonText)
    {
        var jsonObject = JObject.Parse(jsonText);
        var flatDictionary = new Dictionary<string, string>();
        FlattenJToken(jsonObject, flatDictionary, null);
        return flatDictionary;
    }

    public static void FlattenJToken(JToken token, Dictionary<string, string> flatDictionary, string prefix)
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
            default: // For simple types, add the name-value pair to the dictionary
                flatDictionary[prefix] = token.ToString();
                break;
        }
    }
}
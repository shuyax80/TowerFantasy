using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<SaveEntry> entries = new();
}

[Serializable]
public class SaveEntry
{
    public string id;
    public string json;
}

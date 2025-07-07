using System;

[Serializable]
public class DeepSeekMessage
{
    public string role;
    public string content;
}

[Serializable]
public class DeepSeekChoice
{
    public int index;
    public DeepSeekMessage message;
    public string finish_reason;
}

[Serializable]
public class DeepSeekResponse
{
    public string id;
    public string @object;
    public int created;
    public string model;
    public DeepSeekChoice[] choices;
    // usage 字段等可根据需要添加
}
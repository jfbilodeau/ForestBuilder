using System.Text.Json;

namespace ForestBuilder.Core;

public sealed class JsonFileForestPersistence : IForestPersistence
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public Forest Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Forest file was not found at '{filePath}'.", filePath);
        }

        var json = File.ReadAllText(filePath);
        var trees = JsonSerializer.Deserialize<List<Tree>>(json, SerializerOptions) ?? new List<Tree>();

        return new Forest(trees);
    }

    public void Save(string filePath, Forest forest)
    {
        var json = JsonSerializer.Serialize(forest.ListTrees(), SerializerOptions);
        File.WriteAllText(filePath, json);
    }
}
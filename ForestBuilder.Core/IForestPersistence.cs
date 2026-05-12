namespace ForestBuilder.Core;

public interface IForestPersistence
{
    Forest Load(string filePath);

    void Save(string filePath, Forest forest);
}
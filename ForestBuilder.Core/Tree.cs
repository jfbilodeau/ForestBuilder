using System.Text.Json.Serialization;

namespace ForestBuilder.Core;

public sealed class Tree
{
    public Tree(string name, TreeSpecies species, int age)
        : this(Guid.NewGuid(), name, species, age)
    {
    }

    [JsonConstructor]
    public Tree(Guid id, string name, TreeSpecies species, int age)
    {
        Id = id;
        Name = name;
        Species = species;
        Age = age;
    }

    public Guid Id { get; }

    public string Name { get; }

    public TreeSpecies Species { get; }

    public int Age { get; }
}
namespace ForestBuilder.Core;

public sealed class Forest
{
    private readonly List<Tree> _trees;

    public Forest()
        : this(Enumerable.Empty<Tree>())
    {
    }

    public Forest(IEnumerable<Tree> trees)
    {
        _trees = new List<Tree>(trees);
    }

    public Tree CreateTree(string name, TreeSpecies species, int age)
    {
        var tree = new Tree(name, species, age);
        _trees.Add(tree);

        return tree;
    }

    public IReadOnlyList<Tree> ListTrees()
    {
        return _trees.ToArray();
    }

    public void ChopDown(Guid treeId)
    {
        var removedCount = _trees.RemoveAll(tree => tree.Id == treeId);
        if (removedCount == 0)
        {
            throw new KeyNotFoundException($"Tree with id '{treeId}' was not found.");
        }
    }
}
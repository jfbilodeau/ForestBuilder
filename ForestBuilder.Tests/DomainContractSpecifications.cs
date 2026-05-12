using ForestBuilder.Core;

namespace ForestBuilder.Tests;

public class DomainContractSpecifications
{
    [Fact]
    public void TreeExposesIdNameSpeciesAndAge()
    {
        var tree = new Tree("Sentinel", TreeSpecies.Oak, 27);

        Assert.NotEqual(Guid.Empty, tree.Id);
        Assert.Equal("Sentinel", tree.Name);
        Assert.Equal(TreeSpecies.Oak, tree.Species);
        Assert.Equal(27, tree.Age);
    }

    [Fact]
    public void TreeSpeciesEnumDefinesCommonSpecies()
    {
        var species = Enum.GetValues<TreeSpecies>();

        Assert.Contains(TreeSpecies.Oak, species);
        Assert.Contains(TreeSpecies.Maple, species);
        Assert.Contains(TreeSpecies.Pine, species);
        Assert.Contains(TreeSpecies.Birch, species);
        Assert.Contains(TreeSpecies.Cedar, species);
        Assert.Contains(TreeSpecies.Spruce, species);
        Assert.True(species.Length >= 6);
    }

    [Fact]
    public void ForestManagesATreeCollection()
    {
        var firstTree = new Tree(Guid.NewGuid(), "Alpha", TreeSpecies.Pine, 11);
        var secondTree = new Tree(Guid.NewGuid(), "Bravo", TreeSpecies.Birch, 8);
        var forest = new Forest(new[] { firstTree, secondTree });

        var trees = forest.ListTrees();

        Assert.Equal(2, trees.Count);
        Assert.Contains(trees, tree => tree.Id == firstTree.Id);
        Assert.Contains(trees, tree => tree.Id == secondTree.Id);
    }

    [Fact]
    public void ForestCreatesTreesAndChopsThemDownById()
    {
        var forest = new Forest();

        var createdTree = forest.CreateTree("Canopy", TreeSpecies.Cedar, 14);
        var treesAfterCreate = forest.ListTrees();

        Assert.Single(treesAfterCreate);
        Assert.Equal(createdTree.Id, treesAfterCreate[0].Id);
        
        forest.ChopDown(createdTree.Id);

        Assert.Empty(forest.ListTrees());
        Assert.Throws<KeyNotFoundException>(() => forest.ChopDown(Guid.NewGuid()));
    }

    [Fact]
    public void ForestPersistenceLoadsAndSavesAFullForest()
    {
        var persistence = new JsonFileForestPersistence();
        var forest = new Forest();
        var filePath = Path.Combine(Path.GetTempPath(), $"forestbuilder-tests-{Guid.NewGuid():N}.json");

        try
        {
            forest.CreateTree("North", TreeSpecies.Maple, 16);
            forest.CreateTree("South", TreeSpecies.Spruce, 23);

            persistence.Save(filePath, forest);
            var loadedForest = persistence.Load(filePath);
            var loadedTrees = loadedForest.ListTrees();

            Assert.Equal(2, loadedTrees.Count);
            Assert.Contains(loadedTrees, tree => tree.Name == "North" && tree.Species == TreeSpecies.Maple && tree.Age == 16);
            Assert.Contains(loadedTrees, tree => tree.Name == "South" && tree.Species == TreeSpecies.Spruce && tree.Age == 23);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void ForestPersistenceThrowsWhenFileDoesNotExist()
    {
        var persistence = new JsonFileForestPersistence();
        var missingPath = Path.Combine(Path.GetTempPath(), $"forestbuilder-missing-{Guid.NewGuid():N}.json");

        Assert.Throws<FileNotFoundException>(() => persistence.Load(missingPath));
    }
}

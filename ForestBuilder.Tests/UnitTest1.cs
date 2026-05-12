 using System.Reflection;

namespace ForestBuilder.Tests;

public class AssemblySmokeTests
{
    [Fact]
    public void CoreAssemblyCanBeLoaded()
    {
        var assembly = Assembly.Load("ForestBuilder.Core");

        Assert.Equal("ForestBuilder.Core", assembly.GetName().Name);
    }
}

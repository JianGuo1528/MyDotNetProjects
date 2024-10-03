using NUnit.Framework;

namespace MyProject1.UnitTests;

[TestFixture]
public class SimpleTests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Test1()
    {
        Console.WriteLine("Hello World!");
    }
}
﻿using NUnit.Framework;

namespace MyProject1.UnitTests;

[TestFixture]
public class MyTest1
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
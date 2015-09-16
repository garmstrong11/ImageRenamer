namespace ImageRenamer.Tests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using FluentAssertions;
  using ImageRenamer.Concrete;
  using ImageRenamer.Console;
  using NUnit.Framework;

  [TestFixture]
  public class ArtFileTests
  {
    [Test]
    public void ErrorText_NoErrorString_ReturnsEmptyString()
    {
      var info = new FileInfo(@"D:\Switch\Andrews\in\2015_Abita_Hamburgerday_HM.tif");
      var artFile = new ArtFile(info);

      artFile.ErrorText.Should().Be(string.Empty);
    }

    [Test]
    public void ErrorIndex_HasErrorString_FindsErrorText()
    {
      var info = new FileInfo(@"D:\Switch\Andrews\in\Correct 1 2 4 Wrong 3~2.tif");
      var artFile = new ArtFile(info);

      artFile.ErrorText.Should().Be("Not the right color space");
    }

    [Test]
    public void ErrorIndex_OutOfRange_Throws()
    {
      var info = new FileInfo(@"D:\Switch\Andrews\in\Correct 1 2 4 Wrong 3~69.tif");
      var artFile = new ArtFile(info);
      string test;

      Action act = () => test = artFile.ErrorText;

      act.ShouldThrow<KeyNotFoundException>();
    }

    [Test]
    public void MatchName_TildeErrorIsRemovedWhenPresent()
    {
      var info = new FileInfo(@"D:\Switch\Andrews\in\Correct 1 2 4 Wrong 3~2.tif");
      var artFile = new ArtFile(info);

      artFile.MatchName.Should().Be("Correct 1 2 4 Wrong 3.tif");
    }

    [Test]
    public void MatchName_TildeErrorIsIgnoredWhenNotPresent()
    {
      var info = new FileInfo(@"D:\Switch\Andrews\in\Correct 1 2 4 Wrong 3.tif");
      var artFile = new ArtFile(info);

      artFile.MatchName.Should().Be("Correct 1 2 4 Wrong 3.tif");
    }
  }
}
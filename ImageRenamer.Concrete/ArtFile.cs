namespace ImageRenamer.Concrete
{
  using System.Collections.Generic;
  using System.IO;
  using System.Text.RegularExpressions;

  public class ArtFile
  {
    private readonly FileInfo _fileInfo;
    private readonly int _errorIndex;
    private static readonly Regex ErrorRegex = new Regex(@"~(\d+)", RegexOptions.Compiled);


    public ArtFile(FileInfo fileInfo)
    {
      _fileInfo = fileInfo;

      var name = Path.GetFileNameWithoutExtension(_fileInfo.Name);
      int.TryParse(ErrorRegex.Match(name).Groups[1].Value, out _errorIndex);

      MatchName = ErrorRegex.Replace(name, "");
    }

    public string ErrorText
    {
      get
      {
        string err;

        if (!ErrorLookup.TryGetValue(_errorIndex, out err))
        {
          throw new KeyNotFoundException(
            string.Format("Unable to find an error string for the value {0}", _errorIndex));
        }

        return ErrorLookup[_errorIndex];
      }
    }

    public string MatchName { get; private set; }

    public string OriginalName
    {
      get { return _fileInfo.Name; }
    }

    public void CopyToFolder(string newPath)
    {
      _fileInfo.CopyTo(newPath);
    }

    private static readonly Dictionary<int, string> ErrorLookup = new Dictionary<int, string>
    {
      {0, string.Empty},
      {1,  "Not a TIF file"},
      {2,  "Not the right color space"},
      {3,  "Not a TIF file and not the right color space"},
      {4,  "Not the correct resolution"},
      {5,  "Not a TIF file and not the correct resolution"},
      {6,  "Not the right color space and not the correct resolution"},
      {7,  "Not a TIF file, not the right color space and not the correct resolution"},
      {8,  "Not the right size"},
      {9,  "Not a TIF file and not the right size"},
      {10, "Not the right color space and not the right size"},
      {11, "Not a TIF file, not the right color space and not the right size"},
      {12, "Not the correct resolution and not the right size"},
      {13, "Not a TIF file, not the correct resolution and not the right size"},
      {14, "Not the right color space, not the correct resolution and not the right size"},
      {15, "Not a TIF file, not the right color space, not the correct resolution and not the right size"},
      {16, "Unsupported file format"}
    }; 

    protected bool Equals(ArtFile other)
    {
      return _fileInfo.Equals(other._fileInfo);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj.GetType() == GetType() && Equals((ArtFile) obj);
    }

    public override int GetHashCode()
    {
      return _fileInfo.GetHashCode();
    }

    public override string ToString()
    {
      return _fileInfo.Name;
    }
  }
}
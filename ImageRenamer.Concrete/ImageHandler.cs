namespace ImageRenamer.Concrete
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using ImageRenamer.Concrete.Validators;

  public class ImageHandler
  {
    private readonly string _inputDir;
    private readonly string _outputDir;
    private readonly HashSet<RenameRow> _renameRows;

    public ImageHandler(string inputDir, string outputDir)
    {
      _inputDir = inputDir;
      _outputDir = outputDir;
      _renameRows = new HashSet<RenameRow>();
    }

    public void AddRenameRowRange(IEnumerable<RenameRow> rows)
    {
      foreach (var renameRow in rows) {
        _renameRows.Add(renameRow);
      }
    }

    public HashSet<RenameRow> RenameRows
    {
      get { return new HashSet<RenameRow>(_renameRows); }
    } 

    public void AddArtFileRange(IEnumerable<ArtFile> artFiles)
    {
      var extras = new List<RenameRow>();

      foreach (var artFile in artFiles) {
        var found = _renameRows.FirstOrDefault(r => r.MatchName == artFile.MatchName);

        if (found == null) {
          extras.Add(new RenameRow(new RenameRowValidator()) {ArtFile = artFile});
          continue;
        }

        found.ArtFile = artFile;
      }

      foreach (var renameRow in extras) {
        _renameRows.Add(renameRow);
      }
    }

    public void CopyValidFiles()
    {
      var validRows = _renameRows.Where(r => r.IsValid);

      foreach (var row in validRows) {
        var inputFile = new FileInfo(Path.Combine(_inputDir, row.MatchName + ".tif"));
        var outputPath = Path.Combine(_outputDir, row.NewFilename);

        inputFile.CopyTo(outputPath, true);
      }
    }

    public void Cleanup()
    {
      foreach (var file in Directory.EnumerateFiles(_inputDir, "*.*", SearchOption.AllDirectories)) {
        File.Delete(file);
      }
    }
  }
}
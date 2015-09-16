namespace ImageRenamer.Extractor
{
  using System;
  using System.IO;

  public class FlexCelExtractionException : Exception
  {
    public FlexCelExtractionException() {}

    public FlexCelExtractionException(int sheetIndex, int rowIndex, int columnIndex)
    {
      RowIndex = rowIndex;
      ColumnIndex = columnIndex;
      SheetIndex = sheetIndex;
    }

    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int SheetIndex { get; set; }
    public Type Type { get; set; }
    public string FileSpec { get; set; }

    public override string Message
    {
      get
      {
        return string.Format("Unable to extract a value of type {0} from sheet {4} row {1} column {2} of file '{3}'",
          Type.Name, RowIndex, ColumnIndex, Path.GetFileName(FileSpec), SheetIndex);
      }
    }
  }
}
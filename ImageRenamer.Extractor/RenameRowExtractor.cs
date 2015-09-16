namespace ImageRenamer.Extractor
{
  using System;
  using System.Collections.Generic;
  using ImageRenamer.Abstract;
  using ImageRenamer.Concrete;

  public class RenameRowExtractor : ExtractorBase<RenameRow>
  {
    public RenameRowExtractor(IDataSourceAdapter adapter) : base(adapter) {}
    public override IEnumerable<RenameRow> Extract(ColumnMap columnMap, int sheetIndex, int startRowIndex)
    {
      if (columnMap == null) throw new ArgumentNullException("columnMap");
      if (sheetIndex < 1 || sheetIndex > XlAdapter.SheetCount)
      {
        throw new ArgumentOutOfRangeException("sheetIndex");
      }

      XlAdapter.ActiveSheet = sheetIndex;
      var rowCount = XlAdapter.RowCount;

      if (startRowIndex < 1 || startRowIndex > rowCount)
      {
        throw new ArgumentOutOfRangeException("startRowIndex");
      }

      for (var row = startRowIndex; row <= rowCount; row++)
      {
        var renameRow = new RenameRow()
        {
          CustomersFilename = XlAdapter.ExtractString(row, columnMap.CustomerFileNameIndex),
          NewFilename = XlAdapter.ExtractString(row, columnMap.NewFileNameIndex),
        };

        if (renameRow.IsExtractionBlank) continue;

        yield return renameRow;
      }
    }
  }
}
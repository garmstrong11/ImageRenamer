namespace ImageRenamer.Abstract
{
  using System;
  using System.Collections.Generic;

  public interface IDataSourceAdapter
  {
    int ActiveSheet { get; set; }
    string SheetName { get; }
    int ColumnCount { get; }
    int RowCount { get; }
    int SheetCount { get; }
    void SetActiveSheetByName(string sheetName);
    int GetRowCountForSheet(int sheetIndex);
    int GetColumnCountForSheet(int sheetIndex);
    bool IsCellNull(int rowIndex, int columnIndex);
    string ExtractString(int rowIndex, int columnIndex);
    string ExtractString(int sheetIndex, int rowIndex, int columnIndex);
    string ExtractRawString(int rowIndex, int columnIndex);
    string ExtractDateString(int rowIndex, int columnIndex);
    double ExtractDouble(int rowIndex, int columnIndex);
    double ExtractDouble(int sheetIndex, int rowIndex, int columnIndex);
    int ExtractInteger(int rowIndex, int columnIndex);
    int ExtractInteger(int sheetIndex, int rowIndex, int columnIndex);

    /// <summary>
    ///   Returns the number of columns in a row.
    /// </summary>
    /// <param name="rowIndex">The row to count.</param>
    /// <returns></returns>
    int GetRowColumnCount(int rowIndex);

    /// <summary>
    ///   Opens an Excel file for extraction.
    /// </summary>
    /// <param name="sourcePath">The full path of the Excel file to open</param>
    void Open(string sourcePath);

    void Save(string path);
    void SetCellValue(int rowIndex, int columnIndex, object value);
    void MarkForAutofit(int columnIndex);
    void AutofitColumns();
    void FreezeTopRow();
    Tuple<byte, byte, byte> GetCellColor(int rowIndex, int columnIndex);
    bool IsCellForegroundAutoColor(int rowIndex, int columnIndex);
    IEnumerable<string> GetSheetNames();
  }
}
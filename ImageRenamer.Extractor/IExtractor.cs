namespace ImageRenamer.Extractor
{
  using System.Collections.Generic;
  using ImageRenamer.Abstract;

  public interface IExtractor<out T> where T : class
  {
    /// <summary>
    ///   The full path of the Excel file from which to extract data.
    /// </summary>
    string SourcePath { get; }

    /// <summary>
    ///   Checks whether the extractor passed initialization.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    ///   Prepares the extractor for extraction.
    ///   <param name="path">The file path from which to extract data.</param>
    /// </summary>
    void Initialize(string path);

    /// <summary>
    ///   Extracts a ColumnMap from specified sheet and row indices.
    /// </summary>
    /// <param name="settingsService"></param>
    /// <param name="sheetIndex"></param>
    /// <param name="headerRowIndex"></param>
    /// <returns></returns>
    ColumnMap GetColumnMap(ISettingsService settingsService, int sheetIndex, int headerRowIndex);

    IEnumerable<T> Extract(ColumnMap columnMap, int sheetIndex, int startRowIndex);
  }
}
namespace ImageRenamer.Extractor
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using FlexCel.Core;
  using FlexCel.XlsAdapter;
  using ImageRenamer.Abstract;

  public class FlexCelDataSourceAdapter : IDataSourceAdapter
  {
    private readonly XlsFile _xls;
    private string _sourcePath;

    public FlexCelDataSourceAdapter()
    {
      _xls = new XlsFile();
    }

    public int GetRowColumnCount(int rowIndex)
    {
      if (rowIndex < 1) throw new ArgumentOutOfRangeException("rowIndex");
      return _xls.ColCountInRow(rowIndex);
    }

    public void SetActiveSheetByName(string sheetName)
    {
      _xls.ActiveSheetByName = sheetName;
    }

    public void Open(string sourcePath)
    {
      _sourcePath = sourcePath;

      _xls.Open(_sourcePath);
      ActiveSheet = 1;
    }

    public int ActiveSheet
    {
      get
      {
        if (string.IsNullOrWhiteSpace(_sourcePath)) return -1;
        return _xls.ActiveSheet;
      }
      set { _xls.ActiveSheet = value; }
    }

    public int ColumnCount
    {
      get
      {
        if (string.IsNullOrWhiteSpace(_sourcePath)) return -1;
        return _xls.ColCount;
      }
    }

    public int RowCount
    {
      get
      {
        if (string.IsNullOrWhiteSpace(_sourcePath)) return -1;
        return _xls.RowCount;
      }
    }

    public int SheetCount
    {
      get { return _xls.SheetCount; }
    }

    public int GetRowCountForSheet(int sheetIndex)
    {
      return _xls.GetRowCount(sheetIndex);
    }

    public int GetColumnCountForSheet(int sheetIndex)
    {
      return _xls.GetColCount(sheetIndex);
    }

    /// <summary>
    ///   Use this method to extract string dates from date-formatted cells
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public string ExtractRawString(int rowIndex, int columnIndex)
    {
      var richString = _xls.GetStringFromCell(rowIndex, columnIndex);

      return richString.ToString(CultureInfo.CurrentCulture).Trim();
    }

    public string ExtractDateString(int rowIndex, int columnIndex)
    {
      var xf = 0;
      var v = _xls.GetCellValue(rowIndex, columnIndex, ref xf);

      var exception = new FlexCelExtractionException(_xls.ActiveSheet, rowIndex, columnIndex)
      {
        FileSpec = _xls.ActiveFileName
      };

      if (v == null) {
        throw exception;
      }

      var vType = v.GetType();
      var vTypeCode = Type.GetTypeCode(vType);
      exception.Type = vType;

      if (vTypeCode != TypeCode.Double) {
        throw exception;
      }

      DateTime date;

      if (FlxDateTime.TryFromOADate((double) v, false, out date)) {
        return date.ToString("M/d/yyyy");
      }

      exception.Type = date.GetType();
      throw exception;
    }

    public void Save(string path)
    {
      _xls.Save(path);
    }

    public void SetCellValue(int rowIndex, int columnIndex, object value)
    {
      _xls.SetCellValue(rowIndex, columnIndex, value);
    }

    public void MarkForAutofit(int columnIndex)
    {
      _xls.MarkColForAutofit(columnIndex, true, 1.2);
    }

    public void AutofitColumns()
    {
      _xls.AutofitMarkedRowsAndCols(true, false, 1.0);
    }

    public void FreezeTopRow()
    {
      _xls.FreezePanes(new TCellAddress(2, 2));
    }

    public Tuple<byte, byte, byte> GetCellColor(int rowIndex, int columnIndex)
    {
      var color = _xls.GetCellVisibleFormatDef(rowIndex, columnIndex)
        .FillPattern.FgColor.ToColor(_xls);

      return new Tuple<byte, byte, byte>(color.R, color.G, color.B);
    }

    public bool IsCellForegroundAutoColor(int rowIndex, int columnIndex)
    {
      return _xls.GetCellVisibleFormatDef(rowIndex, columnIndex)
        .FillPattern.FgColor.IsAutomatic;
    }

    public IEnumerable<string> GetSheetNames()
    {
      var patchCount = _xls.SheetCount;

      for (var i = 1; i <= patchCount; i++) {
        _xls.ActiveSheet = i;
        yield return _xls.SheetName;
      }
    }

    public string SheetName
    {
      get { return _xls.SheetName; }
    }

    #region ExtractString

    public string ExtractString(int rowIndex, int columnIndex)
    {
      return CheckString(_xls.GetCellValue(rowIndex, columnIndex));
    }

    public string ExtractString(int sheetIndex, int rowIndex, int columnIndex)
    {
      var xf = 0;
      return CheckString(_xls.GetCellValue(sheetIndex, rowIndex, columnIndex, ref xf));
    }

    private static string CheckString(object val)
    {
      if (val == null) return string.Empty;

      var extract = val.ToString().Trim().Replace("\n", " ");

      return extract;
    }

    #endregion

    #region ExtractDouble

    public double ExtractDouble(int rowIndex, int columnIndex)
    {
      var val = CheckDouble(_xls.GetCellValue(rowIndex, columnIndex));

      if (val != null) return val.Value;

      var exception = new FlexCelExtractionException(_xls.ActiveSheet, rowIndex, columnIndex)
      {
        FileSpec = _sourcePath,
        Type = typeof (double)
      };

      throw exception;
    }

    public double ExtractDouble(int sheetIndex, int rowIndex, int columnIndex)
    {
      var xf = 0;
      var val = CheckDouble(_xls.GetCellValue(sheetIndex, rowIndex, columnIndex, ref xf));

      if (val != null) return val.Value;

      var exception = new FlexCelExtractionException(sheetIndex, rowIndex, columnIndex)
      {
        FileSpec = _sourcePath,
        Type = typeof (double)
      };

      throw exception;
    }

    public bool IsCellNull(int rowIndex, int columnIndex)
    {
      var extract = _xls.GetCellValue(rowIndex, columnIndex);

      return extract == null;
    }

    private static double? CheckDouble(object extract)
    {
      if (extract == null) return -1.0;

      if (extract is string) {
        double dbl;
        var str = extract.ToString().Trim();
        if (string.IsNullOrWhiteSpace(str)) return 0.0;
        if (double.TryParse(str, out dbl)) return dbl;

        return null;
      }

      if (!(extract is double)) return 0.0;

      return (double) extract;
    }

    #endregion

    #region ExtractInteger

    public int ExtractInteger(int rowIndex, int columnIndex)
    {
      return CheckInteger(ExtractDouble(rowIndex, columnIndex));
    }

    public int ExtractInteger(int sheetIndex, int rowIndex, int columnIndex)
    {
      return CheckInteger(ExtractDouble(sheetIndex, rowIndex, columnIndex));
    }

    private static int CheckInteger(double val)
    {
      return Convert.ToInt32(Math.Ceiling(val));
    }

    #endregion
  }
}
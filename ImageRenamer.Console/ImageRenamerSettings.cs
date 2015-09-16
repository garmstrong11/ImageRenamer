namespace ImageRenamer.Console
{
  using ImageRenamer.Abstract;
  using ImageRenamer.Console.Properties;

  public class ImageRenamerSettings : ISettingsService
  {
    readonly Settings _settings = Settings.Default;

    public string CustomersFilenameColumnName
    {
      get { return _settings.CustomersFilenameColumnName; }
      set { _settings.CustomersFilenameColumnName = value; }
    }

    public string NewFilenameColumnName
    {
      get { return _settings.NewFilenameColumnName; }
      set { _settings.NewFilenameColumnName = value; }
    }

    public string OutputReportFileName
    {
      get { return _settings.OutputReportFilename; }
      set { _settings.OutputReportFilename = value; }
    }

    public int SheetIndex
    {
      get { return _settings.SheetIndex; }
      set { _settings.SheetIndex = value; }
    }

    public int HeaderRowIndex
    {
      get { return _settings.HeaderRowIndex; }
      set { _settings.HeaderRowIndex = value; }
    }

    public int StartRowIndex
    {
      get { return _settings.HeaderRowIndex + 1; }
    }
  }
}
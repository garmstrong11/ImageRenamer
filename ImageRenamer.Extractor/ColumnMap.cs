namespace ImageRenamer.Extractor
{
  using System.Collections.Generic;
  using System.Linq;
  using ImageRenamer.Abstract;

  public class ColumnMap
  {
    private readonly HashSet<ColumnLocator> _columnLocators;
    private readonly ISettingsService _settings;

    public ColumnMap(ISettingsService settingsService)
    {
      _columnLocators = new HashSet<ColumnLocator>();
      _settings = settingsService;
    }

    public void AddColumnLocator(ColumnLocator item)
    {
      var isAdded = _columnLocators.Add(item);
      if (!isAdded) HasDuplicateColumns = true;
    }

    public bool HasDuplicateColumns { get; private set; }

    public int CustomerFileNameIndex
    {
      get { return FindColumnIndex(_settings.CustomersFilenameColumnName); }
    }

    public int NewFileNameIndex
    {
      get { return FindColumnIndex(_settings.NewFilenameColumnName); }
    }

    private int FindColumnIndex(string columnName)
    {
      var locator = _columnLocators.FirstOrDefault(f => f.Name == columnName);
      return locator == null ? -1 : locator.Index;
    } 
  }
}
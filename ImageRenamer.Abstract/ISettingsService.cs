namespace ImageRenamer.Abstract
{
  public interface ISettingsService
	{
    string CustomersFilenameColumnName { get; set; }
    string NewFilenameColumnName { get; set; }
    string OutputReportFileName { get; set; }

    int SheetIndex { get; set; }
    int HeaderRowIndex { get; set; }
    int StartRowIndex { get; }
	}
}
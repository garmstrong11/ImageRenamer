namespace ImageRenamer.Extractor
{
  using System;

  public class IndexNotFoundException : Exception
  {
    public string ColumnName { get; private set; }

    public IndexNotFoundException(string columnName)
    {
      ColumnName = columnName;
    }

    public override string Message
    {
      get { return string.Format("Unable to find index for a column named {0}", ColumnName); }
    }
  }
}
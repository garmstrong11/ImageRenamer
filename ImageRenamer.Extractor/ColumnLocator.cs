namespace ImageRenamer.Extractor
{
  public class ColumnLocator
  {
    public ColumnLocator(string name, int index)
    {
      Name = name;
      Index = index;
    }

    public string Name { get; private set; }
    public int Index { get; private set; }

    protected bool Equals(ColumnLocator other)
    {
      return string.Equals(Name, other.Name);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj.GetType() == GetType() && Equals((ColumnLocator)obj);
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }
  }
}
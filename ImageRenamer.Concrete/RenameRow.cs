namespace ImageRenamer.Concrete
{
  using ImageRenamer.Concrete.Validators;

  public class RenameRow
  {
    private readonly RenameRowValidator _renameRowValidator;

    public RenameRow()
    {
      _renameRowValidator = new RenameRowValidator();
    }
    
    public RenameRow(string customersFilename, string newFilename)
    {
      CustomersFilename = customersFilename;
      NewFilename = newFilename;
    }
    
    public string CustomersFilename { get; set; }
    public string NewFilename { get; set; }
    public ArtFile ArtFile { get; set; }

    public bool IsExtractionBlank
    {
      get
      {
        return string.IsNullOrWhiteSpace(CustomersFilename)
               && string.IsNullOrWhiteSpace(NewFilename);
      }
    }

    public bool IsValid
    {
      get
      {
        var result = _renameRowValidator.Validate(this);
        return result.IsValid;
      }
    }

    protected bool Equals(RenameRow other)
    {
      return string.Equals(CustomersFilename, other.CustomersFilename) 
        && string.Equals(NewFilename, other.NewFilename) 
        && Equals(ArtFile, other.ArtFile);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj.GetType() == GetType() && Equals((RenameRow) obj);
    }

    public override int GetHashCode()
    {
      unchecked {
        var hashCode = (CustomersFilename != null ? CustomersFilename.GetHashCode() : 0);
        hashCode = (hashCode*397) ^ (NewFilename != null ? NewFilename.GetHashCode() : 0);
        hashCode = (hashCode*397) ^ (ArtFile != null ? ArtFile.GetHashCode() : 0);
        return hashCode;
      }
    }
  }
}
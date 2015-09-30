namespace ImageRenamer.Concrete
{
  using System.IO;
  using FluentValidation;

  public class RenameRow
  {
    private readonly AbstractValidator<RenameRow> _renameRowValidator;

    public RenameRow(AbstractValidator<RenameRow> renameRowValidator)
    {
      _renameRowValidator = renameRowValidator;
    }
    
    //public RenameRow(string customersFilename, string newFilename) : this()
    //{
    //  CustomersFilename = customersFilename;
    //  NewFilename = newFilename;
    //}

    public string CustomersFilename { get; set; }
    public string NewFilename { get; set; }
    public ArtFile ArtFile { get; set; }

    public string MatchName
    {
      get
      {
        return string.IsNullOrWhiteSpace(CustomersFilename) 
          ? string.Empty 
          : Path.GetFileNameWithoutExtension(CustomersFilename);
      }
    }

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

    public string SuccessMessage
    {
      get
      {
        if (!IsValid) return null;
        return NewFilename;
      }
    }
  }
}
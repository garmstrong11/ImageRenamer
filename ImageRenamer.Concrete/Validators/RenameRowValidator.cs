namespace ImageRenamer.Concrete.Validators
{
  using FluentValidation;

  public class RenameRowValidator : AbstractValidator<RenameRow>
  {
    public RenameRowValidator()
    {
      RuleFor(p => p.ArtFile).NotNull()
        .WithMessage("{0}", p => p.CustomersFilename);

      //RuleFor(p => p.ArtFile).SetValidator(new ArtFileValidator());

      RuleFor(p => p.IsExtractionBlank).NotEqual(true)
        .WithMessage("{0}", p => p.ArtFile.OriginalName);

      RuleFor(p => p.ArtFile.ErrorText).Equal(string.Empty)
        .Unless(p => p.ArtFile == null)
        .WithMessage("{0} : {1}", p => p.CustomersFilename, p => p.ArtFile.ErrorText);
    }
  }
}
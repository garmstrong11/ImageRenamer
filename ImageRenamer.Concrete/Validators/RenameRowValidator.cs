namespace ImageRenamer.Concrete.Validators
{
  using FluentValidation;

  public class RenameRowValidator : AbstractValidator<RenameRow>
  {
    public RenameRowValidator()
    {
      RuleFor(p => p.ArtFile).NotNull()
        .WithMessage("Unable to find a file named \"{0}\".", p => p.CustomersFilename);

      //RuleFor(p => p.ArtFile).SetValidator(new ArtFileValidator());

      RuleFor(p => p.IsExtractionBlank).NotEqual(true)
        .WithMessage(
          "Extraneous file found \"{0}\".", p => p.ArtFile.OriginalName);

      RuleFor(p => p.ArtFile.ErrorText).Equal(string.Empty)
        .Unless(p => p.ArtFile == null)
        .WithMessage("Image Format Error on file \"{0}\": {1}", p => p.CustomersFilename, p => p.ArtFile.ErrorText);
    }
  }
}
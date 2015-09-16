namespace ImageRenamer.Concrete.Validators
{
  using FluentValidation;

  public class ArtFileValidator : AbstractValidator<ArtFile>
  {
    public ArtFileValidator()
    {
      RuleFor(p => p.ErrorText).Equal(string.Empty)
        .WithMessage("Image Format Error on file \"{0}\": {1}", p => p.MatchName, p => p.ErrorText);
    }
  }
}
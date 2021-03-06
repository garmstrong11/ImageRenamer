﻿namespace ImageRenamer.Concrete.Validators
{
  using FluentValidation;

  public class RenameRowValidator : AbstractValidator<RenameRow>
  {
    public RenameRowValidator()
    {
      RuleFor(p => p.ArtFile).NotNull()
        .WithMessage("Unable to find a file that matches Customer's File Name \"{0}\".", p => p.CustomersFilename);

      RuleFor(p => p.ArtFile).SetValidator(new ArtFileValidator());

      RuleFor(p => p.IsExtractionBlank).NotEqual(true)
        .WithMessage(
          "Extraneous file found \"{0}\".", p => p.ArtFile.MatchName);
    }
  }
}
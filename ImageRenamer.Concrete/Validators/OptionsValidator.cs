namespace ImageRenamer.Concrete.Validators
{
  using System.IO;
  using FluentValidation;

  public class OptionsValidator : AbstractValidator<Options>
  {
    public OptionsValidator()
    {
      RuleFor(p => p.InputPath).NotEmpty().WithMessage("Input path not specified.");
      RuleFor(p => p.InputPath).Must(ExistAsPath).WithMessage("Input path does not exist");

      RuleFor(p => p.OutputPath).NotEmpty().WithMessage("Output path not specified.");
      RuleFor(p => p.OutputPath).Must(ExistAsPath).WithMessage("Output path does not exist");
    }

    private static bool ExistAsPath(string path)
    {
      return Directory.Exists(path);
    }
  }
}
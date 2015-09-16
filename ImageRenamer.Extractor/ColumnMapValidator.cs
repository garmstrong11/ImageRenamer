namespace ImageRenamer.Extractor
{
  using FluentValidation;
  using ImageRenamer.Abstract;

  public class ColumnMapValidator : AbstractValidator<ColumnMap>
  {
    public ColumnMapValidator(ISettingsService settings)
    {
      var settings1 = settings;

      RuleFor(c => c.CustomerFileNameIndex).GreaterThan(0)
        .WithMessage("\"{0}\" column not found.", settings1.CustomersFilenameColumnName);

      RuleFor(c => c.NewFileNameIndex).GreaterThan(0)
        .WithMessage("\"{0}\" column not found", settings1.NewFilenameColumnName);
    }
  }
}
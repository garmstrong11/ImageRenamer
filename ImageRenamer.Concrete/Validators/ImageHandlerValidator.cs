namespace ImageRenamer.Concrete.Validators
{
  using FluentValidation;

  public class ImageHandlerValidator : AbstractValidator<ImageHandler>
  {
    public ImageHandlerValidator()
    {
      RuleFor(p => p.RenameRows).SetCollectionValidator(new RenameRowValidator());
    }
  }
}
namespace ImageRenamer.Concrete
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using FluentValidation.Results;

  public class ErrorFileBuilder
  {
    private readonly IList<ValidationFailure> _formatFailures;
    private readonly IList<ValidationFailure> _missingFailures;
    private readonly IList<ValidationFailure> _extraFailures;
    private readonly IList<string> _successes; 

    public ErrorFileBuilder(IList<ValidationFailure> failures)
    {
      _formatFailures = failures
        .Where(f => f.PropertyName.EndsWith("ErrorText"))
        .ToList();

      _missingFailures = failures
        .Where(f => f.PropertyName.EndsWith("ArtFile"))
        .ToList();

      _extraFailures = failures
        .Where(f => f.PropertyName.EndsWith("IsExtractionBlank"))
        .ToList();

      _successes = new List<string>();
    }

    public void AddSuccess(string message)
    {
      _successes.Add(message);
    }

    public string GetErrorText()
    {
      var sb = new StringBuilder();

      if (_successes.Any()) {
        sb.AppendLine("Files processed successfully:");
        foreach (var success in _successes) {
          sb.AppendFormat("\t{0}\n", success);
        }
      }

      if (_formatFailures.Any()) {
        if (_successes.Any()) sb.AppendLine();
        sb.AppendLine("File Validation Failures:");
        foreach (var failure in _formatFailures) {
          sb.AppendFormat("\t{0}\n", failure.ErrorMessage);
        }
      }

      if (_missingFailures.Any()) {
        if (_formatFailures.Any()) sb.AppendLine();
        sb.AppendLine("Missing Files:");
        foreach (var failure in _missingFailures) {
          sb.AppendFormat("\t{0}\n", failure.ErrorMessage);
        }
      }

      if (!_extraFailures.Any()) return sb.ToString();

      if (_formatFailures.Any() || _missingFailures.Any()) sb.AppendLine();
      sb.AppendLine("Extra Files Found:");
      foreach (var failure in _extraFailures) {
        sb.AppendFormat("\t{0}\n", failure.ErrorMessage);
      }

      return sb.ToString();
    }
  }
}
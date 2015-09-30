namespace ImageRenamer.Console
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text.RegularExpressions;
  using CommandLine;
  using FluentValidation;
  using ImageRenamer.Abstract;
  using ImageRenamer.Concrete;
  using ImageRenamer.Concrete.Validators;
  using ImageRenamer.Extractor;
  using SimpleInjector;

  class Program
  {
    private static readonly Container Container;

    static Program()
    {
      Container = new Container();
      ConfigureContainer();
    }

    static void Main(string[] args)
    {
      var options = new Options();
      var extractor = Container.GetInstance<IExtractor<RenameRow>>();
      var settings = Container.GetInstance<ISettingsService>();

      var optionsValidator = new OptionsValidator();
      var columnMapValidator = new ColumnMapValidator(settings);
      var handlerValidator = new ImageHandlerValidator();
      var existingFiles = new List<FileInfo>();
      var messages = "";
      var excelFileName = "No Excel File";

      Parser.Default.ParseArguments(args, options);

      try {
        optionsValidator.ValidateAndThrow(options);
        var sourceDir = new DirectoryInfo(options.InputPath);

        existingFiles.AddRange(sourceDir.EnumerateFiles("*.*", SearchOption.AllDirectories));

        // Throws InvalidOperationException if not found
        var excelFile = existingFiles.Single(f => Regex.IsMatch(f.Extension, @"\.xlsx?"));
        excelFileName = Path.GetFileNameWithoutExtension(excelFile.Name);
        existingFiles.Remove(excelFile);

        extractor.Initialize(excelFile.FullName);
        var columnMap = extractor.GetColumnMap(settings, settings.SheetIndex, settings.HeaderRowIndex);
        columnMapValidator.ValidateAndThrow(columnMap);
        var rows = extractor.Extract(columnMap, settings.SheetIndex, settings.StartRowIndex).ToList();

        var handler = new ImageHandler(options.InputPath, options.OutputPath);
        handler.AddRenameRowRange(rows);
        handler.AddArtFileRange(existingFiles.Select(f => new ArtFile(f)));

        var result = handlerValidator.Validate(handler);
        var errorBuilder = new ErrorFileBuilder(result.Errors);
        foreach (var validRow in rows.Where(r => r.IsValid)) {
          errorBuilder.AddSuccess(validRow.SuccessMessage);
        }

        messages += errorBuilder.GetErrorText();

        handler.CopyValidFiles();
#if !DEBUG
        handler.Cleanup();
#endif

      }

      catch (ValidationException exc) {
        messages += exc.Message;
      }

      catch (FlexCelExtractionException exc) {
        messages += exc.Message;
      }

      catch (InvalidOperationException) {
        messages += "Unable to find an Excel file for this group.";
      }

      catch (KeyNotFoundException exc) {
        messages += exc.Message;
      }

      catch (Exception exc) {
        messages += exc.Message;
      }

      finally {
        var outputFilename = Path.Combine(options.OutputPath, excelFileName + settings.OutputReportFileName);
        File.WriteAllText(outputFilename, messages);
      }
    }

    private static void ConfigureContainer()
    {
      Container.RegisterSingle<ISettingsService, ImageRenamerSettings>();
      Container.RegisterSingle<IDataSourceAdapter, FlexCelDataSourceAdapter>();
      Container.Register<IExtractor<RenameRow>, RenameRowExtractor>();
      Container.Register<AbstractValidator<RenameRow>, RenameRowValidator>();
    }
  }
}

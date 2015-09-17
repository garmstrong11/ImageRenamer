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
      var messages = new List<string>();

      Parser.Default.ParseArguments(args, options);

      try {
        optionsValidator.ValidateAndThrow(options);
        var sourceDir = new DirectoryInfo(options.InputPath);

        existingFiles.AddRange(sourceDir.EnumerateFiles("*.*", SearchOption.AllDirectories));

        // Throws InvalidOperationException if not found
        var excelFile = existingFiles.Single(f => Regex.IsMatch(f.Extension, @"\.xlsx?"));
        existingFiles.Remove(excelFile);

        extractor.Initialize(excelFile.FullName);
        var columnMap = extractor.GetColumnMap(settings, settings.SheetIndex, settings.HeaderRowIndex);
        columnMapValidator.ValidateAndThrow(columnMap);
        var rows = extractor.Extract(columnMap, settings.SheetIndex, settings.StartRowIndex);

        var handler = new ImageHandler(options.InputPath, options.OutputPath);
        handler.AddRenameRowRange(rows);
        handler.AddArtFileRange(existingFiles.Select(f => new ArtFile(f)));

        var result = handlerValidator.Validate(handler);
        messages.AddRange(result.Errors.Select(e => e.ErrorMessage).OrderBy(e => e));

        handler.CopyValidFiles();
        handler.Cleanup();
      }

      catch (ValidationException exc) {
        messages.AddRange(exc.Errors.Select(f => f.ErrorMessage));
      }

      catch (FlexCelExtractionException exc) {
        messages.Add(exc.Message);
      }

      catch (InvalidOperationException) {
        messages.Add("Unable to find an Excel file for this group.");
      }

      catch (KeyNotFoundException exc) {
        messages.Add(exc.Message);
      }

      catch (Exception exc) {
        messages.Add(exc.Message);
      }

      finally {
        var outputFilename = Path.Combine(options.OutputPath, settings.OutputReportFileName);
        File.WriteAllLines(outputFilename, messages.OrderBy(p => p));
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

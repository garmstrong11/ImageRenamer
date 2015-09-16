namespace ImageRenamer.Concrete
{
  using CommandLine;

  public class Options
  {
    [Option('i', "input", Required = true, HelpText = "Input folder path")]
    public string InputPath { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output folder path")]
    public string OutputPath { get; set; }

    //[Option('e', "error", Required = true, HelpText = "Error folder path")]
    //public string ErrorPath { get; set; }
  }

}
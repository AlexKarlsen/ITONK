using System;

public static void Run(TimerInfo myTimer, TraceWriter log, Stream inputBlob, out string outputBlob)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
    //CombineAllInDirectory(inputBlob, outputBlob);

    outputBlob = "ghjk";
}

/// <summary>
/// Combines all given files to one file with the given outputname
/// </summary>
/// <param name="inputfiles"></param>
/// <param name="outputName"></param>
public void Combine(string[] inputfiles, string outputName)
{
    using (var fs = File.OpenWrite(outputName))
    {
        foreach (var file in inputfiles)
        {
            var buffer = File.ReadAllBytes(file);
            fs.Write(buffer, 0, buffer.Length);
        }
        fs.Flush();
    }
}

/// <summary>
/// Takes all files in given directory and concatenates them into one file
/// </summary>
/// <param name="directoryPath"></param>
/// <param name="outputName"></param>
public void CombineAllInDirectory(string directoryPath, string outputName)
{
    Combine(Directory.GetFiles(directoryPath), outputName);
}
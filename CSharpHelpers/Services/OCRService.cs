using System.Diagnostics;
using CSharpHelpers.Models;
using IronOcr;
using Microsoft.Extensions.Configuration;

namespace CSharpHelpers.Services
{
    public class OCRService : BaseService
    {
        #region Variables and constants
        private readonly OCRServiceProvider _provider;
        private readonly IronTesseract? _ocrIronTesseract;

        #endregion

        #region Constructors
        // Constructor for IronOcr
        public OCRService(OCRServiceProvider provider) : base()
        {
            _provider = provider;

            // IronOCRProvider
            if(provider == OCRServiceProvider.IronOCRProvider) 
            {
                IConfigurationSection? settingSession = config!.GetSection("Settings");
                License.LicenseKey = settingSession.GetValue<string>("TestIronOcrKey");
                _ocrIronTesseract = new() { Language = OcrLanguage.Russian };
            }
        }
        #endregion

        #region Funtionality

        /// <summary>
        /// Scans text information from files.
        /// </summary>
        /// <param name="fileInfos">A FileInfo objects representing a files</param>
        /// <returns>A collection of ScanTextResult objects as result of execution.</returns>
        public async Task<List<ScanTextResult>> ScanTextFromFiles(List<FileInfo> fileInfos, bool isTrasing = false) 
        {
            if(isTrasing)
                Trace.WriteLine("Documents scanning started...");

            List<ScanTextResult> results = [];

            List<Task<ScanTextResult>?> scanTasks = [];
            foreach (var fileInfo in fileInfos)
            {
                scanTasks.Add(ScanTextFromFile(fileInfo));
            }
            var scanTasksArray = scanTasks.ToArray();

            if(scanTasksArray is null || scanTasksArray.Length == 0) return results;
        
            if(isTrasing)
                Trace.WriteLine($"{scanTasksArray.Length} files ready for text scan.");

            for (int i = 0; i < scanTasksArray.Length; i++)
            {
                var currentTask = scanTasksArray[i];
                if (currentTask is not null)
                {
                    try
                    {
                        await currentTask;
                        currentTask.Wait();
                        results.Add(currentTask.Result);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                }
            }
            Task.WaitAll(scanTasksArray!);
            
            var scanTime = results.Aggregate(0.00, (acc, i) => acc + i.ScanTime);

            if(isTrasing)
                Trace.WriteLine("\nDocument scanning completed.\n" + 
                                $"Scanning {results.Count} documents took {scanTime} sec.");
                                
            return results;
        }

        /// <summary>
        /// Async scan text info from a file to ScanTextResult type.
        /// </summary>
        /// <param name="fileInfo">A FileInfo object representing a file.</param>
        /// <returns>ScanTextResult object as a result of scanning.</returns>
        private async Task<ScanTextResult> ScanTextFromFile(FileInfo fileInfo) 
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var text = string.Empty;
            var isConvertToPdfRequired = false;
            
            switch (_provider)
            {
                case OCRServiceProvider:

                    using (var input = new OcrInput()) 
                    {
                        switch (fileInfo.Extension) 
                        {
                            case FileService.FILEEXTENSION_JPG: 
                            case FileService.FILEEXTENSION_JPEG:
                                input.LoadImage(fileInfo.FullName);
                                isConvertToPdfRequired = true;
                                break;
                            case FileService.FILEEXTENSION_PDF:
                                input.LoadPdf(fileInfo.FullName);
                                break;  
                        }

                        var readTask = _ocrIronTesseract.ReadAsync(input);
                        await Task.Run(() => readTask);
                        readTask.Wait();

                        text = readTask.Result.Text;
                    }

                    break;
            }

            stopwatch.Stop();
            double time = stopwatch.ElapsedMilliseconds / 1000;

            await Task.Delay(1);
            return new ScanTextResult(fileInfo.FullName, text, time, isConvertToPdfRequired);
        }
        #endregion
    }
}
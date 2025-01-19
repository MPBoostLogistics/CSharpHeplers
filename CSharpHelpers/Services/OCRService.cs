using System.Diagnostics;
using CSharpHelpers.Models;
using IronOcr;

namespace CSharpHelpers.Services
{
    public class OCRService : BaseService
    {
        #region Variables and constants
        private readonly OCRServiceProvider _provider;
        private readonly IronTesseract _ocrIronTesseract;
        public const string IRON_OCR_KEY = "IRON_OCR_KEY";
        #endregion

        #region Constructors
        // For IronOcr
        public OCRService(OCRServiceProvider provider) : base()
        {
            _provider = provider;

            // IronOCRProvider
            var ocrIronKey = config[IRON_OCR_KEY];
            if(!string.IsNullOrEmpty(ocrIronKey)) 
            {
                License.LicenseKey = ocrIronKey; 
                _ocrIronTesseract = new() { Language = OcrLanguage.Russian };
            } 
            else 
            {
                throw new Exception($"No argument '{IRON_OCR_KEY}' was received to create OCRServiceProvider.");
            }
        }
        #endregion

        #region Funtionality

        /// <summary>
        /// Scans text information from files.
        /// </summary>
        /// <param name="fileInfos">A FileInfo objects representing a files</param>
        /// <returns>A collection of ScanTextResult objects as result of execution.</returns>
        public async Task<List<ScanTextResult>> ScanTextFromFiles(List<FileInfo> fileInfos) 
        {
            Trace.WriteLine("Document scanning started...");

            List<ScanTextResult> results = [];

            List<Task<ScanTextResult>?> scanTasks = [];
            foreach (var fileInfo in fileInfos)
            {
                scanTasks.Add(ScanTextFromFile(fileInfo));
            }
            var scanTasksArray = scanTasks.ToArray();
            if(scanTasksArray is null || scanTasksArray.Length == 0) 
            {
                return results;
            }
            else 
            {
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
            }

            var scanTime = results.Aggregate(0.00, (acc, i) => acc + i.ScanTime);

            Trace.WriteLine("\nDocument scanning completed.\n" + 
                            $"Scanning {results.Count} documents took {scanTime} seconds");
            
            return results;
        }

        /// <summary>
        /// Scanning text information from a file.
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

                        var read = _ocrIronTesseract.Read(input);
                       // await Task.Run(() => readTask);
                        //readTask.Wait();

                        text = read.Text;
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
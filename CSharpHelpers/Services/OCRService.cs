using System.Diagnostics;
using CSharpHelpers.Models;
using IronOcr;

namespace CSharpHelpers.Services
{

    public class OCRService
    {
        #region Variables and constants
        private OCRServiceProvider _provider;

        private readonly IronTesseract _ocrIronTesseract;
        #endregion

        #region Constructors
        // For IronOcr
        public OCRService(OCRServiceProvider provider, string ocrIronKey)
        {
            _provider = provider;

            // IronOCRProvider
            IronOcr.License.LicenseKey = ocrIronKey; 
            _ocrIronTesseract = new() { Language = OcrLanguage.Russian };
        }
        #endregion

        #region Funtionality
        public async Task<List<ScanTextResult>> ScanTextFromFiles(List<FileInfo> fileInfos) 
        {
            Trace.WriteLine("Document scanning started...");

            List<ScanTextResult> results = [];

            foreach (var fileInfo in fileInfos)
            {
                var currentTask = ScanTextFromFile(fileInfo);
                await currentTask;
                currentTask.Wait();
                results.Add(currentTask.Result);
            }

            var scanTime = results.Aggregate(0.00, (acc, i) => acc + i.ScanTime);

            Trace.WriteLine("\nDocument scanning completed.\n" + 
                            $"Scanning {results.Count} documents took {scanTime} seconds");
            
            return results;
        }

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
                            case FileService.FILEEXTENSION_JPG: case FileService.FILEEXTENSION_JPEG:
                                input.LoadImage(fileInfo.FullName);
                                isConvertToPdfRequired = true;
                                break;
                            case FileService.FILEEXTENSION_PDF:
                                input.LoadPdf(fileInfo.FullName);
                                break;  
                        }

                        OcrResult result = _ocrIronTesseract.Read(input);
                        text = result.Text;
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
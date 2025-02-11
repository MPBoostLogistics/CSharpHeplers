using System.Diagnostics;
using CSharpHelpers.Models;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test 
{
    public abstract class Base_Test
    {
        #region Variables and constants
        internal const string TEST_PATH_CORRECT = "TestDirectory";     
        internal readonly string[] _testFilesExtensions = 
            [FileService.FILEEXTENSION_JPG, FileService.FILEEXTENSION_JPEG, FileService.FILEEXTENSION_PDF];
        internal const int TEST_FILES_COUNT = 4;
        internal DirectoryInfo? testDirectoryInfo;
        internal FileInfo[]? testFileInfos;

        // Services
        internal OCRService? ironOcrService;

        #endregion

        #region Properties
        internal DirectoryInfo? ProjectDirectory;
        #endregion

        [OneTimeSetUp]
        virtual public void Setup() 
        {   
            Trace.Listeners.Add(new ConsoleTraceListener());

            FileService.GetProjectDirectory(out ProjectDirectory);
            var testDirectoryPath = string.Concat(ProjectDirectory?.FullName, $"/{TEST_PATH_CORRECT}");

            FileService.GetDirectoryInfo(testDirectoryPath, out testDirectoryInfo);
            FileService.SearchFilesInDirectory(testDirectoryInfo!.FullName, out testFileInfos, _testFilesExtensions);

            ironOcrService = new(OCRServiceProvider.IronOCRProvider);
        }
    
        [OneTimeTearDown]
        virtual public void Finish() 
        {
            Trace.Flush();
        }
    }
}
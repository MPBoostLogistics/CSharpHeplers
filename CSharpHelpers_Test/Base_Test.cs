using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpHelpers.Models;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test 
{
    public abstract class Base_Test
    {
        #region Variables and constants
        internal const string TEST_SOURCE_PATH_CORRECT = "TestSourceDirectory";  
        internal const string TEST_SOURCE_PATH_INCORRECT = "TestSourceDirectory001";
        internal const string TEST_TARGET_PATH_CORRECT = "TestTargetDirectory";    
        internal readonly string[] _testFilesExtensions = 
            [FileService.FILEEXTENSION_JPG, FileService.FILEEXTENSION_JPEG, FileService.FILEEXTENSION_PDF];
        internal const int TEST_FILES_COUNT = 4;
        internal DirectoryInfo? testSourceDirectoryInfo;
        internal DirectoryInfo? testTargetDirectoryInfo;
        internal FileInfo[]? testFileInfos;

        // Services
        internal OCRService? ironOcrService;

        // Regex
        internal static readonly string fileNameRegexPattern = @"(Отгрузка товаров с хранения № УТ-)\d+\s(от)\s+\d+\s+[а-яА-Я]+\s+\d+\s+[г]+[.]";
        internal static readonly Regex fileNameRegex = new(fileNameRegexPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        internal static readonly string fileDateRegexPattern = @"\d+\s+[а-яА-Я]+\s+\d+\s+[г]+[.]";
        internal static readonly Regex fileDateRegex = new(fileDateRegexPattern, RegexOptions.None);

        #endregion

        #region Properties
        internal DirectoryInfo? ProjectDirectory;
        #endregion

        [OneTimeSetUp]
        virtual public void Setup() 
        {   
            Trace.Listeners.Add(new ConsoleTraceListener());

            FileService.GetProjectDirectory(out ProjectDirectory);

            var testSourceDirectoryPath = string.Concat(ProjectDirectory?.FullName, $"/{TEST_SOURCE_PATH_CORRECT}");
            FileService.GetDirectoryInfo(testSourceDirectoryPath, out testSourceDirectoryInfo);
            FileService.SearchFilesInDirectory(testSourceDirectoryInfo!.FullName, out testFileInfos, _testFilesExtensions);

            var testTargetDirectoryPath = string.Concat(ProjectDirectory?.FullName, $"/{TEST_TARGET_PATH_CORRECT}");
            FileService.GetDirectoryInfo(testTargetDirectoryPath, out testTargetDirectoryInfo);

            ironOcrService = new(OCRServiceProvider.IronOCRProvider);
        }
    
        [OneTimeTearDown]
        virtual public void Finish() 
        {
            Trace.Flush();
        }
    }
}
using System.Diagnostics;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test 
{
    public abstract class Base_Test
    {
        #region Variables and constants
        internal const string TEST_PATH_CORRECT = "TestDirectory";     
        internal string? _testDirectoryPath;
        internal readonly string[] _testFilesExtensions = 
            [FileService.FILEEXTENSION_JPG, FileService.FILEEXTENSION_JPEG, FileService.FILEEXTENSION_PDF];
        internal const int TEST_FILES_COUNT = 4;

        #endregion


        #region Properties
        internal DirectoryInfo? ProjectDirectory;
        #endregion

        [OneTimeSetUp]
        virtual public void Setup() 
        {   
            Trace.Listeners.Add(new ConsoleTraceListener());

            FileService.GetProjectDirectory(out ProjectDirectory);
            _testDirectoryPath = string.Concat(ProjectDirectory?.FullName, $"/{TEST_PATH_CORRECT}");
        }
    
        [OneTimeTearDown]
        virtual public void Finish() 
        {
            Trace.Flush();
        }
    }
}
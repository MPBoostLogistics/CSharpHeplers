using System.Diagnostics;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test
{
    public class FileService_Test
    {
        private const string TEST_PATH_CORRECT = "TestDirectory"; 
        private const string TEST_PATH_INCORRECT = "TestDirectory01";
        private DirectoryInfo? ProjectDirectory;
        private DirectoryInfo? _testDirectory;
        private FileInfo[]? testFiles = null;
        private readonly int testFilesCount = 4;
        private readonly string[] testExtensions = 
            [FileService.FILEEXTENSION_JPG, FileService.FILEEXTENSION_JPEG, FileService.FILEEXTENSION_PDF];
         private readonly string[] incorrectFileNames = 
            ["Some filename 01", "Some filename 02", "Some filename 03"];

        [OneTimeSetUp]
        public void Setup()
        {
            FileService.GetProjectDirectory(out ProjectDirectory);
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [TestCase(TEST_PATH_CORRECT, true)]
        [TestCase(TEST_PATH_INCORRECT, false)]
        [Order(1)]
        public void GetDirectoryByPathWithoutCreating_Test(string path, bool flag)
        {
            // arrange
            var testPath = string.Concat(ProjectDirectory?.FullName, $"/{path}");

            // act 
            FileService.GetDirectoryInfo(testPath, out DirectoryInfo? dirInfo);

            // assert 02
            Assert.Multiple (() => 
            {
                Assert.That(ProjectDirectory, Is.Not.Null);
                Assert.That(dirInfo is not null, Is.EqualTo(flag));
            });
        }

        [Test, Order(2)]
        public void GetFilesByExtensions_Test() 
        {
            // arrange
            var testPath = string.Concat(ProjectDirectory?.FullName, $"/{TEST_PATH_CORRECT}");
            var testDirectorySuccess = FileService.GetDirectoryInfo(testPath, out _testDirectory); 
            
            // act 
            var filesByExtensionSuccess = FileService.SearchFilesInDirectory(_testDirectory!.FullName, out testFiles, testExtensions);

            // assert 
            Assert.Multiple(() => {
                Assert.That(testDirectorySuccess, Is.EqualTo(true));
                Assert.That(filesByExtensionSuccess, Is.EqualTo(true));
                Assert.That(testFiles, Is.Not.Null);
                Assert.That(testFiles!.Length, Is.EqualTo(4));
            });
        }

        [Test, Order(3)]
        public void RenameFiles_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(testFiles, Is.Not.Null);
                Assert.That(testFilesCount, Is.EqualTo (testFiles!.Length));
            });

            




            

        }


        [OneTimeTearDown]
        public void Finish() 
        {
            Trace.Flush();
        }

        // arrange
        // ... 

        // act 
        // ...

        // assert 
        // ... 
    }
}


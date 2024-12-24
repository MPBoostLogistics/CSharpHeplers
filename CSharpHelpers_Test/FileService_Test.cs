using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpHelpers.Models;
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
        private static readonly int testFilesCount = 4;
        private readonly string testFilesAuthor = "Mister X.";
        private readonly string[] testExtensions = 
            [FileService.FILEEXTENSION_JPG, FileService.FILEEXTENSION_JPEG, FileService.FILEEXTENSION_PDF];
        private readonly string[] testFileNames = new string[testFilesCount];

        [OneTimeSetUp]
        public void Setup()
        {
            FileService.GetProjectDirectory(out ProjectDirectory);
            Trace.Listeners.Add(new ConsoleTraceListener());

            for (int i = 0; i < testFilesCount; i++)
            {
                testFileNames[i] = Guid.NewGuid().ToString();
            }
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

            // arrange 
            for (int i = 0; i < testFiles!.Length; i++)
            {
                FileInfo? testFile = testFiles![i];
                FileService.RenameFile(new SavedFile(testFile, testFileNames[i], FileService.FILEEXTENSION_JPEG, testFilesAuthor, testFile.Directory));
            }

            // assert 
            for (int i = 0; i < testFiles.Length; i++)
            {
                Does.Contain(testFileNames.Contains(testFiles[i].Name));
            }
        }

        [Test, Order(4)]
        public void GetFilesByName_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(testFiles, Is.Not.Null);
                Assert.That(testFilesCount, Is.EqualTo (testFiles!.Length));
            });

            // arrange 
            Regex guidRegex = new (FileService.GUID_REGEX_PATTERN, RegexOptions.None, TimeSpan.FromSeconds(1));
            
            // act 
            var (matchList, missMatchList) = FileService.SearchFilesInDirectory(in testFiles!, guidRegex);

            //assert 
            Assert.Multiple(() => {
                Assert.That(matchList, Has.Count.EqualTo(testFilesCount));
                Assert.That(missMatchList, Has.Count.EqualTo(0));
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


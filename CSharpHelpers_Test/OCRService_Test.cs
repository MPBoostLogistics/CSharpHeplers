using CSharpHelpers.Models;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test 
{
    [Order (2)]
    public class OCRService_Test : Base_Test
    {
        #region Variables and constants
        private DirectoryInfo? _testDirectoryInfo;
        private FileInfo[]? _testFileInfos;
        private OCRService? _ironOcrService;
        
        #endregion

        public override void Setup()
        {
            base.Setup();

            FileService.GetDirectoryInfo(_testDirectoryPath!, out _testDirectoryInfo);
            FileService.SearchFilesInDirectory(_testDirectoryInfo!.FullName, out _testFileInfos, _testFilesExtensions);

            _ironOcrService = new(OCRServiceProvider.IronOCRProvider);
        }

        [Test, Order (1)]
        public async Task ScanDocuments_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(_testDirectoryInfo, Is.Not.Null);
                Assert.That(_testFileInfos, Is.Not.Null);
                Assert.That(_testFileInfos, Has.Length.EqualTo(TEST_FILES_COUNT));
                Assert.That(_ironOcrService, Is.Not.Null);
            });

            // arrange
            var testScanTask = _ironOcrService!.ScanTextFromFiles([.. _testFileInfos!]);

            // act 
            await Task.Run(async () => await testScanTask);
            testScanTask.Wait();

            List<ScanTextResult>? testScanTextResults = testScanTask.Result;

            // assert 
            Assert.That(testScanTextResults, Has.Count.EqualTo(TEST_FILES_COUNT));
        }
    }
}

        // arrange
        // ... 

        // act 
        // ...

        // assert 
        // ... 
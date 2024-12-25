using CSharpHelpers.Models;
using CSharpHelpers.Services;
using IronOcr;

namespace CSharpHelpers_Test 
{
    [Order (2)]
    public class OCRService_Test : Base_Test
    {
        #region Variables and constants
        private DirectoryInfo? _testDirectoryInfo;
        private FileInfo[]? _testFileInfos;
        private OCRService? _ocrService;
        

        #endregion

        public override void Setup()
        {
            base.Setup();

            FileService.GetDirectoryInfo(_testDirectoryPath!, out _testDirectoryInfo);
            FileService.SearchFilesInDirectory(_testDirectoryInfo!.FullName, out _testFileInfos, _testFilesExtensions);
        }

        [Test]
        public void ScanDocuments_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(_testDirectoryInfo, Is.Not.Null);
                Assert.That(_testFileInfos, Is.Not.Null);
                Assert.That(_testFileInfos, Has.Length.EqualTo(TEST_FILES_COUNT));
            });

            // arrange
            // ... 
            
            //_ocrService = new (OCRServiceProvider.IronOCRProvider, ironOcrKey);
            // ...

            // act 
            var ironOcrKey = Environment.GetEnvironmentVariable(OCRService.IRON_OCR_KEY);

            // assert 
            Assert.That(ironOcrKey, Is.Not.Null);


            //OCRService. 
        }
    }
}

        // arrange
        // ... 

        // act 
        // ...

        // assert 
        // ... 
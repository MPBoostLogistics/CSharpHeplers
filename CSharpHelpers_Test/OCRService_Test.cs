using CSharpHelpers.Models;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test 
{
    [Order (2)]
    public class OCRService_Test : Base_Test
    {
        [Test, Order (1)]
        public async Task ScanDocuments_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(testSourceDirectoryInfo, Is.Not.Null);
                Assert.That(testFileInfos, Is.Not.Null);
                Assert.That(testFileInfos, Has.Length.EqualTo(TEST_FILES_COUNT));
                Assert.That(ironOcrService, Is.Not.Null);
            });

            // arrange
            var testScanTask = ironOcrService!.ScanTextFromFiles([.. testFileInfos!]);

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
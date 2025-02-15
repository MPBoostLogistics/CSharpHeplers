using CSharpHelpers.Models;
using CSharpHelpers.Services;

namespace CSharpHelpers_Test
{
    [Order (3)]
    public class TextService_Test : Base_Test
    {
        [Test, Order(0)]
        public async Task Some_Test() 
        {
            // input assert 
            Assert.Multiple(() => {
                Assert.That(testSourceDirectoryInfo, Is.Not.Null);
                Assert.That(testFileInfos, Is.Not.Null);
                Assert.That(testFileInfos, Has.Length.EqualTo(TEST_FILES_COUNT));
                Assert.That(testTargetDirectoryInfo, Is.Not.Null);
                Assert.That(ironOcrService, Is.Not.Null);
            });

            // arrange
            var testScanTask = ironOcrService!.ScanTextFromFiles([.. testFileInfos!]);
            await Task.Run(async () => await testScanTask);
            testScanTask.Wait();
            List<ScanTextResult>? testScanTextResults = testScanTask.Result;

            // act 
            Task<List<(ScanTextResult result, string[]? matches)>>? findMatchingTask = 
                TextService.FindMatchesInTextsByRegex(testScanTextResults, fileNameRegex, 1);
            List<(ScanTextResult result, string[]? matches)> findMatchingTaskResults = [];
            await findMatchingTask;
                findMatchingTask.Wait();
                findMatchingTaskResults = findMatchingTask.Result;

            // assert  
            for (int i = 0; i < findMatchingTaskResults.Count; i++)
            {
                var (result, matches) = findMatchingTaskResults[i];
                if (matches is null) continue;

                //TextService.UpdateScanTextResult()
            }

        }
    }
}
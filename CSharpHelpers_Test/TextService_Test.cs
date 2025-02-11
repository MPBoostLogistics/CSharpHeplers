namespace CSharpHelpers_Test
{
    [Order (3)]
    public class TextService_Test : Base_Test
    {
        [Test, Order(0)]
        public void Some_Test() 
        {
            Assert.Pass();
        }
    }
}
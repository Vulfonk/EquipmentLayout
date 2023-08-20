namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Thread.Sleep(120000);
            Assert.AreEqual(1, 1);
            //Assert.Pass();
        }
    }
}
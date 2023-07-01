namespace Tests
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
            bool a = ELeagues.LogIn.Check("s", "s");
            bool b = ELeagues.LogIn.Check("", "s");
            bool c = ELeagues.LogIn.Check("s", "");
            bool d = ELeagues.LogIn.Check("", "");

            Assert.AreEqual(a, true);
            Assert.AreEqual(b, false);
            Assert.AreEqual(c, false);
            Assert.AreEqual(d, false);


            Assert.Pass();
        }
    }
}
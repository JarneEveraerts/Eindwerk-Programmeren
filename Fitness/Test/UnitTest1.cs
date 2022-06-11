namespace Test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("admin", null)]
        [InlineData(null, "admin")]
        public static void Beheerder_FouteInput(string? user, string? pass)
        {
            Assert.Throws<ArgumentException>(() => Test.BeheerderLogin(user, pass));
        }

        [Fact]
        public static void Beheerder_CorrectLogin()
        {
            Assert.Equal(true, Test.BeheerderLogin("admin", "admin"));
        }

        public static void Beheerder_FalseLogin()
        {
            Assert.Equal(false, Test.BeheerderLogin("admin", "admi"));
        }

        [Theory]
        [InlineData(null)]
        public static void Klant_FouteInput(string? user)
        {
            Assert.Throws<ArgumentException>(() => Test.KlantLogin(user));
        }

        [Fact]
        public static void Klant_CorrectLogin()
        {
            Assert.Equal(true, Test.KlantLogin("Brian.Thorn@hotmail.com"));
        }

        public static void Klant_FalseLogin()
        {
            Assert.Equal(false, Test.KlantLogin("jollo@hotmail.ru"));
        }

        [Theory]
        [InlineData("sdfuds/df")]
        [InlineData("sdfsd89")]
        [InlineData(null)]
        public static void AddMachine_WrongInput(string? s)
        {
            Assert.Throws<ArgumentException>(() => Test.AddMachine(s));
        }

        [Fact]
        public static void AddMachine_Correct()
        {
            Assert.Equal(true, Test.AddMachine("fiets"));
        }
    }
}
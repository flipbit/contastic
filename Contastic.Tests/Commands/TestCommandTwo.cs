namespace Contastic.Commands
{
    public class TestCommandTwo : Command<TestCommandTwo.Options>
    {
        public class Options
        {
            [Parameter("one")]
            public string One { get; set; }

            [Parameter("two")]
            public string Two { get; set; }

            [Parameter("three")]
            public string Three { get; set; }
        }

        public override int Execute(Options options)
        {
            return 0;
        }
    }
}

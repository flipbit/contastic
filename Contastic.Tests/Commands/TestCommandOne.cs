namespace Contastic.Commands
{
    public class TestCommandOne : Command<TestCommandOne.Options>
    {
        public class Options
        {
            [Parameter("one")]
            public string One { get; set; }

            [Parameter("two")]
            public string Two { get; set; }
        }

        public override int Execute(Options options)
        {
            return 0;
        }
    }
}

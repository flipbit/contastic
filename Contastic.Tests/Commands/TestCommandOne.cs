namespace Contastic.Commands
{
    public class TestCommandOne : Command<TestCommandOne.Options>
    {
        public class Options
        {
            [Parameter]
            public string One { get; set; }

            [Parameter]
            public string Two { get; set; }
        }

        public override int Execute(Options options)
        {
            return 0;
        }
    }
}

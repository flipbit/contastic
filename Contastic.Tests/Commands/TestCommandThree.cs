namespace Contastic.Commands
{
    public class TestCommandThree : Command<TestCommandThree.Options>
    {
        [Flag("flag")]
        public class Options
        {
            [Parameter]
            public string One { get; set; }

            [Parameter]
            public string Two { get; set; }

            [Parameter]
            public string Three { get; set; }
        }

        public override int Execute(Options options)
        {
            return 0;
        }
    }
}

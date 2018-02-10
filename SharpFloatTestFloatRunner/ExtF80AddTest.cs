namespace SharpFloatTestFloatRunner {
    internal class ExtF80AddTest : TestEntry {


        public override string Name
            => "add";

        internal override void ProcessLineInTest(string[] data) {
            var op1 = TryStrToFloat(data[0]);
            var op2 = TryStrToFloat(data[1]);
            var gr = TryStrToFloat(data[2]);
            var or = op1 + op2;
            if (gr.signExp != or.signExp)
                throw new System.Exception(gr.signExp.ToString("x") + " != " + or.signExp.ToString("x"));
            if (gr.signif != or.signif)
                throw new System.Exception(gr.signif.ToString("x") + " != " + or.signif.ToString("x"));

        }
    }
}
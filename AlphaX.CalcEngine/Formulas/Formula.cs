namespace AlphaX.CalcEngine.Formulas
{
    public abstract class Formula
    {
        public int MinArgs { get; set; }
        public int MaxArgs { get; set; }
        public string Name { get; private set; }

        public Formula(string name)
        {
            Name = name;
        }

        public abstract CalcValue Calculate(params CalcValue[] values);

        public abstract string GetDescription();
    }
}

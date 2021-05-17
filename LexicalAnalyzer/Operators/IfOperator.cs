
namespace LexicalAnalyzer
{
    class IfOperator : Operator
    {
        // Имеет ли if оператор ветку else
        private bool hasElseBranch;

        // Метка условного перехода по значению истина
        private string labelUPI;

        // Метка условного перехода по значению ложь
        private string labelUPL;

        public IfOperator() : base("if")
        {
            labelUPL = LabelsManager.GetNewLabel();
            labelUPI = "";
            hasElseBranch = false;
        }

        public string GetLabelUPI()
        {
            if (labelUPI.Length == 0)
            {
                labelUPI = LabelsManager.GetNewLabel();
            }
            return labelUPI;
        }

        // Возвращает метку УПЛ
        public string GetLabelUPL()
        { 
            return labelUPL;
        }

        public bool HasElseBrach()
        {
            return hasElseBranch;
        }

        public void SetHasElseBranch(bool _val)
        {
            hasElseBranch = _val;
        }
    }
}

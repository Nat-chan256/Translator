using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class TwoLiterOpState : State
    {
        public TwoLiterOpState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            string curSymbol = machine.GetBufferContent() + _ch;

            if (IsTwoLiterOp(curSymbol))
            {
                machine.AddToBuffer(_ch);
                SemanticProcedure6();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
            }
            else if (IsIdentifierSymbol(_ch) || IsDivider(_ch) || _ch == '\'' || _ch == '\"' || _ch == '-')
            {
                SemanticProcedure6();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}

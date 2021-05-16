using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class PointState : State
    {
        public PointState(StateMachine _machine) : base(_machine)
        {

        }


        public override void NextChar(char _ch)
        {
            if (Char.IsDigit(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new FloatNumState(machine));
            }
            else if (IsIdentifierSymbol(_ch))
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

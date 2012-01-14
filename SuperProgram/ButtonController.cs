//Created by Evgeniya Martynova
//Corsunina Core: controller of buttons state
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperProgram
{
    enum ButtonState { CleanPressed, StopEnteringPressed, MakeSinglePressed, RenumerationPressed, TriangulationPressed, KathMaccPressed, SolvePressed }

    class ButtonController
    {
        private static ButtonController instance = null;
        public ButtonState CurrentState{ get; set;}
        protected ButtonController()
        { }

        public static ButtonController Instance()
        {
            if (instance == null)
            {
                instance = new ButtonController();
            }
            return instance;
        }

        public void changeButtonState(Button clean, Button stop, Button makeSingle, Button renumerate, Button triangle, Button kathMacc, Button solve)
        {
            switch (CurrentState)
            {
                case ButtonState.CleanPressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = true;
                        makeSingle.Enabled = false;
                        renumerate.Enabled = false;
                        triangle.Enabled = false;
                        kathMacc.Enabled = false;
                        solve.Enabled = false;
                    }
                    break;
                case ButtonState.StopEnteringPressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = true;
                        renumerate.Enabled = false;
                        triangle.Enabled = false;
                        kathMacc.Enabled = false;
                        solve.Enabled = false;
                    }
                    break;
                case ButtonState.MakeSinglePressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = false;
                        renumerate.Enabled = true;
                        triangle.Enabled = false;
                        kathMacc.Enabled = false;
                        solve.Enabled = false;
                    }
                    break;
                case ButtonState.RenumerationPressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = false;
                        //Not sure about it
                        renumerate.Enabled = false;
                        triangle.Enabled = true;
                        kathMacc.Enabled = false;
                        solve.Enabled = false;
                    }
                    break;
                case ButtonState.TriangulationPressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = false;
                        renumerate.Enabled = false;
                        triangle.Enabled = false;
                        kathMacc.Enabled = true;
                        solve.Enabled = false;
                    }
                    break;
                case ButtonState.KathMaccPressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = false;
                        renumerate.Enabled = false;
                        triangle.Enabled = false;
                        kathMacc.Enabled = false;
                        solve.Enabled = true;
                    }
                    break;
                case ButtonState.SolvePressed:
                    {
                        clean.Enabled = true;
                        stop.Enabled = false;
                        makeSingle.Enabled = false;
                        renumerate.Enabled = false;
                        triangle.Enabled = false;
                        kathMacc.Enabled = false;
                        solve.Enabled = false;
                    }
                    break;
            }
        }

    }
}

using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Core;
using SuperProgram.CodeBehind;
using Point = Core.Point;
// Julia
using Triangulator = Triangulation.Triangulator;
using System.Collections.Generic;
// end Julia
//Evgeniya
using KathMaccRenumerator;
//end Evgeniya

namespace SuperProgram
{
    public partial class FormMain : Form
    {
        private PointsInputManager _pointsInputManager;
        private GeometryDrawer _geometryDrawer;
        private TabControlHelper _tabControlHelper;
        private Contour _singleContour;

        // Julia
        private List<Triangle> _triangleList;
        // end Julia

        //Evgenij
        private Contour _contourPoints;

        private double[,] matrix_system;
        private double[] vector_system;

        private double[] solution;

        private void initSystem()
        {
            matrix_system = new double[_singleContour.Count, _singleContour.Count];
            vector_system = new double[_singleContour.Count];
        }
        //end Evgenij

        public FormMain()
        {
            InitializeComponent();
            SetUp();
            //Evgeniya
            ButtonController.Instance().CurrentState = ButtonState.CleanPressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
            //end Evgeniya
        }

        private void SetUp()
        {
            _geometryDrawer = new GeometryDrawer(pbDrawField);
            _tabControlHelper = new TabControlHelper(tabCtrlContours);
            _pointsInputManager = new PointsInputManager(_geometryDrawer, _tabControlHelper);
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            pbDrawField.Image = new Bitmap(pbDrawField.Width, pbDrawField.Height);
            Graphics gr = Graphics.FromImage(pbDrawField.Image);
            gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
            pbDrawField.MouseClick += _pointsInputManager.PointsInputHandler;
        }

        private void btnCompleteInput_Click(object sender, EventArgs e)
        {
            pbDrawField.MouseClick -= _pointsInputManager.PointsInputHandler;
            ButtonController.Instance().CurrentState = ButtonState.StopEnteringPressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(pbDrawField.Image);
            gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
            pbDrawField.Refresh();
            _pointsInputManager.Reset();
            tabCtrlContours.TabPages.Clear();
            pbDrawField.MouseClick -= _pointsInputManager.PointsInputHandler;
            pbDrawField.MouseClick += _pointsInputManager.PointsInputHandler;
            //Evgeniya
            ButtonController.Instance().CurrentState = ButtonState.CleanPressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
            //end Evgeniya
        }

        private void pbDrawField_Paint(object sender, PaintEventArgs e)
        {
            if (_geometryDrawer.LastDrawnFrame != null)
            {
                Graphics gr = e.Graphics;
                gr.DrawImage(_geometryDrawer.LastDrawnFrame, 0, 0);
            }
        }

        private void btnGetSingleContour_Click(object sender, EventArgs e)
        {
            try
            {
                _singleContour = _pointsInputManager.GetSingleContour();
                tabCtrlContours.TabPages.Clear();
                _tabControlHelper.CreatePageForContour(_singleContour);
                Graphics gr = Graphics.FromImage(pbDrawField.Image);
                gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
                _geometryDrawer.DrawContours(Pens.Black, _singleContour);
                //Evgeniya
                ButtonController.Instance().CurrentState = ButtonState.MakeSinglePressed;
                ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
                //end Evgeniya
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void btnNetReculc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите значение шага", "Ошибка");
            }
            else
            {
                int step;
                if (int.TryParse(textBox1.Text, out step))
                {
                    // Julia
                    int count = _singleContour.Count();
                    int min = (int)Triangulation.BPoint.dist(_singleContour[count - 1], _singleContour[0]);
                    for (int i = 0; i < count - 1; i++)
                    {
                        int cur = (int)Triangulation.BPoint.dist(_singleContour[i], _singleContour[i + 1]);
                        if (cur < min)
                            min = cur;
                    }
                    if (min < step)
                        step = min;
                    // end Julia

                    _singleContour.RecalculateWithStep(step);
                    tabCtrlContours.TabPages.Clear();
                    _tabControlHelper.CreatePageForContour(_singleContour);
                    Graphics gr = Graphics.FromImage(pbDrawField.Image);
                    gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
                    _geometryDrawer.DrawContours(Pens.Black, _singleContour);
                    //Evgeniya
                    ButtonController.Instance().CurrentState = ButtonState.RenumerationPressed;
                    ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
                    //end Evgeniya
                }
                else
                {
                    MessageBox.Show("Неверный ввод. Шаг должен быть положительным целым числом", "Ошибка");
                }
            }
        }

        // Julia
        private void btnTriangulate_Click(object sender, EventArgs e)
        {
            //Evgenij
            _contourPoints = new Contour(_singleContour.Index);
            _contourPoints.AddRange(_singleContour);
            //end Evgenij

            Triangulator triangulator = new Triangulator();
            _triangleList = (triangulator.Triangulate(_singleContour)) as List<Triangle>;
            _geometryDrawer.DrawTriangles(Pens.Black, _triangleList.ToArray());
            //Evgeniya
            ButtonController.Instance().CurrentState = ButtonState.TriangulationPressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
            //end Evgeniya
        }
        // end Julia

        //Evgeniya
        private void btnRenumerator_Click(object sender, EventArgs e)
        {
            Renumerator.DoRenumeration(_triangleList);
            //just for visualization
            Graphics gr = Graphics.FromImage(pbDrawField.Image);
            gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
            pbDrawField.Refresh();
            _geometryDrawer.DrawTriangles(Pens.Black, _triangleList.ToArray());
            ButtonController.Instance().CurrentState = ButtonState.KathMaccPressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
        }
        //end Evgeniya

        //Evgenij
        private void btnSolve_Click(object sender, EventArgs e)
        {
            initSystem();
            SystemMaker.MakeSystem(ref _triangleList, ref _contourPoints, ref matrix_system, ref vector_system);
            solution = Cholesky.CholeskySolver.Solve(matrix_system, vector_system);

            //Evgeniya
            ButtonController.Instance().CurrentState = ButtonState.SolvePressed;
            ButtonController.Instance().changeButtonState(btnClear, btnCompleteInput, btnGetSingleContour, btnNetReculc, btnTriangulate, btnRenumerator, btnSolve);
            //end Evgeniya
        }
        //end Evgenij

    }
}

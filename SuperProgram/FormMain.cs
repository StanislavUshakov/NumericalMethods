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

namespace SuperProgram {
	public partial class FormMain : Form{
		private PointsInputManager _pointsInputManager;
		private GeometryDrawer _geometryDrawer;
		private TabControlHelper _tabControlHelper;
		private Contour _singleContour;
        // Julia
        private List<Triangle> _triangleList;
        // end Julia

		public FormMain() {
			InitializeComponent();
			SetUp();
		}

		private void SetUp(){
			_geometryDrawer = new GeometryDrawer(pbDrawField);
			_tabControlHelper = new TabControlHelper(tabCtrlContours);
			_pointsInputManager = new PointsInputManager(_geometryDrawer, _tabControlHelper);
		}

		private void FormMain_Shown(object sender, EventArgs e){
			pbDrawField.Image = new Bitmap(pbDrawField.Width, pbDrawField.Height);
			Graphics gr = Graphics.FromImage(pbDrawField.Image);
			gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
			pbDrawField.MouseClick += _pointsInputManager.PointsInputHandler;
		}

		private void btnCompleteInput_Click(object sender, EventArgs e) {
			pbDrawField.MouseClick -= _pointsInputManager.PointsInputHandler;
		}

		private void btnClear_Click(object sender, EventArgs e) {
			Graphics gr = Graphics.FromImage(pbDrawField.Image);
			gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
			pbDrawField.Refresh();
			_pointsInputManager.Reset();
			tabCtrlContours.TabPages.Clear();
			pbDrawField.MouseClick -= _pointsInputManager.PointsInputHandler;
			pbDrawField.MouseClick += _pointsInputManager.PointsInputHandler;
		}

		private void pbDrawField_Paint(object sender, PaintEventArgs e) {
			if (_geometryDrawer.LastDrawnFrame != null){
				Graphics gr = e.Graphics;
				gr.DrawImage(_geometryDrawer.LastDrawnFrame, 0, 0);
			}
		}

		private void btnGetSingleContour_Click(object sender, EventArgs e){
			try{
				_singleContour = _pointsInputManager.GetSingleContour();
				tabCtrlContours.TabPages.Clear();
				_tabControlHelper.CreatePageForContour(_singleContour);
				Graphics gr = Graphics.FromImage(pbDrawField.Image);
				gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
				_geometryDrawer.DrawContours(Pens.Black, _singleContour);
			} catch(Exception ex){
				MessageBox.Show(ex.Message, "Ошибка");
			}
		}

		private void btnNetReculc_Click(object sender, EventArgs e){
			if (string.IsNullOrEmpty(textBox1.Text)) {
				MessageBox.Show("Введите значение шага", "Ошибка");
			} else{
				int step;
				if (int.TryParse(textBox1.Text, out step)){
					_singleContour.RecalculateWithStep(step);
					tabCtrlContours.TabPages.Clear();
					_tabControlHelper.CreatePageForContour(_singleContour);
					Graphics gr = Graphics.FromImage(pbDrawField.Image);
					gr.FillRectangle(Brushes.White, 0, 0, pbDrawField.Width, pbDrawField.Height);
					_geometryDrawer.DrawContours(Pens.Black, _singleContour);
				} else{
					MessageBox.Show("Неверный ввод. Шаг должен быть положительным целым числом", "Ошибка");
				}
			}
		}

        // Julia
        private void btnTriangulate_Click(object sender, EventArgs e)
        {
            Triangulator triangulator = new Triangulator();
            _triangleList = (triangulator.Triangulate(_singleContour)) as List<Triangle>;
            _geometryDrawer.DrawTriangles(Pens.Black, _triangleList.ToArray());
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
        }
        //end Evgeniya
	}
}

using System.Drawing;
using System.Windows.Forms;
using Core;

namespace SuperProgram.CodeBehind {
	public class TabControlHelper{
		private readonly TabControl _control;

		public TabControlHelper(TabControl control){
			_control = control;
		}

		public void CreatePageForContour(Contour contour){
			var page = new TabPage {
				Size = new Size(_control.Width - 8, _control.Height - 26),
				Text = string.Format("Контур {0}", contour.Index)
			};
			var grid = new ContourDataGridView{Size = new Size(page.Width, page.Height), DataSource = contour};
			page.Controls.Add(grid);
			_control.TabPages.Add(page);
		}
	}
}

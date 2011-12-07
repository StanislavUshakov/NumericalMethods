using System.Windows.Forms;

namespace SuperProgram.CodeBehind {
	public class ContourDataGridView : DataGridView {
		public ContourDataGridView(){
			RowHeadersVisible = false;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			AllowUserToResizeColumns = false;
			AllowUserToResizeRows = false;
			DataError += grid_DataError;
		}

		private static void grid_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			MessageBox.Show("Некорректный ввод", "Ошибка");
		}
	}
}

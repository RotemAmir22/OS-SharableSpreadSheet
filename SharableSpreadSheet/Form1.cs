using System.Windows.Forms;

namespace SharableSpreadSheet
{
    public partial class Form1 : Form
    {
        DataGridView viewer;
        public Form1()
        {
            InitializeComponent();
            viewer = new DataGridView();
        }

        public void SetViewer(int x, int y, int nCols, int nRows)
        {
            viewer.SetBounds(0, 0, nCols, nRows);
        }

        public string GetCellInViewer(int row, int col)
        {
            return viewer.Rows[row].Cells[col].ToString();
        }

        public void SetCellInViewer(int row, int col, String info)
        {
            viewer.Rows[row].Cells[col].Value = info;
        }
        
        public void SearchInViewer(String str, out int row, out int col) 
        {
            for(int i=0; i<viewer.Rows.Count; i++)
            {
                for(int j=0; j < viewer.Columns.Count; j++) 
                {
                    if(viewer.Rows[i].Cells[j].Value.Equals(str))
                    {
                        row = i; col = j; break;    
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (viewer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                MessageBox.Show(viewer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }
    }
}
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

        public void SwitchRows(int row1, int row2)
        {
            for(int i=0; i<viewer.Columns.Count; i++) 
            {
                DataGridViewCell tmp = viewer.Rows[row1].Cells[i];
                viewer.Rows[row1].Cells[i].Value = viewer.Rows[row2].Cells[i].Value;
                viewer.Rows[row2].Cells[i].Value = tmp.Value;
            }
        }

        public void SwitchCols(int col1, int col2)
        {
            for (int i = 0; i < viewer.Rows.Count; i++)
            {
                DataGridViewCell tmp = viewer.Rows[i].Cells[col1];
                viewer.Rows[i].Cells[col1].Value = viewer.Rows[i].Cells[col2].Value;
                viewer.Rows[i].Cells[col2].Value = tmp.Value;
            }
        }

        public void SearchInRow(int row, String str, out int col)
        {
            col = -1;
            for(int i = 0; i<viewer.Columns.Count;i++)
                if (viewer.Rows[row].Cells[i].Value.Equals(str))
                    col = i; 
        }

        public void SearchInCol(int col, String str, out int row)
        {
            row = -1;
            for (int i = 0; i < viewer.Rows.Count; i++)
                if (viewer.Rows[i].Cells[col].Value.Equals(str))
                    row = i;
        }

        public void SearchInViewer(String str, out int row, out int col) 
        {
            row = 0; col = 0;
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

        public void AddRow(int row)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            for(int i=0; i<viewer.Columns.Count; i++)
                newRow.Cells.Add(new DataGridViewTextBoxCell());
            viewer.Rows.Insert(row+1, newRow);
        }

        public void AddCol(int col)
        {
            DataGridViewColumn newCol = new DataGridViewColumn();
            viewer.Columns.Insert(col+1, newCol);
        }

        public List<Tuple<int, int>> FindAll(String str, bool caseSensitive)
        {
            List<Tuple<int, int>> matchingCells = new List<Tuple<int, int>>();

            // Iterate over each row in the DataGridView
            foreach (DataGridViewRow row in viewer.Rows)
            {
                // Iterate over each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Perform the search based on case sensitivity flag
                    bool isMatch;
                    if (caseSensitive)
                        isMatch = cell.Value?.ToString().Contains(str) == true;
                    else
                        isMatch = cell.Value?.ToString().IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;

                    // If the cell value matches, add the cell coordinates to the list
                    if (isMatch)
                    {
                        int rowIndex = cell.RowIndex;
                        int columnIndex = cell.ColumnIndex;
                        matchingCells.Add(new Tuple<int, int>(rowIndex, columnIndex));
                    }
                }
            }

            return matchingCells;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (viewer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                MessageBox.Show(viewer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }
    }
}
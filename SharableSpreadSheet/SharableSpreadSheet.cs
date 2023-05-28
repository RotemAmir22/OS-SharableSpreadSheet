using System;
using System.Windows.Forms;

namespace SharableSpreadSheet
{
    class SharableSpreadSheet
    {
        DataGridView dataGrid;
        int nR;
        int nC;
        public SharableSpreadSheet(int nRows, int nCols, int nUsers = -1)
        {
            // nUsers used for setConcurrentSearchLimit, -1 mean no limit.
            // construct a nRows*nCols spreadsheet
            dataGrid = new DataGridView();
            nR = nRows;
            nC = nCols;
            dataGrid.SetBounds(0, 0, nCols, nRows);

        }
        public String getCell(int row, int col)
        {
            // return the string at [row,col]
            return dataGrid.Rows[row].Cells[col].ToString();
        }
        public void setCell(int row, int col, String str)
        {
            // set the string at [row,col]
            dataGrid.Rows[row].Cells[col].Value = str;

        }
        public Tuple<int, int> searchString(String str)
        {
            int row = -1, col = -1;
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Rows[i].Cells[j].Value.Equals(str))
                    {
                        row = i; col = j; break;
                    }
                }
            }
            // return first cell indexes that contains the string (search from first row to the last row)
            return Tuple.Create(row, col);
        }
        public void exchangeRows(int row1, int row2)
        {
            // exchange the content of row1 and row2
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                DataGridViewCell tmp = dataGrid.Rows[row1].Cells[i];
                dataGrid.Rows[row1].Cells[i].Value = dataGrid.Rows[row2].Cells[i].Value;
                dataGrid.Rows[row2].Cells[i].Value = tmp.Value;
            }
        }
        public void exchangeCols(int col1, int col2)
        {
            // exchange the content of col1 and col2
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                DataGridViewCell tmp = dataGrid.Rows[i].Cells[col1];
                dataGrid.Rows[i].Cells[col1].Value = dataGrid.Rows[i].Cells[col2].Value;
                dataGrid.Rows[i].Cells[col2].Value = tmp.Value;
            }
        }
        public int searchInRow(int row, String str)
        {
            int col = -1;
            // perform search in specific row
            for (int i = 0; i < dataGrid.Columns.Count; i++)
                if (dataGrid.Rows[row].Cells[i].Value.Equals(str))
                {
                    col = i;
                    break;
                }
            return col;
        }
        public int searchInCol(int col, String str)
        {
            int row = -1;
            for (int i = 0; i < dataGrid.Rows.Count; i++) 
            { 
                if (dataGrid.Rows[i].Cells[col].Value.Equals(str))
                {
                    row = i;
                    break;
                }
            }
            return row;
        }
        public Tuple<int, int> searchInRange(int col1, int col2, int row1, int row2, String str)
        {
            // perform search within spesific range: [row1:row2,col1:col2] 
            //includes col1,col2,row1,row2
            for(int i = col1; i <= col2; i++)
            {
                for(int j = row1; j <= row2; j++)
                {
                    if (getCell(j, i).Equals(str))
                        return Tuple.Create(i, j);
                }
            }
            return Tuple.Create(-1, -1);
        }
        public void addRow(int row1)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
                newRow.Cells.Add(new DataGridViewTextBoxCell());
            dataGrid.Rows.Insert(row1 + 1, newRow);
            nR++;
        }
        public void addCol(int col1)
        {
            //add a column after col1
            DataGridViewColumn newCol = new DataGridViewColumn();
            dataGrid.Columns.Insert(col1 + 1, newCol);
            nC++;
        }
        public Tuple<int, int>[] findAll(String str,bool caseSensitive)
        {
            List<Tuple<int, int>> matchingCells = new List<Tuple<int, int>>();

            // Iterate over each row in the DataGridView
            foreach (DataGridViewRow row in dataGrid.Rows)
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
            return matchingCells.ToArray();
        }
        public void setAll(String oldStr, String newStr ,bool caseSensitive)
        {
            // replace all oldStr cells with the newStr str according to caseSensitive param
            Tuple<int, int>[] cellsToSet = findAll(oldStr, caseSensitive);
            foreach(Tuple<int,int> cell in cellsToSet)
                setCell(cell.Item1, cell.Item2, newStr);
        }
        public Tuple<int, int> getSize()
        {
            // return the size of the spreadsheet in nRows, nCols
            return Tuple.Create(nR, nC);
        }

        public void save(String fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                for (int row = 0; row < nR; row++)
                {
                    for (int col = 0; col < nC; col++)
                    {
                        string cellValue = getCell(row, col); // Replace with your own method to retrieve cell value
                        writer.Write(cellValue);
                        if (col < nC - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }
        public void load(String fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                int row = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] cellValues = line.Split(',');
                    int col = 0;

                    foreach (string cellValue in cellValues)
                    {
                        setCell(row, col, cellValue.Trim()); // Replace with your own method to set the cell value
                        col++;
                    }

                    row++;
                }
            }
        }
    }
}





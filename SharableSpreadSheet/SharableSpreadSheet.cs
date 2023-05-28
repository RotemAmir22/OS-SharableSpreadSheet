using System;
using System.Windows.Forms;

namespace SharableSpreadSheet
{
    class SharableSpreadSheet
    {
        Form1 dataGrid;
        int nR;
        int nC;
        public SharableSpreadSheet(int nRows, int nCols, int nUsers=-1)
        {
            // nUsers used for setConcurrentSearchLimit, -1 mean no limit.
            // construct a nRows*nCols spreadsheet
            dataGrid = new Form1();
            nR = nRows; 
            nC = nCols;
            dataGrid.SetViewer(0, 0, nCols, nRows);
               
        }
        public String getCell(int row, int col)
        {
            // return the string at [row,col]
            return dataGrid.GetCellInViewer(row, col);
        }
        public void setCell(int row, int col, String str)
        {
            // set the string at [row,col]
            dataGrid.SetCellInViewer(row, col, str);
   
        }
        public Tuple<int,int> searchString(String str)
        {
            int row, col;
            dataGrid.SearchInViewer(str, out row, out col);
            // return first cell indexes that contains the string (search from first row to the last row)
            return Tuple.Create(row, col);
        }
        public void exchangeRows(int row1, int row2)
        {
            // exchange the content of row1 and row2
            dataGrid.SwitchRows(row1, row2);
        }
        public void exchangeCols(int col1, int col2)
        {
            // exchange the content of col1 and col2
            dataGrid.SwitchCols(col1, col2);
        }
        public int searchInRow(int row, String str)
        {
            int col;
            // perform search in specific row
            dataGrid.SearchInRow(row, str, out col);
            return col;
        }
        public int searchInCol(int col, String str)
        {
            int row;
            // perform search in specific col
            dataGrid.SearchInCol(col, str, out row);
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
                    if (dataGrid.GetCellInViewer(j, i).Equals(str))
                        return Tuple.Create(i, j);
                }
            }
            return Tuple.Create(-1, -1);
        }
        public void addRow(int row1)
        {
            dataGrid.AddRow(row1);
            nR++;
        }
        public void addCol(int col1)
        {
            //add a column after col1
            dataGrid.AddCol(col1);
            nC++;
        }
        public Tuple<int, int>[] findAll(String str,bool caseSensitive)
        {
            List<Tuple<int, int>> matchingCells = dataGrid.FindAll(str, caseSensitive);
            return matchingCells.ToArray();
        }
        public void setAll(String oldStr, String newStr ,bool caseSensitive)
        {
            // replace all oldStr cells with the newStr str according to caseSensitive param
            Tuple<int, int>[] cellsToSet = findAll(oldStr, caseSensitive);
            foreach(Tuple<int,int> cell in cellsToSet)
                dataGrid.SetCellInViewer(cell.Item1, cell.Item2, newStr);
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
                        string cellValue = dataGrid.GetCellInViewer(row, col); // Replace with your own method to retrieve cell value
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
                        dataGrid.SetCellInViewer(row, col, cellValue.Trim()); // Replace with your own method to set the cell value
                        col++;
                    }

                    row++;
                }
            }
        }
    }
}





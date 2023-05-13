using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class TableView
    {
        internal void DisplayTable(List<CodingSession> tableData)
        {
            ConsoleTableBuilder.From(tableData).WithTitle("Coding Sessions").ExportAndWriteLine();
        }
    }
}

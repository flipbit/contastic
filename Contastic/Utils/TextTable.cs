using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contastic.Utils
{
    /// <summary>
    /// Textual table for console applications.
    /// </summary>
    public class TextTable
    {
        private readonly List<List<string>> rows;
        private readonly List<RowType> rowTypes;
        private readonly List<Align> alignments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTable"/> class.
        /// </summary>
        public TextTable()
        {
            rows = new List<List<string>>();
            rowTypes = new List<RowType>();
            alignments = new List<Align>();

            NewLine = Environment.NewLine;
            ColumnSeparator = " ";
        }

        public string NewLine { get; set; }

        public string ColumnSeparator { get; set; }

        /// <summary>
        /// Adds a separator to the table.
        /// </summary>
        public void AddSeparator()
        {
            AddRow("=");
        }

        public void AddHeader(string value)
        {
            var row = GetNextRow();

            row.Add(value);
            rowTypes.Add(RowType.Header);
        }

        /// <summary>
        /// Adds a row to this table.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddRow(params object[] values)
        {
            var row = GetNextRow();

            foreach (var value in values)
            {
                row.Add(value?.ToString() ?? string.Empty);
            }

            rowTypes.Add(RowType.Data);
        }

        public void Write(IWriter writer)
        {
            var rowIndex = 0;
            var widths = GetColumnWidths();

            foreach (var row in rows)
            {
                var rowType = rowTypes[rowIndex];

                switch (rowType)
                {
                    case RowType.Header:
                        WriteHeader(writer, row);
                        break;
                        
                    case RowType.Data:
                        WriteData(writer, row, widths);
                        break;

                    default:
                        throw new Exception($"Unknown column type: {rowType}");
                }

                writer.Write(NewLine);

                rowIndex++;
            }
        }

        private void WriteData(IWriter writer, List<string> row, List<int> widths)
        {
            var columnIndex = 0;

            foreach (var cell in row)
            {
                if (columnIndex > 0)
                {
                    writer.Write(ColumnSeparator);
                }

                var value = cell ?? string.Empty;
                var alignment = GetAlignment(columnIndex);

                switch (alignment)
                {
                    case Utils.Align.Right:
                        value = value.PadLeft(widths[columnIndex]);
                        break;

                    case Utils.Align.Left:
                        value = value.PadRight(widths[columnIndex]);
                        break;

                    default:
                        throw new ContasticException($"Unknown column alignment: {alignment}");
                }

                writer.Write(value);

                columnIndex++;
            }
        }

        private void WriteHeader(IWriter writer, List<string> row)
        {
            writer.Write(row.FirstOrDefault());
        }

        public void WriteConsole()
        {
            Write(new ConsoleWriter());
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var writer = new StringWriter();

            Write(writer);

            return writer.ToString();
        }

        private Align GetAlignment(int columnIndex)
        {
            if (columnIndex >= alignments.Count) return Utils.Align.Left;

            return alignments[columnIndex];
        }

        private List<string> GetNextRow()
        {
            var row = new List<string>();

            rows.Add(row);

            return row;
        }

        private int GetMaximumRowLength()
        {
            var length = 0;

            foreach (var row in rows)
            {
                if (row.Count > length) length = row.Count;
            }

            return length;
        }

        private List<int> GetColumnWidths()
        {
            var widths = Enumerable
                .Range(0, GetMaximumRowLength())
                .Select(r => 0)
                .ToList();

            var rowIndex = 0;
            foreach (var row in rows)
            {
                if (rowTypes[rowIndex] == RowType.Header)
                {
                    rowIndex++;
                    continue;
                }

                for (var i = 0; i < row.Count; i++)
                {
                    var column = row[i];

                    if (string.IsNullOrEmpty(column)) continue;

                    if (column.Length > widths[i]) widths[i] = column.Length;
                }

                rowIndex++;
            }

            return widths;
        }

        private enum RowType
        {
            Data,
            Header
        }


        public void Align(params Align[] columnAlignments)
        {
            if (columnAlignments == null) return;

            alignments.Clear();
            alignments.AddRange(columnAlignments);
        }
    }
}

using System.Collections;
using System.Collections.Generic;

namespace eLog.HeavyTools.ImportBase
{
    public abstract class ImportResultSetsBase<T> : IEnumerable<T>
        where T: ImportResultSetBase
    {
        private List<T> resultSets = new List<T>();

        public void Add(T resultSet)
        {
            this.resultSets.Add(resultSet);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.resultSets).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.resultSets).GetEnumerator();
        }

        public int? TotalRows { get; set; }
        public int? ProcessedRows { get; set; }

        public int? FirstHeaderRow { get; set; }
        public int? LastHeaderRow { get; set; }
        public int? LogColIndex { get; set; }
    }
}

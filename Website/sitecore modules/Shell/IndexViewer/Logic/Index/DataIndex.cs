using System.Collections;

namespace IndexViewer
{
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Indexing;
    using Sitecore.Diagnostics;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;

    public class DataIndex : BaseIndex
    {
        #region fields

        private readonly Index _index;
        private readonly Database _database;

        #endregion fields

        #region constructors

        public DataIndex(Index index, string databaseName)
        {
            Assert.ArgumentNotNull(index, "index");
            Assert.ArgumentNotNull(databaseName, "databaseName");

            _index = index;
            _database = Factory.GetDatabase(databaseName);
        }

        #endregion constructors

        #region public properties

        public override string Name
        {
            get
            {
                return _index.Name;
            }
        }

        public override ICollection Fields
        {
            get
            {
                IndexReader reader = CreateReader();

                if (reader != null)
                {
                    try
                    {
                        return (ICollection) reader.GetFieldNames(IndexReader.FieldOption.ALL);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }

                return null;
            }
        }

        #endregion public properties

        #region public methods

        public override IndexReader CreateReader()
        {
            return _index.OpenReader(_database);
        }

        public override IndexSearcher CreateSearcher()
        {
            if (this.Searcher == null)
            {
                this.Searcher = _index.GetSearcher(_database);
            }

            return this.Searcher;
        }

        public override int GetDocumentCount()
        {
            return _index.GetDocumentCount(_database);
        }

        public override Directory CreateDirectory()
        {
            return _index.GetIndexDirectory(_database);
        }

        public override void Rebuild()
        {
            this.CloseSearcher();

            _index.Rebuild(_database);
        }

        public override void Optimize()
        {
            this.CloseSearcher();
            
            lock (_index.SyncRoot)
            {
                using (new LockScope(CreateDirectory(), "write.lock"))
                {
                    IndexWriter writer = _index.OpenWriter(_database);
                    writer.Optimize();
                    writer.Close();
                }
            }
        }

        #endregion public methods
    }
}

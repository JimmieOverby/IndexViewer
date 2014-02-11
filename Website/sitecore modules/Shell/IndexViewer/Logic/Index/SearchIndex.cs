using System;
using System.Reflection;
using System.Collections;

namespace IndexViewer
{
    using Sitecore.Search;
    using Sitecore.Diagnostics;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using System.Collections.Generic;


    public class SearchIndex : BaseIndex
    {
        #region fields

        private readonly Index _index;

        #endregion fields

        #region constructors

        public SearchIndex(Index index)
        {
            Assert.ArgumentNotNull(index, "index");

            _index = index;
        }

        #endregion constructors

        #region public properties

        public override IndexReader Reader
        {
            get
            {
                return CreateReader();
            }
        }

        public override string Name
        {
            get
            {
                return _index.Name;
            }
        }

        public override ICollection<string> Fields
        {
            get
            {
                IndexReader reader = Reader;//CreateReader();

                return reader.GetFieldNames(IndexReader.FieldOption.ALL);
            }
        }

        #endregion public properties

        #region public methods

        public IndexReader CreateReader()
        {
            IndexReader reader = DirectoryReader.Open(this._index.Directory, true);
            return reader;
        }

        public override IndexSearcher CreateSearcher()
        {
            if (this.Searcher == null)
            {
                MethodInfo methodInfo = typeof(Index).GetMethod("CreateSearcher",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                this.Searcher = methodInfo.Invoke(_index, new object[] { true }) as IndexSearcher;
            }

            return this.Searcher;
        }


        public override int GetDocumentCount()
        {
            return _index.GetDocumentCount();
        }


        public override Directory CreateDirectory()
        {
            Type indexType = typeof(Index);

            Directory directory = indexType.InvokeMember("_directory",
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                null,
                                _index,
                                null) as Directory;


            return directory;
        }

        public override void Rebuild()
        {
            this.CloseSearcher();

            _index.Rebuild();
        }

        public override void Optimize()
        {
            this.CloseSearcher();

            using (IndexUpdateContext context = _index.CreateUpdateContext())
            {
                Type indexUpdateContextType = typeof(IndexUpdateContext);

                IndexWriter writer = indexUpdateContextType.InvokeMember("_writer",
                                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                    null,
                                    context,
                                    null) as IndexWriter;

                if (writer != null)
                {
                    writer.Optimize();
                }
                
                context.Commit();
            }
        }

        #endregion public methods

    }
}

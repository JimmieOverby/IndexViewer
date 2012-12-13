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
            MethodInfo methodInfo = typeof(Index).GetMethod("CreateReader",
                    BindingFlags.NonPublic | BindingFlags.Instance);

            return methodInfo.Invoke(_index, null) as IndexReader;            
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

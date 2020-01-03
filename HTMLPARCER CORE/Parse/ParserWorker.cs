using AngleSharp.Html.Parser;
using System;
using System.Text;

namespace HTMLPARCER_CORE.Parse
{
    public class ParserWorker<T> where T : class
    {

        IParser<T> parser;
        IParserSettings parserSettings;

        HtmlLoader loader;

        bool isActive;

        #region Properties

        public IParser<T> Parser
        {
            get {
                return parser;
            }
            set {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get {
                return parserSettings;
            }
            set {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get {
                return isActive;
            }
        }

        #endregion

        public event Action<object, T> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        private async void Worker()
        {
            int count = Settings.MaxPage == 0 ? Settings.MinPage : Settings.MaxPage;
            for (int i = Settings.MinPage; i <= count; i++)
            {
                if (!isActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }
                ;
                var source = await loader.GetSource(i);
                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);
                
                var result = parser.Parse(document);
                if (parser.GetCount() == 0)
                    break;

                OnNewData?.Invoke(this, result);
            }
           

            OnCompleted?.Invoke(this);
            isActive = false;
        }


    }
}

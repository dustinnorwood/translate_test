using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace translate_test
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder text;
        Timer t;
        int count = 0, timer_interval = 50;
        private List<string> Languages;
        public List<string> Strings;
        public bool GroupByLanguage = false;
        [DefaultValue("")]
        public string Prefix, StringDelimiter, GroupDelimiter, Suffix;
        
        [DefaultValue("C:\\strings.txt")]
        public string OutputFilepath;

       public Form1(string p)
        {
            t = new Timer();
            t.Interval = timer_interval;
            t.Tick += t_Tick;
            text = new StringBuilder();
            Prefix = p;
            text.Append(p);
            Languages = new List<string>();
            Strings = new List<string>();
            LoadLanguageDictionary();
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            t.Interval = timer_interval;
            t.Enabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            foreach (HtmlElement h in webBrowser1.Document.All)
            {
                if (h.Id != null && h.Id.Equals("result_box"))
                {
                    if (h.InnerText != null)
                    {
                        if (h.InnerText.Contains("..."))
                        {
                            ((Timer)sender).Enabled = false;
                            timer_interval += 50;
                            if (timer_interval > 250)
                                webBrowser1.Url = getBackupURL();
                            else
                                webBrowser1.Url = getURL();
                            // return;
                        }
                        else
                        {
                            text.Append(Format(h.InnerText));
                            ((Timer)sender).Enabled = false;
                            timer_interval = 50;
                            LoadNextString();
                        }
                    }
                    break;
                }
            }
        }

        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {

        }

        private void LoadNextString()
        {
            List<string> OuterList, InnerList;

            if(GroupByLanguage)
            {
                OuterList = Strings;
                InnerList = Languages;
            }
            else
            {
                OuterList = Languages;
                InnerList = Strings;
            }
            ++count;
            if (count % OuterList.Count == 0)
            {
                if (count / OuterList.Count == InnerList.Count)
                {
                    text.Append(Suffix);
                    using(System.IO.StreamWriter wr = new System.IO.StreamWriter(OutputFilepath))
                    {
                        wr.Write(text);
                    }
                    this.Close();
                    return;
                }
                else
                    text.Append(GroupDelimiter);
            }
            else text.Append(StringDelimiter);
            OuterList = InnerList = null;
            webBrowser1.Navigate(getURL());

        }

        private Uri getURL()
        {
            if (GroupByLanguage)
                return new Uri("https://translate.google.com/#en/" + Languages[count / Strings.Count] + "/" + Strings[count % Strings.Count].Replace(" ", "%20").ToLower(), System.UriKind.Absolute);
            else
                return new Uri("https://translate.google.com/#en/" + Languages[count % Languages.Count] + "/" + Strings[count / Languages.Count].Replace(" ", "%20").ToLower(), System.UriKind.Absolute);
        }

        private Uri getBackupURL()
        {
            if (GroupByLanguage)
                return new Uri("https://translate.google.com/#en/" + Languages[count / Strings.Count] + "/" + Strings[count % Strings.Count].Replace(" ", "%20").ToLower() + "%20", System.UriKind.Absolute);
            else
                return new Uri("https://translate.google.com/#en/" + Languages[count % Languages.Count] + "/" + Strings[count / Languages.Count].Replace(" ", "%20").ToLower() + "%20", System.UriKind.Absolute);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }

        private string Format(string s)
        {
         
            if(string.IsNullOrEmpty(s)) return s;
            StringBuilder t = new StringBuilder(s.Length);
           // int last_space = 0;
            t.Append( ToUpper(s[0]));
            for(int c = 1; c < s.Length; c++)
            {
                int i = s.IndexOf(' ', c) - c;
                if((s[c-1] == ' ') && ((i > 3) || ((i < 0) && (c + 3 < s.Length))))
                    t.Append( ToUpper(s[c]));
                else t.Append( s[c]);
            }

            return t.ToString();
        }

        private char ToUpper(char c)
        {
            return ((c >= 'a') && (c <= 'z') ? (char)(c & 0xDF) : c);
        }

        public void AddLanguage(string l)
        {
            string g;
            if(dict.TryGetValue(l, out g))
            {
                Languages.Add(g);
            }
        }

        private void LoadLanguageDictionary()
        {
            dict.Add("English", "en");
            dict.Add("Afrikaans", "af");
            dict.Add("Albanian", "sq");
            dict.Add("Arabic", "ar");
            dict.Add("Armenian", "hy");
            dict.Add("Azerbaijani", "az");
            dict.Add("Basque", "eu");
            dict.Add("Belarusian", "be");
            dict.Add("Bengali", "bn");
            dict.Add("Bosnian", "bs");
            dict.Add("Bulgarian", "bg");
            dict.Add("Catalan", "ca");
            dict.Add("Cebuano", "ceb");
            dict.Add("Chinese (Simplified)", "zh-CN");
            dict.Add("Chinese (Traditional)", "zh-TW");
            dict.Add("Croatian", "hr");
            dict.Add("Czech", "cs");
            dict.Add("Danish", "da");
            dict.Add("Dutch", "nl");
            dict.Add("Esperanto", "eo");
            dict.Add("Estonian", "et");
            dict.Add("Filipino", "tl");
            dict.Add("Finnish", "fi");
            dict.Add("French", "fr");
            dict.Add("Galician", "gl");
            dict.Add("Georgian", "ka");
            dict.Add("German", "de");
            dict.Add("Greek", "el");
            dict.Add("Gujarati", "gu");
            dict.Add("Haitian Creole", "ht");
            dict.Add("Hausa", "ha");
            dict.Add("Hebrew", "iw");
            dict.Add("Hindi", "hi");
            dict.Add("Hmong", "hmn");
            dict.Add("Hungarian", "hu");
            dict.Add("Icelandic", "is");
            dict.Add("Igbo", "ig");
            dict.Add("Indonesian", "id");
            dict.Add("Irish", "ga");
            dict.Add("Italian", "it");
            dict.Add("Japanese", "ja");
            dict.Add("Javanese", "jw");
            dict.Add("Kannada", "kn");
            dict.Add("Khmer", "km");
            dict.Add("Korean", "ko");
            dict.Add("Lao", "lo");
            dict.Add("Latin", "la");
            dict.Add("Latvia", "lv");
            dict.Add("Lithuanian", "lt");
            dict.Add("Macedonian", "mk");
            dict.Add("Malay", "ms");
            dict.Add("Maltese", "mt");
            dict.Add("Maori", "mi");
            dict.Add("Marathi", "mr");
            dict.Add("Mongolian", "mn");
            dict.Add("Nepali", "ne");
            dict.Add("Norwegian", "no");
            dict.Add("Persian", "fa");
            dict.Add("Polish", "pl");
            dict.Add("Portuguese", "pt");
            dict.Add("Punjabi", "pa");
            dict.Add("Romanian", "ro");
            dict.Add("Russian", "ru");
            dict.Add("Serbian", "sr");
            dict.Add("Slovak", "sk");
            dict.Add("Slovenian", "sl");
            dict.Add("Somali", "so");
            dict.Add("Spanish", "es");
            dict.Add("Swahili", "sw");
            dict.Add("Swedish", "sv");
            dict.Add("Tamil", "ta");
            dict.Add("Telugu", "te");
            dict.Add("Thai", "th");
            dict.Add("Turkish", "tr");
            dict.Add("Ukrainian", "uk");
            dict.Add("Urdu", "ur");
            dict.Add("Vietnamese", "vi");
            dict.Add("Welsh", "cy");
            dict.Add("Yiddish", "yi");
            dict.Add("Yoruba", "yo");
            dict.Add("Zulu", "zu");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnShown(EventArgs e)
        {
            webBrowser1.Url = getURL();
            base.OnShown(e);
        }
    }
}

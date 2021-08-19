using System.Text;

namespace LogViewer
{
    internal class RTFGenerator
    {
        StringBuilder sb;
        public RTFGenerator()
        {
            sb = new StringBuilder(@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}" +
            @"{\colortbl;\red30\green122\blue212;\red255\green243\blue128;\red134\green169\blue90;\red176\green87\blue44;\red220\green220\blue170;}" +
            @"{\*\generator Riched20 10.0.19041}\viewkind4\uc1\pard\cf1\f0\fs17");
        }

        string _color = @"\cf1";

        internal void add(string text, bool[] show)
        {
            string header = "";

            int ini = text.IndexOf('\t');
            int end = text.IndexOf('\t', text.IndexOf('\t') + 1);
            if (ini > 0 && end > 0)
            {
                header = text.Substring(ini + 1, end - ini - 1);
            }
            else if (text.Length > 50)
            {
                header = text.Substring(0, 50);
            }
            
            if (header != "") 
            header = header.ToUpper();
            if (header.Contains("INFO")) if (show[0]) _color = @"\cf1"; else return;
            else if (header.Contains("WARNING")) if (show[1]) _color = @"\cf2"; else return;
            else if (header.Contains("MESSAGE")) if (show[2]) _color = @"\cf3"; else return;
            else if (header.Contains("ERROR")) if (show[3]) _color = @"\cf4"; else return;
            else if (header.Contains("DEBUG")) if (show[4]) _color = @"\cf5"; else return;

            sb.Append(_color + " " + text.Replace(@"\", @"\\") + @" \par");
        }

        internal string get()
        {
            sb.Append(@"\cf0\par}");
            return sb.ToString();
        }
    }
}
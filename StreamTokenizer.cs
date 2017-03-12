using System;
using System.IO;

namespace Tokenizer
{
    public class StreamTokenizer
    {
        StreamReader sr;
        char[] delimiters;  // chars which bound tokens
        char[] punct;   // list of punctuation which are valid tokens

        string[] buf;     // split string we're working on
        int pos;      // index of next item in buf
        int linenum;      // which line number we're processing

        string unget;     // one-slot unget buffer

        public StreamTokenizer(StreamReader sr,
            char[] delimiters,
            char[] punct)
        {
            this.sr = sr;
            this.punct = punct;
            this.delimiters = delimiters;
            this.buf = null;
            this.pos = 0;
            this.linenum = 0;
            this.unget = null;
        }

        public int Linenum { get { return linenum; } }

        // Returns the next token in the stream, or NULL on EOF.
        // Tokens are bounded by the chars in "delimiters", or those
        // in "punct".  The difference is that "punct" chars are also
        // tokens, and returned as such, whereas "delimiters" are never
        // returned.
        public string NextToken()
        {
            string s;

            if (unget != null)
            {
                s = unget;
                unget = null;
                return s;
            }

            while (true)
            {
                // refill our buffer if needed
                if (buf == null || pos >= buf.Length)
                {
                    s = sr.ReadLine();
                    if (s == null)
                        return null; // EOF
                    linenum++;
                    buf = s.Split(delimiters);
                    pos = 0;
                }

                // skip empty strings (results of multiple delimiters in a row)
                if (buf[pos] == "")
                {
                    pos++;
                    continue;
                }

                s = buf[pos];

                // do we need to further split this string on punctuation?
                int p = s.IndexOfAny(punct);
                if (p < 0) // no punctuation present
                {
                    pos++;
                    return s;
                }

                // punctuation - so split it off
                if (p == 0) // right at start
                {
                    buf[pos] = s.Substring(1);
                    return s.Substring(0, 1);
                }

                // otherwise the punctuation is in middle or at end
                buf[pos] = s.Substring(p);
                return s.Substring(0, p);
            }
        }

        public bool Unget(string s)
        {
            if (unget != null)
                return false;
            unget = s;
            return true;
        }

        // junk the rest of the line (presumably it's a comment)
        public void SkipToEOL()
        {
            if (buf != null)
                pos = buf.Length;
        }
    }
}

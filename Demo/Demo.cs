using System;
using System.IO;

using Tokenizer;

namespace Demo
{
    // Demo:
    //
    // Build an in-memory stream, and setup a StreamTokenizer on it,
    // splitting on whitespace, and considering '#' and '/' characters as tokens.
    // Print the tokens we get back, and skip comments introduced by '#'
    // or "//".  To parse "//" (2-token sequence) we look-ahead one token,
    // and push it back if it isn't '/'.
    //
    // More normal use of StreamTokenizer would be to parse some
    // simple config file, where the StreamReader would be created directly from
    // the filename.
    class DemoClass
    {
	[STAThread]
	static void Main(string[] args)
	{
	    // DEMO BOILERPLATE:
	    // create a test stream, with some sample text in it
	    MemoryStream ms = new MemoryStream();
	    StreamWriter sw = new StreamWriter(ms);

	    sw.WriteLine("This is a       sample stream");
	    sw.WriteLine("with\tsome # this is a comment");
	    sw.WriteLine("   (example) text // this is another comment");
	    sw.WriteLine("and several /comments");
	    sw.Flush();
	    ms.Seek(0, SeekOrigin.Begin);

	    // and open a reader on it:
	    StreamReader sr = new StreamReader(ms);
	    // could also just open a file:
	    //StreamReader sr = new StreamReader("inputfile.txt");

	    // DEMO START:
	    StreamTokenizer tok = new StreamTokenizer(sr,
		null, // whitespace delimiters
		new char [] {'#', '/'}); // comment start chars are tokens too

	    string s;
	    while ((s = tok.NextToken()) != null)
	    {
		Console.WriteLine("line {0} token: '{1}'", tok.Linenum, s);
		if (s == "#")
		{
		    Console.WriteLine("  (# comment, skipping to end of line)");
		    tok.SkipToEOL();
		}
		if (s == "/")
		{
		    // peek at the next token to see if it is our
		    // second slash:
		    s = tok.NextToken();
		    if (s == null)
			break;
		    if (s == "/")
		    {
			Console.WriteLine("  (// comment, skipping)");
			tok.SkipToEOL();
		    }
		    else
		    {
			// no, so push the token back into the tokenizer
			tok.Unget(s);
		    }
		}
	    }

	    Console.WriteLine("[Hit return to exit]");
	    Console.ReadLine();
	}
    }
}

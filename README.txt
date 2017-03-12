Stream Tokenizer
----------------

Summary: A C# class which you wrap around a StreamReader to tokenize
    the stream and hand it back token by token.  Useful for parsing
    simple config files.


Detailed description:

StreamTokenizer provides "cooked" access to a StreamReader, making it
easy to write simple parsers (eg recursive descent, single token
lookahead).  It can be parameterised to split a text stream into
tokens delimited by whitespace or any other set of characters, and to
return a set of characters as tokens.  It remembers the current line
number, making useful error messages easy to print.  It supports
pushing a single token back into the token stream to allow one-token
lookahead.


Usage:

Copy the following file into your C# project:
  StreamTokenizer.cs
You may wish to adjust the namespace it places itself into (set to
"Tokenizer" in this distribution).

Instantiate a StreamTokenizer as follows:

 StreamTokenizer tok = new StreamTokenizer(mystreamreader,
		null, // delimiters (null==whitespace)
		new char [] {'#', '/'}); // punctuation

Tokens are bounded by characters which appear in either the
"delimiters" or "punctuation" character arrays.  The difference is
that "delimiters" are swallowed and are never part of tokens, whereas
"punctuation" characters have two roles: they are both delimiters and
tokens (single character ones) and thus will be returned.

 tok.NextToken()
Method: returns the next token available, according to the rules given
above.  It returns null when the underlying StreamReader returns EOF.

 tok.Linenum
Property: returns the current line number, for use in error messages.

 tok.Unget(string s)
Method: push s (a token) back into the StreamTokenizer; it will be
returned on the next call to NextToken().  If called multiple times
before NextToken(), Unget() returns false and has no effect, otherwise
it succeeds and returns true.

 tok.SkipToEOL()
Method: move the parse point to the end of the current line, ignoring
all tokens between the current location and the end of line.



Mini-example:

To parse well-bracketed strings such as "(() ())", you would construct
a StreamTokenizer with delimiters == null (whitespace), and
punctuation == ['(', ')'].  You would then write code similar to the
following:

-----------------------------------
int paren_count = 0;
while ((s = tok.NextToken()) != null)
{
  if (s == "(")
  {
    paren_count++;
  }
  else if (s == ")")
  {
    if (paren_count == 0)
      throw new UnbalancedParensException();
    paren_count--;
  }
  else
  {
    throw new ParseErrorException("Unknown token", s);
  }
}
if (paren_count != 0)
  throw new UnbalancedParensException();
-----------------------------------




Sample code:

See Demo/ directory for a sample Visual Studio project which uses
the StreamTokenizer.cs to parse a short stream with two styles
of comments.

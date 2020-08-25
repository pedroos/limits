using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.NumberSystem
{
    // The simplest numeral system is the unary numeral system, in which every natural number is represented by a 
    // corresponding number of symbols.

    // The unary notation can be abbreviated by introducing different symbols for certain new values. Very commonly, 
    // these values are powers of 10.

    // More useful still are systems which employ special abbreviations for repetitions of symbols; for example, using 
    // the first nine letters of the alphabet for these abbreviations, with A standing for "one occurrence", B "two 
    // occurrences", and so on, one could then write C+ D/ for the number 304.

    // More elegant is a positional system, also known as place-value notation. Again working in base 10, ten different 
    // digits 0, ..., 9 are used and the position of a digit is used to signify the power of ten that the digit is to be 
    // multiplied with, as in 304 = 3×100 + 0×10 + 4×1 or more precisely 3×102 + 0×101 + 4×100. Zero, which is not 
    // needed in the other systems, is of crucial importance here, in order to be able to "skip" a power.

    // Arithmetic is much easier in positional systems than in the earlier additive ones; furthermore, additive systems 
    // need a large number of different symbols for the different powers of 10; a positional system needs only ten 
    // different symbols (assuming that it uses base 10).

    // In computers, the main numeral systems are based on the positional system in base 2 (binary numeral system), with 
    // two binary digits, 0 and 1. Positional systems obtained by grouping binary digits by three (octal numeral system) 
    // or four (hexadecimal numeral system) are commonly used. For very large integers, bases 232 or 264 (grouping 
    // binary digits by 32 or 64, the length of the machine word) are used, as, for example, in GMP.

    // The numerals used when writing numbers with digits or symbols can be divided into two types that might be called 
    // the arithmetic numerals (0, 1, 2, 3, 4, 5, 6, 7, 8, 9) and the geometric numerals (1, 10, 100, 1000, 10000 ...), 
    // respectively. The sign-value systems use only the geometric numerals and the positional systems use only the 
    // arithmetic numerals. A sign-value system does not need arithmetic numerals because they are made by repetition 
    // (except for the Ionic system), and a positional system does not need geometric numerals because they are made by 
    // position. However, the spoken language uses both arithmetic and geometric numerals.

    // In certain areas of computer science, a modified base k positional system is used, called bijective numeration, 
    // with digits 1, 2, ..., k (k ≥ 1), and zero being represented by an empty string. This establishes a bijection 
    // between the set of all such digit-strings and the set of non-negative integers, avoiding the non-uniqueness 
    // caused by leading zeros.

    public class Number
    {
        public int Base { get; }
        public int Decimal { get; }
    }


}

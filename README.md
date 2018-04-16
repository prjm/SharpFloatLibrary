SharpFloatLibrary
---

This small library provides an 80-bit floating point precision type for C#. Included are

* basic arithmetic operations
* operators to cast from / and to other numeric types
* a string parser and a floating point printing routine

This couldn't have been implemented without the great work of other libraries, nameley:

* [the berkeley softfloat library](https://github.com/ucb-bar/berkeley-softfloat-3/) - implementation of arithmetic operations
* [the dragon4 algorithm](http://www.ryanjuckett.com/programming/printing-floating-point-numbers/) - to print floating point numbers
* [the roslyn project](https://github.com/dotnet/roslyn) - to parse floating point numbers

Please note the license details in the file `LICENSE.txt`.

The project source code includes:
* the class library for the `ExtF80` struct and associated helper types
* some unit tests
* a test runner which compares the output of this library to the output of the [testfloat](https://github.com/ucb-bar/berkeley-testfloat-3) utility

Please note that this project is a hobby project - there might be bugs, incompleteness or irregularities.

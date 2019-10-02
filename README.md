# TelephoneKeyPad
This is a simple MVC 5 web application to demostrate how to generate all the alphanumeric combinations of a given phone number.

## Background

[E.161](https://en.wikipedia.org/wiki/E.161) defines the assignment of basic 26 Latin letters (A to Z) to the 12-key telephone keypad. An example telephone keypad:

![Keypad](https://upload.wikimedia.org/wikipedia/commons/thumb/4/43/Telephone-keypad.svg/220px-Telephone-keypad.svg.png)

## Requirements

1. The phone number must be a 7 or 10-digit number (Validation).
2. The total number of combination must be calculated before generation (No nested loops/ brutal forcing). 
3. Paged results. 
4. Unit tests.
5. Develop on .Net Framework 4.5.x.

## Example

If the input number is 222 555 6106 then the output is

Total number of combinations: <count goes here>

222 555 610M
222 555 610N
222 555 610O



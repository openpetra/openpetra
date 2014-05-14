https://code.google.com/p/password-utilities

A C# library that handles:

Variable-length password generation using a cryptographically-secure pseudo-random number generator.
Variable-length password salting using a cryptographically-secure pseudo-random number generator.
Password hash generation from a range of hash algorithms, although currently only SHA1-160 and SHA2-256 (both with HMAC and PKDBF2) are implemented.
Password hash strengthening based on a specified work factor (number of hash iterations).
Password verification against a previously-stored password/salt hash.
Password policies that specify password min/max length, allowed character sets, and a minimum number of characters from each set.
Hashing policies that specify the hash algorithm, storage format, work factor, and number of salt bytes.

License: MIT (GPL v3 compatible)

Current version:
Modified version based on version 1.3.1

PasswordPolicy.cs is modified to customise code for OpenPetra.
Various constants are changed and code is added to PasswordSatisfiesPolicy(string password).
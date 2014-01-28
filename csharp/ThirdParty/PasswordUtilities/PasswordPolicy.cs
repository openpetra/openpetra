// Copyright (c) 2013 Mark Pearce
// http://opensource.org/licenses/MIT

using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace PasswordUtilities
{
/// <summary>
/// Policy used to control generation and validation of passwords.
/// </summary>
public sealed class PasswordPolicy
{
    // Minimum and maximum password lengths allowed.
    private const Int32 PASSWORD_LENGTH_MINIMUM = 0;
    private const Int32 PASSWORD_LENGTH_MAXIMUM = 200;

    // Values for default policy.
    private const Int32 PASSWORD_DEFAULT_LENGTH_MINIMUM = 11;
    private const Int32 PASSWORD_DEFAULT_LENGTH_MAXIMUM = 12;

    // These are the character sets used by the default password policy.
    // You can of course define and use your own character sets.
    private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMERIC = "0123456789";
    private const string PUNCTUATION = @"!;:,.'?""";
    private const string BRACKETS = "()[]{}<>";
    private const string ASCII_OTHER = @"*$£-+_&=%/\^~#@";

    // Default maximum number of password rejections allowed.
    private const Int32 DEFAULT_MAXIMUM_REJECTIONS = 1000;

    private string m_AllowedSymbols = String.Empty;
    private Dictionary <String, CharacterSet>m_AllowedCharacterSets = new Dictionary <String, CharacterSet>();

    /// <summary>
    /// Constructor for creating the default password policy.
    /// </summary>
    public PasswordPolicy()
    {
        this.SetupPolicy(PASSWORD_DEFAULT_LENGTH_MINIMUM, PASSWORD_DEFAULT_LENGTH_MAXIMUM, null);
    }

    /// <summary>
    /// Constructor for creating a specific password policy.
    /// </summary>
    /// <param name="lengthMinimum">
    /// Minimum length of password must be at least 0.
    /// </param>
    /// <param name="lengthMaximum">
    /// Maximum length of password must be no more than 200.
    /// </param>
    public PasswordPolicy(Int32 lengthMinimum, Int32 lengthMaximum)
    {
        this.SetupPolicy(lengthMinimum, lengthMaximum, null);
    }

    /// <summary>
    /// Constructor for creating a specific password policy.
    /// </summary>
    /// <param name="lengthMinimum">
    /// Minimum length of password must be at least 4.
    /// </param>
    /// <param name="lengthMaximum">
    /// Maximum length of password must be no more than 200.
    /// </param>
    /// <param name="characterSets">
    /// Character sets that are allowed for password generation.
    /// </param>
    public PasswordPolicy(Int32 lengthMinimum, Int32 lengthMaximum, IEnumerable <CharacterSet>characterSets)
    {
        this.SetupPolicy(lengthMinimum, lengthMaximum, characterSets);
    }

    // Called from every constructor to setup and validate the password policy.
    private void SetupPolicy(Int32 lengthMinimum, Int32 lengthMaximum, IEnumerable <CharacterSet>characterSets)
    {
        this.LengthMinimum = lengthMinimum;
        this.LengthMaximum = lengthMaximum;
        this.MaximumPasswordRejections = DEFAULT_MAXIMUM_REJECTIONS;

        if (characterSets == null)
        {
            // These are the default character sets.
            this.CharacterSetAdd(new CharacterSet("AL", "ASCII lowercase", LOWERCASE));
            this.CharacterSetAdd(new CharacterSet("AU", "ASCII uppercase", UPPERCASE));
            this.CharacterSetAdd(new CharacterSet("AN", "ASCII numeric", NUMERIC));
            this.CharacterSetAdd(new CharacterSet("AP", "ASCII punctuation", PUNCTUATION));
            this.CharacterSetAdd(new CharacterSet("AB", "ASCII brackets", BRACKETS));
            this.CharacterSetAdd(new CharacterSet("AO", "ASCII other", ASCII_OTHER));
        }
        else
        {
            foreach (CharacterSet characterSet in characterSets)
            {
                this.CharacterSetAdd(characterSet);
            }
        }

        this.ValidatePolicy();
    }

    /// <summary>
    /// The absolute minimum length of password.
    /// </summary>
    public static Int32 AbsoluteLengthMinimum
    {
        get
        {
            return PASSWORD_LENGTH_MINIMUM;
        }
    }

    /// <summary>
    /// The absolute maximum length of password.
    /// </summary>
    public static Int32 AbsoluteLengthMaximum
    {
        get
        {
            return PASSWORD_LENGTH_MAXIMUM;
        }
    }

    /// <summary>
    /// Minimum length of current password as a Unicode string.
    /// </summary>
    public Int32 LengthMinimum {
        get; set;
    }

    /// <summary>
    /// Maximum length of current password as a Unicode string.
    /// </summary>
    public Int32 LengthMaximum {
        get; set;
    }

    /// <summary>
    /// Maximum number of passwords that can be rejected.
    /// </summary>
    /// <remarks>
    /// This prevents password generation continuing for too long.
    /// If this number of passwords are rejected during generation (because none of
    /// the passwords match the current policy), the last generated password will
    /// be returned.
    /// </remarks>
    public Int32 MaximumPasswordRejections {
        get; set;
    }

    /// <summary>
    /// Returns an enumeration of the allowed character sets.
    /// </summary>
    public IEnumerable <CharacterSet>AllowedCharacterSets
    {
        get
        {
            return m_AllowedCharacterSets.Values;
        }
    }

    /// <summary>
    /// Number of allowed character sets.
    /// </summary>
    public Int32 Count
    {
        get
        {
            return m_AllowedCharacterSets.Count;
        }
    }

    /// <summary>
    /// Adds the specified character set to the list of allowed character sets.
    /// </summary>
    public void CharacterSetAdd(CharacterSet allowedCharacterSet)
    {
        if (allowedCharacterSet == null)
        {
            throw new ArgumentNullException("allowedCharacterSet", String.Format(CultureInfo.InvariantCulture, "Character set must not be null"));
        }
        else if (m_AllowedCharacterSets.ContainsKey(allowedCharacterSet.Key))
        {
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                    "The specified character set already exists"), "allowedCharacterSet");
        }
        else
        {
            m_AllowedCharacterSets.Add(allowedCharacterSet.Key, allowedCharacterSet);
            m_AllowedSymbols = this.FindAllowedSymbols();
        }
    }

    /// <summary>
    /// Removes the specified character set from the list of allowed character sets.
    /// </summary>
    public void CharacterSetRemove(string key)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("key", String.Format(CultureInfo.InvariantCulture, "Key cannot be null or empty"));
        }

        if (m_AllowedCharacterSets.ContainsKey(key))
        {
            m_AllowedCharacterSets.Remove(key);
            m_AllowedSymbols = this.FindAllowedSymbols();
        }
        else
        {
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The specified character set doesn't exist"), "key");
        }
    }

    /// <summary>
    /// Reset the minimum number of characters for the specified character set.
    /// </summary>
    public void CharacterSetMinimumCharacters(string key, Int32 minimumCharacters)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("key", String.Format(CultureInfo.InvariantCulture, "Key cannot be null or empty"));
        }

        if (minimumCharacters < 0)
        {
            throw new ArgumentOutOfRangeException("minimumCharacters",
                String.Format(CultureInfo.InvariantCulture, "Minimum number of characters cannot be negative"));
        }

        if (m_AllowedCharacterSets.ContainsKey(key))
        {
            m_AllowedCharacterSets[key].MinimumNumberOfCharacters = minimumCharacters;
        }
        else
        {
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The specified character set doesn't exist"), "key");
        }
    }

    /// <summary>
    /// A byte array containing the symbols that can
    /// form a password that matches this policy.
    /// </summary>
    public byte[] AllowedSymbolBytes()
    {
        return Encoding.UTF8.GetBytes(this.AllowedSymbols);
    }

    /// <summary>
    /// A Unicode string containing the symbols that can
    /// form a password that matches this policy.
    /// </summary>
    public string AllowedSymbols
    {
        get
        {
            return m_AllowedSymbols;
        }
    }

    /// <summary>
    /// The minimum number of mandatory symbols needed
    /// to form a password that matches this policy.
    /// </summary>
    public Int32 MinimumNumberOfSymbols
    {
        get
        {
            Int32 symbolCount = 0;

            foreach (CharacterSet characterSet in m_AllowedCharacterSets.Values)
            {
                symbolCount += characterSet.MinimumNumberOfCharacters;
            }

            return symbolCount;
        }
    }

    /// <summary>
    /// Tests whether a password matches this policy.
    /// </summary>
    /// <param name="password">
    /// The password as a byte array.
    /// </param>
    /// <returns>
    /// True if the password matches this policy, otherwise false.
    /// </returns>
    public bool PasswordSatisfiesPolicy(byte[] password)
    {
        return this.PasswordSatisfiesPolicy(Encoding.UTF8.GetString(password));
    }

    /// <summary>
    /// Tests whether a password matches this policy.
    /// </summary>
    /// <param name="password">
    /// The password as a Unicode UTF-8 string.
    /// </param>
    /// <returns>
    /// True if the password matches this policy, otherwise false.
    /// </returns>
    public bool PasswordSatisfiesPolicy(string password)
    {
        ValidatePassword(password);

        // Every password symbol must be in the complete list of allowed symbols.
        for (Int32 i = 0; i < password.Length; i++)
        {
            if (!this.AllowedSymbols.Contains(password[i].ToString()))
            {
                return false;
            }
        }

        // Check that each character set is represented appropriately.
        Int32 symbolCount;

        foreach (CharacterSet characterSet in m_AllowedCharacterSets.Values)
        {
            if (characterSet.MinimumNumberOfCharacters > 0)
            {
                // Count the number of password symbols belonging to this character set.
                symbolCount = 0;

                for (Int32 i = 0; i < password.Length; i++)
                {
                    if (characterSet.Characters.Contains(password[i].ToString()))
                    {
                        symbolCount++;

                        if (symbolCount >= characterSet.MinimumNumberOfCharacters)
                        {
                            break;
                        }
                    }
                }

                // Does the password have the minimum number of symbols required for this character set?
                if (symbolCount < characterSet.MinimumNumberOfCharacters)
                {
                    return false;
                }
            }
        }

        int AlphabetCount = 0;
        int NumberCount = 0;

        for (Int32 i = 0; i < password.Length; i++)
        {
            if (LOWERCASE.Contains(password[i].ToString()))
            {
                AlphabetCount++;
            }
            else if (UPPERCASE.Contains(password[i].ToString()))
            {
                AlphabetCount++;
            }
            else if (NUMERIC.Contains(password[i].ToString()))
            {
                NumberCount++;
            }
        }

        if ((AlphabetCount < 7) || (AlphabetCount > 8))
        {
            return false;
        }

        if ((NumberCount < 2) || (NumberCount > 3))
        {
            return false;
        }

        return true;
    }

    // Validates this policy.
    private void ValidatePolicy()
    {
        // Password security is increased by a longer length.
        if (this.LengthMinimum < PASSWORD_LENGTH_MINIMUM)
        {
            throw new ArgumentOutOfRangeException("lengthMinimum",
                String.Format(CultureInfo.InvariantCulture, "Password must be at least {0} characters: not {1}", PASSWORD_LENGTH_MINIMUM,
                    this.LengthMinimum));
        }

        // We can't have an endless password.
        if (this.LengthMaximum > PASSWORD_LENGTH_MAXIMUM)
        {
            throw new ArgumentOutOfRangeException("lengthMaximum",
                String.Format(CultureInfo.InvariantCulture, "Password must be no longer than {0} characters: not {1}", PASSWORD_LENGTH_MAXIMUM,
                    this.LengthMaximum));
        }
    }

    // Validate parameters of public methods.
    private static void ValidatePasswordLength(Int32 passwordLength)
    {
        if (passwordLength < PASSWORD_LENGTH_MINIMUM)
        {
            throw new ArgumentOutOfRangeException("passwordLength",
                String.Format(CultureInfo.InvariantCulture, "Password length must be greater than or equal to {0}, not {1}", PASSWORD_LENGTH_MINIMUM,
                    passwordLength));
        }
    }

    // Validate parameters of public methods.
    private static void ValidatePassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException("password", "Password cannot be null");
        }

        if (password.Length < PASSWORD_LENGTH_MINIMUM)
        {
            throw new ArgumentOutOfRangeException("password",
                String.Format(CultureInfo.InvariantCulture, "Password length must be greater than or equal to {0}, not {1}", PASSWORD_LENGTH_MINIMUM,
                    password.Length));
        }
    }

    // Extract all distinct symbols allowed by this password policy.
    private string FindAllowedSymbols()
    {
        string acceptableSymbols = String.Empty;

        foreach (CharacterSet characterSet in m_AllowedCharacterSets.Values)
        {
            acceptableSymbols += characterSet.Characters;
        }

        // Remove duplicate symbols using LINQ - relatively slow, but short and easy to understand.
        var duplicates = acceptableSymbols.Where(ch => acceptableSymbols.Count(c => c == ch) > 1);
        return new string(acceptableSymbols.Except(duplicates).ToArray());
    }

    /// <summary>
    /// Overrides the default ToString().
    /// </summary>
    /// <returns>
    /// The symbols acceptable by this policy as a Unicode string.
    /// </returns>
    public override string ToString()
    {
        return String.Format(CultureInfo.InvariantCulture, "PasswordPolicy: {0}", this.AllowedSymbols);
    }
}
}
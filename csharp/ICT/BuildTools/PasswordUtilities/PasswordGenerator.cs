// Copyright (c) 2013 Mark Pearce
// http://opensource.org/licenses/MIT

using System;
using System.Globalization;
using System.Diagnostics;
using System.Text;

namespace PasswordUtilities
{
/// <summary>
/// This class uses a password policy to generate random Unicode passwords
/// which do not include ambiguous characters such as I, l, O, 0, and 1.
/// </summary>
/// <remarks>
/// Each generated password is a Unicode string composed of
/// symbols taken from the symbol types specified in the policy.
/// A cryptographically-secure PNRG does the password generation.
/// You can provide a password policy (or use the default policy)
/// to dictate the password length and the permitted symbol types.
/// </remarks>
public sealed class PasswordGenerator
{
    /// <summary>
    /// Use the default password policy.
    /// </summary>
    /// <remarks>
    /// If you use the default password policy with the default hash policy,
    /// you will have a combined entropy of around 83 bits - greater than
    /// NIST's recommendation of 80 bits for the most secure passwords.
    /// </remarks>
    public PasswordGenerator()
    {
        this.Policy = new PasswordPolicy();
    }

    /// <summary>
    /// Use the specified password policy.
    /// </summary>
    public PasswordGenerator(PasswordPolicy passwordPolicy)
    {
        if (passwordPolicy == null)
        {
            throw new ArgumentNullException("passwordPolicy", String.Format(CultureInfo.InvariantCulture, "Password policy cannot be null"));
        }
        else
        {
            this.Policy = passwordPolicy;
        }
    }

    /// <summary>
    /// Generated password as a Unicode string.
    /// </summary>
    public string Password
    {
        get
        {
            return Encoding.UTF8.GetString(this.PasswordBytes);
        }
    }

    /// <summary>
    /// Generated password as a byte array.
    /// </summary>
    public byte[] PasswordBytes {
        get; private set;
    }

    /// <summary>
    /// Current password policy.
    /// </summary>
    public PasswordPolicy Policy {
        get; set;
    }

    /// <summary>
    /// Time taken to generate the password.
    /// </summary>
    public TimeSpan GenerationTime {
        get; private set;
    }

    /// <summary>
    /// Generated password entropy in bits.
    /// </summary>
    /// <remarks>
    /// For a machine-generated random password, this is equivalent to the password
    /// strength. For more details, see http://en.wikipedia.org/wiki/Password_strength
    /// In 1998, the EFF cracked a 56-bit key in 56 hours using specialised FPGA hardware.
    /// In 2002, distributed.net cracked a 64-bit key in 4 years, 9 months, and 23 days.
    /// In August 2010, distributed.net estimated that cracking a 72-bit key using
    /// current hardware would take about 48,712 days or 133.5 years.
    /// NIST recommends 80 bits for the most secure passwords up until 2014.
    /// </remarks>
    public double PasswordEntropy
    {
        get
        {
            EntropyChecker passwordEntropy = new EntropyChecker();
            return passwordEntropy.MachinePasswordPolicyKnown(this.Policy, this.Password.Length);
        }
    }

    /// <summary>
    /// Number of random passwords generated before
    /// password matching specified policy is found.
    /// </summary>
    public Int32 PasswordRejectedCount {
        get; private set;
    }

    /// <summary>
    /// Generates password using the current password policy.
    /// </summary>
    /// <returns>
    /// Password as a Unicode (UTF-8) string.
    /// </returns>
    public string GeneratePassword()
    {
        Stopwatch timer = TimerStart();
        string allowedSymbols = this.Policy.AllowedSymbols;

        // Init password generation.
        Int32 passwordLength; Int32 randomIndex;
        Int32 countPasswordsRejected = -1; string password;

        using (var die = new Die())
        {
            // Generate the password using a dice roll to index into the acceptable symbols list.
            // Throw away any password that doesn't match the specified password policy.
            do
            {
                // Choose password length and optimise policy for this length.
                passwordLength = ChoosePasswordLength(this.Policy);
                //passwordPolicy.ImprovePerformance(passwordLength);
                die.NumberOfSides = allowedSymbols.Length;
                password = String.Empty;

                // Generate each symbol in the password.
                for (Int32 i = 0; i < passwordLength; i++)
                {
                    randomIndex = die.Roll() - 1;
                    password += allowedSymbols[randomIndex];
                }

                countPasswordsRejected++;
            }
            // New password has to match the policy.
            while ((this.Policy.PasswordSatisfiesPolicy(password) == false) && (countPasswordsRejected < this.Policy.MaximumPasswordRejections));
        }

        // We're done.
        this.GenerationTime = TimerStop(timer);
        this.PasswordRejectedCount = countPasswordsRejected;
        this.PasswordBytes = Encoding.UTF8.GetBytes(password);
        return this.Password;
    }

    // Choose a random password length between the specified minimum and maximum.
    private static Int32 ChoosePasswordLength(PasswordPolicy passwordPolicy)
    {
        if (passwordPolicy.LengthMinimum == passwordPolicy.LengthMaximum)
        {
            return passwordPolicy.LengthMinimum;
        }
        else
        {
            // Example: max 10 and min 6 = a dice roll between 1 and 5.
            Int32 dieNumberOfSides = (passwordPolicy.LengthMaximum - passwordPolicy.LengthMinimum + 1);
            using (var die = new Die(dieNumberOfSides))
            {
                return passwordPolicy.LengthMaximum - (die.Roll() - 1);
            }
        }
    }

    // Create and start the timer.
    private static Stopwatch TimerStart()
    {
        Stopwatch timer = new Stopwatch();

        timer.Start();
        return timer;
    }

    // Stop the timer and return result.
    private static TimeSpan TimerStop(Stopwatch timer)
    {
        timer.Stop();
        return timer.Elapsed;
    }

    /// <summary>
    /// Overrides the default ToString().
    /// </summary>
    /// <returns>
    /// The password as a Unicode (UTF-8) string.
    /// </returns>
    public override string ToString()
    {
        return String.Format(CultureInfo.InvariantCulture, "PasswordGenerator: " + this.Password ?? String.Empty);
    }
}
}
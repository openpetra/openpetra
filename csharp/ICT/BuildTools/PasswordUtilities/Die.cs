// Copyright (c) 2013 Mark Pearce
// http://opensource.org/licenses/MIT

using System;
using System.Security.Cryptography;
using System.Globalization;

namespace PasswordUtilities
{
/// <summary>
/// This class generates cryptographically-strong
/// pseudo-random numbers within a specified range.
/// </summary>
/// <remarks>
/// With current implementation, the die
/// can have between 1 and 255 sides.
/// </remarks>
internal sealed class Die : IDisposable
{
    private const Int32 SINGLE_BYTE = 1;
    private const Int32 NUMBER_SIDES_MINIMUM = 1;
    private const Int32 NUMBER_SIDES_NORMAL = 6;
    private const Int32 NUMBER_SIDES_MAXIMUM = 255;

    private readonly RNGCryptoServiceProvider CSPRNG = new RNGCryptoServiceProvider();

    private Int32 m_NumberOfSides;
    private byte m_FairRollLimit;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Die()
    {
        this.NumberOfSides = NUMBER_SIDES_NORMAL;
    }

    /// <summary>
    /// Constructor specifying number of sides.
    /// </summary>
    public Die(Int32 numberOfSides)
    {
        this.NumberOfSides = numberOfSides;
    }

    /// <summary>
    /// The number of sides of this die - must be between 1 and 255.
    /// </summary>
    /// <returns>
    /// An integer between 1 and 255 representing the number of die sides.
    /// </returns>
    public Int32 NumberOfSides
    {
        get
        {
            this.AlreadyDisposedCheck();
            return m_NumberOfSides;
        }
        set
        {
            this.AlreadyDisposedCheck();

            if ((value < NUMBER_SIDES_MINIMUM) || (value > NUMBER_SIDES_MAXIMUM))
            {
                throw new ArgumentException("NumberOfSides", String.Format(CultureInfo.InvariantCulture, "Die must have between 1 and 255 sides"));
            }

            m_NumberOfSides = value;
            this.FairRollLimit = (byte)value;
        }
    }

    /// <summary>
    /// Calculate the maximum roll result that avoids modulo bias.
    /// </summary>
    /// <remarks>
    /// This property removes the modulo bias. For more details, see
    /// http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    /// There are Byte.MaxValue / NumberSides complete sets that can appear
    /// in a single byte. For instance, if we have a 6-sided die, there are
    /// 42 full sets of 1-6 that come up. The 43rd set is incomplete.
    /// </remarks>
    /// <returns>
    /// The maximum roll result that avoids modulo bias.
    /// </returns>
    public Byte FairRollLimit
    {
        get
        {
            this.AlreadyDisposedCheck();
            return m_FairRollLimit;
        }
        // If the roll is within this range of fair values, then we let it
        // continue. In the 6-sided die case, a roll between 0 and 251 is
        // allowed. We need to subtract 1 from 6 * 42 because otherwise we
        // would be letting through an extra 0 value. For example, 252
        // through 255 would provide an extra 0, 1, 2, 3, so they are not
        // fair to use.
        private set
        {
            this.AlreadyDisposedCheck();
            Int32 fullSetsOfValues = (Byte.MaxValue / value);
            m_FairRollLimit = (byte)((fullSetsOfValues * value) - 1);
        }
    }

    /// <summary>
    /// Rolls the die using CSPRNG with modulo.
    /// </summary>
    /// <returns>
    /// An integer between 1 and the die's number of sides.
    /// </returns>
    /// <remarks>
    /// This uses the modulo operation to convert a cryptographically-secure
    /// pseudo-random byte into an integer within the required range
    /// (between 1 and the number of die sides). The modulo operation is fast,
    /// but creates a bias. The FairRollLimit check removes the modulo bias.
    /// </remarks>
    public Int32 Roll()
    {
        this.AlreadyDisposedCheck();
        byte[] randomByte = new byte[SINGLE_BYTE];

        do
        {
            CSPRNG.GetBytes(randomByte);
        } while (randomByte[0] > this.FairRollLimit);

        // The roll result is zero-based, so we add 1.
        this.RollResult = ((randomByte[0] % (byte)this.NumberOfSides) + 1);

        return this.RollResult;
    }

    /// <summary>
    /// The result from the previous roll of the die.
    /// </summary>
    /// <returns>
    /// An integer between 1 and the number of die sides.
    /// </returns>
    public Int32 RollResult {
        get; private set;
    }

    /// <summary>
    /// Has this type been disposed already?
    /// </summary>
    public bool Disposed {
        get; private set;
    }

    /// <summary>
    /// Overrides the default ToString().
    /// </summary>
    /// <returns>
    /// The result of the last roll of the die.
    /// </returns>
    public override string ToString()
    {
        this.AlreadyDisposedCheck();
        return String.Concat("Dice: ", this.RollResult.ToString(CultureInfo.InvariantCulture));
    }

    // Invoke at the start of every public method.
    private void AlreadyDisposedCheck()
    {
        if (this.Disposed)
        {
            throw new ObjectDisposedException(String.Format(CultureInfo.InvariantCulture, "This type instance has already been disposed!"));
        }
    }

    /// <summary>
    /// Implements IDisposable.
    /// </summary>
    /// <remarks>
    /// Don't make this method virtual. A derived type should
    /// not be able to override this method.
    /// Because this type only disposes managed resources, it
    /// don't need a finaliser. A finaliser isn't allowed to
    /// dispose managed resources.
    /// Without a finaliser, this type doesn't need an internal
    /// implementation of Dispose() and doesn't need to suppress
    /// finalisation to avoid race conditions. So the full
    /// IDisposable code pattern isn't required.
    /// </remarks>
    public void Dispose()
    {
        if (this.Disposed == false)
        {
            this.Disposed = true;
            CSPRNG.Dispose();
        }
    }
}
}
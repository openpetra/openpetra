// Copyright (c) 2013 Mark Pearce
// http://opensource.org/licenses/MIT

using System;

namespace PasswordUtilities
{
/// <summary>
/// Represents a set of characters that can be used to create a password.
/// </summary>
public sealed class CharacterSet
{
    private const Int32 CHARACTERSET_OPTIONAL = 0;

    /// <summary>
    /// Validate and create as an optional character set.
    /// </summary>
    public CharacterSet(string key, string title, string characters)
    {
        this.SetupCharacterSet(key, title, characters, CHARACTERSET_OPTIONAL);
    }

    /// <summary>
    /// Validate and create as a mandatory character set.
    /// </summary>
    public CharacterSet(string key, string title, string characters, Int32 minimumNumberOfCharacters)
    {
        this.SetupCharacterSet(key, title, characters, minimumNumberOfCharacters);
    }

    private void SetupCharacterSet(string key, string title, string characters, Int32 minimumNumberOfCharacters)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("key", "Character set key cannot be null or empty");
        }

        if (String.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException("title", "Character set title cannot be null or empty");
        }

        if (String.IsNullOrEmpty(characters))
        {
            throw new ArgumentNullException("characters", "Character set must have at least one character");
        }

        if (minimumNumberOfCharacters < 0)
        {
            throw new ArgumentNullException("minimumNumberOfCharacters", "Minimum must be zero or greater");
        }

        this.Key = key.Trim();
        this.Title = title.Trim();
        this.Characters = characters.Trim();
        this.MinimumNumberOfCharacters = minimumNumberOfCharacters;
    }

    /// <summary>
    /// The key to be used to retrieve this character set.
    /// </summary>
    public string Key {
        get; private set;
    }

    /// <summary>
    /// The user-friendly title of this character set.
    /// </summary>
    public string Title {
        get; private set;
    }

    /// <summary>
    /// The characters that make up this character set.
    /// </summary>
    public string Characters {
        get; private set;
    }

    /// <summary>
    /// The characters that make up this character set.
    /// </summary>
    public Int32 MinimumNumberOfCharacters {
        get; set;
    }
}
}
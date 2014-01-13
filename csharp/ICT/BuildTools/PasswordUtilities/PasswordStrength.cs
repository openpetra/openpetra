// Copyright (c) 2013 Mark Pearce
// http://opensource.org/licenses/MIT

using System;

namespace PasswordUtilities
{
/// <summary>
/// List of password strength ranges based on the password policy,
/// estimated symbol set, password source, and password  entropy.
/// </summary>
/// <remarks>
/// This is a bit of a guesstimate, and will be increasingly inaccurate
/// as hardware scales up (CPU speed, memory speed/size) and out (grids,
/// GPUs, FPGAs, etc).
/// For more details, see http://en.wikipedia.org/wiki/Password_strength
/// </remarks>
internal enum PasswordStrength : int
{
    /// <summary>
    /// Password is probably weak in today's terms (2011).
    /// </summary>
    Weak = 0,
    /// <summary>
    /// Password is probably adequate in today's terms (2011).
    /// </summary>
    Adequate = 1,
    /// <summary>
    /// Password is probably strong in today's terms (2011).
    /// </summary>
    Strong = 2,
    /// <summary>
    /// Password is probably very strong in today's terms (2011).
    /// </summary>
    VeryStrong = 3
}
}
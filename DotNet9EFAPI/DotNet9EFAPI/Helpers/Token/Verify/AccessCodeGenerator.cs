using System.Security.Cryptography;
using System.Text;

namespace DotNet9EFAPI.Helpers.Token.Verify;

/// <summary>
/// PURPOSE: GENERATES AND RESULTS A RANDOM ACCESS CODE OF LENGTH int length AS A STRING
/// </summary>
public static class AccessCodeGenerator
{
    // THE LIST OF ALLOWED CHARACTERS FOR GENERATING A RANDOM ACCESS CODE
    private static readonly char[] _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()".ToCharArray();
    
    public static string GetRandomAccssCode(int length)
    {
        if (length <= 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
        
        // ALLOCATE MEMORY FOR STORING THE ACCESS TOKEN
        byte[] bytes = new byte[length];

        // POPULATES byte[] bytes WITH RANDOM NUMBERS TO LATER BE USED AS INDEXES
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        // result WILL STORE THE RANDOM ACCESS CODE
        StringBuilder result = new StringBuilder(length);

        // POPULATES result WITH RANDOM CHARACTERS
        foreach (byte b in bytes) { result.Append(_chars[b % _chars.Length]); }
        
        return result.ToString();
    }
}
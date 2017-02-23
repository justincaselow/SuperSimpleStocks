namespace UI.Console
{
    using System;

    public static class Extensions
    {
        public static void CheckForNull(this object o, string paramName)
        {
            if (o == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
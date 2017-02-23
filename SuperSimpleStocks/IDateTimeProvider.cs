namespace UI.Console
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MarcelJoachimKloubert.Blog
{
    /// <summary>
    /// Speichert den Wert einer Uhrzeit.
    /// </summary>
    public struct Time : IEquatable<Time>,
                         IComparable<Time>,
                         IComparable
    {
        #region Data Members (6)

        private readonly long _TICKS;

        /// <summary>
        /// Speichert den minimalen Tick-Wert.
        /// </summary>
        public const long MinTicks = 0;

        /// <summary>
        /// Der Minimal-Wert.
        /// </summary>
        public static readonly Time MinValue = new Time(ticks: MinTicks);

        /// <summary>
        /// Speichert den maximalen Tick-Wert.
        /// </summary>
        public const long MaxTicks = 863999999999;

        /// <summary>
        /// Der Maximal-Wert.
        /// </summary>
        public static readonly Time MaxValue = new Time(ticks: MaxTicks);

        /// <summary>
        /// Der 0-Wert.
        /// </summary>
        public static readonly Time Zero = new Time(ticks: 0);

        #endregion Data Members

        #region Delegates (1)

        private delegate bool TryParseInnerHandler(string str,
                                                   out TimeSpan result);

        #endregion Delegates

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Klasse der <see cref="Time" /> Struktur.
        /// </summary>
        /// <param name="ticks">Der zugrundeliegende Wert in Ticks.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ticks" /> ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public Time(long ticks)
        {
            CheckTickValue(ticks);
            this._TICKS = ticks;
        }

        #endregion Constructors

        #region Methods (56)

        /// <summary>
        /// Addiert eine Zeitspanne auf diesen <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="ts">Der zu addierende Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public Time Add(TimeSpan ts)
        {
            return this.Add(ticks: ts.Ticks);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf diesen <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="ticks">Der urspüngliche Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public Time Add(long ticks)
        {
            return new Time(this._TICKS + ticks);
        }

        private static string AsString(IEnumerable<char> chars)
        {
            if (chars == null)
            {
                return null;
            }

            if (chars is string)
            {
                return (string)chars;
            }

            if (chars is char[])
            {
                return new string((char[])chars);
            }

            return new string(chars.ToArray());
        }

        private static void CheckTickValue(long ticks)
        {
            if (ticks < MinTicks || ticks > MaxTicks)
            {
                throw new ArgumentOutOfRangeException("ticks",
                                                      ticks,
                                                      string.Format("Der Wert muss grösser oder gleich {0} und kleiner oder gleich {1} sein!",
                                                                    MinTicks,
                                                                    MaxTicks));
            }
        }

        /// <summary>
        /// Vergleicht zwei <see cref="Time" />-Werte und gibt eine ganze Zahl zurück,
        /// die angibt, ob der erste Wert kürzer oder länger als der zweite Wert ist
        /// oder ob beide Werte die gleiche Länge aufweisen.
        /// </summary>
        /// <param name="x">Der erste Wert.</param>
        /// <param name="y">Der zweite Wert.</param>
        /// <returns>
        /// -1 <paramref name="x" /> ist kürzer als <paramref name="y" />.
        /// 0 <paramref name="x" /> ist gleich <paramref name="y" />.
        /// 1 <paramref name="x" /> ist länger als <paramref name="y" />.
        /// </returns>
        public static int Compare(Time x, Time y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IComparable.CompareTo(object)" />
        public int CompareTo(object other)
        {
            if (other == null)
            {
                return 1;
            }

            var otherClock = (Time)other;

            var ticks = otherClock._TICKS;
            if (this._TICKS > ticks)
            {
                return 1;
            }
            else if (this._TICKS < ticks)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IComparable{T}.CompareTo(T)" />
        public int CompareTo(Time other)
        {
            return this._TICKS
                       .CompareTo(other._TICKS);
        }

        /// <summary>
        /// Erzeugt eine neue Klasse der <see cref="Time"/> Struktur.
        /// </summary>
        /// <param name="hours">Der Wert der <see cref="Time.Hours"/>-Eigenschaft.</param>
        /// <param name="minutes">Der Wert der <see cref="Time.Minutes"/>-Eigenschaft.</param>
        /// <param name="seconds">Der Wert der <see cref="Time.Seconds"/>-Eigenschaft.</param>
        /// <param name="milliseconds">Der Wert der <see cref="Time.Milliseconds"/>-Eigenschaft.</param>
        /// <param name="additionalTicks">Der Wert in Ticks, der am Schluss aufaddiert werden soll.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert oder einer der Eingabe-Parameter ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public static Time Create(int hours,
                                  int minutes = 0,
                                  int seconds = 0,
                                  int milliseconds = 0,
                                  long additionalTicks = 0)
        {
            return new Time(ticks: (((hours * 3600L) +
                                     (minutes * 60L) +
                                     seconds) * 1000L + milliseconds) * 10000L    // ticks per millisecond
                                                                      + additionalTicks);
        }

        /// <summary>
        /// Erzeugt eine neue Instanz aus einem Ticks-Wert.
        /// </summary>
        /// <param name="ticks">Die Anzahl an Ticks, die den Wert repräsentiert.</param>
        /// <returns>Die neue Instanz.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ticks" /> ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public static Time FromTicks(long ticks)
        {
            return new Time(ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IEquatable{T}.Equals(T)" />
        public bool Equals(Time other)
        {
            return this._TICKS == other._TICKS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.Equals(object)" />
        public override bool Equals(object other)
        {
            if (other is Time)
            {
                return this.Equals((Time)other);
            }

            return base.Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.GetHashCode()" />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.Parse(string)" />
        public static Time Parse(IEnumerable<char> str)
        {
            return new Time(ticks: TimeSpan.Parse(AsString(str)).Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.Parse(string, IFormatProvider)" />
        public static Time Parse(IEnumerable<char> str, IFormatProvider formatProvider)
        {
            return new Time(ticks: TimeSpan.Parse(AsString(str), formatProvider).Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ParseExact(string, string, IFormatProvider)" />
        public static Time ParseExact(IEnumerable<char> str, IEnumerable<char> format, IFormatProvider formatProvider)
        {
            return (Time)TimeSpan.ParseExact(AsString(str),
                                             AsString(format),
                                             formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ParseExact(string, string[], IFormatProvider)" />
        public static Time ParseExact(IEnumerable<char> str, IEnumerable<IEnumerable<char>> formats, IFormatProvider formatProvider)
        {
            return (Time)TimeSpan.ParseExact(AsString(str),
                                             formats == null ? null : formats.Select(f => AsString(f))
                                                                             .ToArray(),
                                             formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ParseExact(string, string, IFormatProvider, TimeSpanStyles)" />
        public static Time ParseExact(IEnumerable<char> str, IEnumerable<char> format, IFormatProvider formatProvider, TimeSpanStyles styles)
        {
            return (Time)TimeSpan.ParseExact(AsString(str),
                                             AsString(format),
                                             formatProvider,
                                             styles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ParseExact(string, string[], IFormatProvider, TimeSpanStyles)" />
        public static Time ParseExact(IEnumerable<char> str, IEnumerable<IEnumerable<char>> formats, IFormatProvider formatProvider, TimeSpanStyles styles)
        {
            return (Time)TimeSpan.ParseExact(AsString(str),
                                             formats == null ? null : formats.Select(f => AsString(f))
                                                                             .ToArray(),
                                             formatProvider,
                                             styles);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von diesem <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="ts">Der zu subtrahierende Wert.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public Time Subtract(TimeSpan ts)
        {
            return this.Subtract(ticks: ts.Ticks);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von diesem <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public Time Subtract(long ticks)
        {
            return new Time(this._TICKS - ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.ToString()" />
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}",
                                 this.Hours.ToString().PadLeft(2, '0'),
                                 this.Minutes.ToString().PadLeft(2, '0'),
                                 this.Seconds.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ToString(string)" />
        public string ToString(IEnumerable<char> format)
        {
            return TimeSpan.FromTicks(this._TICKS)
                           .ToString(AsString(format));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ToString(string, IFormatProvider)" />
        public string ToString(IEnumerable<char> format, IFormatProvider formatProvider)
        {
            return TimeSpan.FromTicks(this._TICKS)
                           .ToString(AsString(format),
                                     formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParse(string, out TimeSpan)" />
        public static bool TryParse(IEnumerable<char> str, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParse(input,
                                                              out tsResult);
                                 }, str
                                  , result: out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParse(string, IFormatProvider, out TimeSpan)" />
        public static bool TryParse(IEnumerable<char> str, IFormatProvider formatProvider, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParse(input, formatProvider,
                                                              out tsResult);
                                 }, str
                                  , out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParseExact(string, string, IFormatProvider, out TimeSpan)" />
        public static bool TryParseExact(IEnumerable<char> str, IEnumerable<char> format, IFormatProvider formatProvider, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParseExact(input, AsString(format), formatProvider,
                                                                   out tsResult);
                                 }, str
                                  , out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParseExact(string, string[], IFormatProvider, out TimeSpan)" />
        public static bool TryParseExact(IEnumerable<char> str, IEnumerable<IEnumerable<char>> formats, IFormatProvider formatProvider, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParseExact(input,
                                                                   formats == null ? null : formats.Select(f => AsString(f))
                                                                                                   .ToArray(),
                                                                   formatProvider,
                                                                   out tsResult);
                                 }, str
                                  , out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParseExact(string, string, IFormatProvider, TimeSpanStyles, out TimeSpan)" />
        public static bool TryParseExact(IEnumerable<char> str, IEnumerable<char> format, IFormatProvider formatProvider, TimeSpanStyles styles, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParseExact(input, AsString(format), formatProvider, styles,
                                                                   out tsResult);
                                 }, str
                                  , out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParseExact(string, string[], IFormatProvider, TimeSpanStyles, out TimeSpan)" />
        public static bool TryParseExact(IEnumerable<char> str, IEnumerable<IEnumerable<char>> formats, IFormatProvider formatProvider, TimeSpanStyles styles, out Time result)
        {
            return TryParseInner(delegate(string input, out TimeSpan tsResult)
                                 {
                                     return TimeSpan.TryParseExact(input,
                                                                   formats == null ? null : formats.Select(f => AsString(f))
                                                                                                   .ToArray(),
                                                                   formatProvider,
                                                                   styles,
                                                                   out tsResult);
                                 }, str
                                  , out result);
        }

        private static bool TryParseInner(TryParseInnerHandler handler, IEnumerable<char> str, out Time result)
        {
            result = default(Time);

            TimeSpan ts;
            if (handler(AsString(str), out ts))
            {
                result = (Time)ts;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Konvertiert einen <see cref="Time" /> implizit
        /// nach <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator TimeSpan(Time clock)
        {
            return new TimeSpan(ticks: clock.Ticks);
        }

        /// <summary>
        /// Konvertiert einen <see cref="TimeSpan" /> explizit
        /// nach <see cref="Time" />.
        /// </summary>
        /// <param name="ts">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ts" /> ist ein Wert ausserhalb des gültigen Bereichs.
        /// </exception>
        public static explicit operator Time(TimeSpan ts)
        {
            return new Time(ts.Ticks);
        }

        /// <summary>
        /// Explizite Konvertierung von <see cref="Time" /> nach <see cref="string" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static explicit operator string(Time clock)
        {
            return clock.ToString();
        }

        /// <summary>
        /// Implizite Konvertierung von <see cref="string" /> nach <see cref="Time" />.
        /// </summary>
        /// <param name="str">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator Time?(string str)
        {
            return str != null ? Time.Parse(str) : (Time?)null;
        }

        /// <summary>
        /// Konvertiert einen <see cref="Time" />
        /// implizit nach <see cref="long" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator long(Time clock)
        {
            return clock._TICKS;
        }

        /// <summary>
        /// Konvertiert einen <see cref="long" />
        /// explizit nach <see cref="Time" />.
        /// </summary>
        /// <param name="ticks">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static explicit operator Time(long ticks)
        {
            return new Time(ticks: ticks);
        }

        /// <summary>
        /// Klont einen Wert.
        /// </summary>
        /// <param name="clock">Der zu klonende Wert.</param>
        /// <param name="count">Die Anzahl der Instanzen.</param>
        /// <returns>Die verzögerte Sequenz mit geklonten Instanzen.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> ist kleiner als 0.
        /// </exception>
        public static IEnumerable<Time> operator *(Time clock, long count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count",
                                                      count,
                                                      "Der Wert muss grösser oder gleich 0 sein!");
            }

            for (long i = 0; i < count; i++)
            {
                yield return clock;
            }
        }

        /// <summary>
        /// Prüft, ob ein Wert grösser ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist grösser als <paramref name="right" /> oder nicht.</returns>
        public static bool operator >(Time left, Time right)
        {
            return left._TICKS > right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert grösser oder gleich ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist grösser oder gleich als <paramref name="right" /> oder nicht.</returns>
        public static bool operator >=(Time left, Time right)
        {
            return left._TICKS >= right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert kleiner ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist kleiner als <paramref name="right" /> oder nicht.</returns>
        public static bool operator <(Time left, Time right)
        {
            return left._TICKS < right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert kleiner oder gleich ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist kleiner oder gleich als <paramref name="right" /> oder nicht.</returns>
        public static bool operator <=(Time left, Time right)
        {
            return left._TICKS <= right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert gleich ist wie ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist gleich <paramref name="right" /> oder nicht.</returns>
        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Prüft, ob ein Wert ungleich ist wie ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist ungleich <paramref name="right" /> oder nicht.</returns>
        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf einen <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ts">Der zu addierende Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        public static Time operator +(Time clock, TimeSpan ts)
        {
            return clock.Add(ts);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf einen <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        public static Time operator +(Time clock, long ticks)
        {
            return clock.Add(ticks);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von einem <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ts">Der zu subtrahierende Wert.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        public static Time operator -(Time clock, TimeSpan ts)
        {
            return clock.Subtract(ts);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von einem <see cref="Time" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        public static Time operator -(Time clock, long ticks)
        {
            return clock.Subtract(ticks);
        }

        /// <summary>
        /// Überträgt einen <see cref="Time" />-Wert auf einen
        /// <see cref="DateTime" />-Wert.
        /// </summary>
        /// <param name="dateTime">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTime" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTime operator <=(DateTime dateTime, Time clock)
        {
            return dateTime.Date
                           .AddTicks(clock.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTime operator >=(DateTime dateTime, Time clock)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="Time" />-Wert auf einen
        /// <see cref="DateTimeOffset" />-Wert.
        /// </summary>
        /// <param name="dateTimeOff">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTimeOff" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTimeOffset operator <=(DateTimeOffset dateTimeOff, Time clock)
        {
            return new DateTimeOffset(dateTimeOff.Date,
                                      dateTimeOff.Offset).AddTicks(clock.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTimeOffset operator >=(DateTimeOffset dateTimeOff, Time clock)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="Time" />-Wert auf einen
        /// <see cref="DateTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <param name="dateTime">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTime" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTime operator >=(Time clock, DateTime dateTime)
        {
            return dateTime <= clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTime operator <=(Time clock, DateTime dateTime)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="Time" />-Wert auf einen
        /// <see cref="DateTimeOffset" />-Wert.
        /// </summary>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <param name="dateTimeOff">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTimeOff" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTimeOffset operator >=(Time clock, DateTimeOffset dateTimeOff)
        {
            return dateTimeOff <= clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTimeOffset operator <=(Time clock, DateTimeOffset dateTimeOff)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Konvertiert einen <see cref="Time" /> Wert implizit nach <see cref="DateTime" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator DateTime(Time clock)
        {
            return DateTime.Today
                           .AddTicks(clock._TICKS);
        }

        /// <summary>
        /// Konvertiert einen <see cref="Time" /> Wert implizit nach <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator DateTimeOffset(Time clock)
        {
            return DateTime.Today
                           .AddTicks(clock._TICKS);
        }

        #endregion Methods

        #region Properties (10)

        /// <summary>
        /// Gibt die Stunden-Komponente dieses <see cref="Time" />-Wertes zurück.
        /// </summary>
        public int Hours
        {
            get { return (int)(this._TICKS / 36000000000L % 24L); }
        }

        /// <summary>
        /// Gibt die Millisekunden.Komponente dieses <see cref="Time" />-Wertes zurück.
        /// </summary>
        public int Milliseconds
        {
            get { return (int)(this._TICKS / 10000L % 1000L); }
        }

        /// <summary>
        /// Gibt die Minuten-Komponente dieses <see cref="Time" />-Wertes zurück.
        /// </summary>
        public int Minutes
        {
            get { return (int)(this._TICKS / 600000000L % 60L); }
        }

        /// <summary>
        /// Gibt die aktuelle Uhrzeit zurück.
        /// </summary>
        public static Time Now
        {
            get
            {
                var now = DateTime.Now;
                return new Time((now - now.Date).Ticks);
            }
        }

        /// <summary>
        /// Gibt die Sekunden-Komponente dieses <see cref="Time" />-Wertes zurück.
        /// </summary>
        public int Seconds
        {
            get { return (int)(this._TICKS / 10000000L % 60L); }
        }

        /// <summary>
        /// Gibt den zugrundeliegenden Wert in Ticks zurück
        /// (verstrichene Ticks seit 00:00 Uhr).
        /// </summary>
        public long Ticks
        {
            get { return this._TICKS; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="Time" />-Struktur in ganzen Tagen
        /// und Bruchteilen von Stunden seit 00:00 Uhr ab.
        /// </summary>
        public double TotalHours
        {
            get { return (double)this._TICKS * 2.7777777777777777E-11; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="Time" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Millisekunden seit 00:00 Uhr ab.
        /// </summary>
        public double TotalMilliseconds
        {
            get
            {
                double num = (double)this._TICKS * 0.0001;

                if (num > 922337203685477.0)
                {
                    return 922337203685477.0;
                }

                if (num < -922337203685477.0)
                {
                    return -922337203685477.0;
                }

                return num;
            }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="Time" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Minuten seit 00:00 Uhr ab.
        /// </summary>
        public double TotalMinutes
        {
            get { return (double)this._TICKS * 1.6666666666666667E-09; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="Time" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Sekunden seit 00:00 Uhr ab.
        /// </summary>
        public double TotalSeconds
        {
            get { return (double)this._TICKS * 1E-07; }
        }

        #endregion Properties
    }
}

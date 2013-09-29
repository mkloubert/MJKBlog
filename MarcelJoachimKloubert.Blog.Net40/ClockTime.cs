// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog
{
    /// <summary>
    /// Speichert den Wert einer Uhrzeit.
    /// </summary>
    public struct ClockTime : IEquatable<ClockTime>,
                              IComparable<ClockTime>,
                              IComparable
    {
        #region Data Members (4)

        private readonly long _TICKS;

        /// <summary>
        /// Der Minimal-Wert.
        /// </summary>
        public static readonly ClockTime MinValue = new ClockTime(0);

        /// <summary>
        /// Der Maximal-Wert.
        /// </summary>
        public static readonly ClockTime MaxValue = new ClockTime(863999999999);

        /// <summary>
        /// Der 0-Wert.
        /// </summary>
        public static readonly ClockTime Zero = new ClockTime(0);

        #endregion Data Members

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Klasse des <see cref="ClockTime" /> Wertes.
        /// </summary>
        /// <param name="ticks">Der zugrundeliegende Wert in Ticks.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ticks" /> ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public ClockTime(long ticks)
        {
            if (ticks < 0 || ticks >= 864000000000)
            {
                throw new ArgumentOutOfRangeException("ticks",
                                                      ticks,
                                                      "Der Wert muss zwischen 0 und 863999999999 (einschliesslich) liegen!");
            }

            this._TICKS = ticks;
        }

        #endregion Constructors

        #region Methods (39)

        /// <summary>
        /// Addiert eine Zeitspanne auf diesen <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="ts">Der zu addierende Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public ClockTime Add(TimeSpan ts)
        {
            return this.Add(ticks: ts.Ticks);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf diesen <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="ticks">Der urspüngliche Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public ClockTime Add(long ticks)
        {
            return new ClockTime(this._TICKS + ticks);
        }

        /// <summary>
        /// Vergleicht zwei <see cref="ClockTime" />-Werte und gibt eine ganze Zahl zurück,
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
        public static int Compare(ClockTime x, ClockTime y)
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

            var otherClock = (ClockTime)other;

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
        public int CompareTo(ClockTime other)
        {
            return this._TICKS.CompareTo(other._TICKS);
        }

        /// <summary>
        /// Erzeugt eine neue Instanz aus einem Ticks-Wert.
        /// </summary>
        /// <param name="ticks">Die Anzahl an Ticks, die den Wert repräsentiert.</param>
        /// <returns>Die neue Instanz.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ticks" /> ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public static ClockTime FromTicks(long ticks)
        {
            return new ClockTime(ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IEquatable{T}.Equals(T)" />
        public bool Equals(ClockTime other)
        {
            return this._TICKS == other._TICKS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.Equals(object)" />
        public override bool Equals(object other)
        {
            if (other is ClockTime)
            {
                return this.Equals((ClockTime)other);
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
        public static ClockTime Parse(string str)
        {
            return (ClockTime)TimeSpan.Parse(str);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von diesem <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="ts">Der zu subtrahierende Wert.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public ClockTime Subtract(TimeSpan ts)
        {
            return this.Subtract(ticks: ts.Ticks);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von diesem <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        public ClockTime Subtract(long ticks)
        {
            return new ClockTime(this._TICKS - ticks);
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
        public string ToString(string format)
        {
            return TimeSpan.FromTicks(this._TICKS)
                           .ToString(format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.ToString(string, IFormatProvider)" />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return TimeSpan.FromTicks(this._TICKS)
                           .ToString(format, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TimeSpan.TryParse(string, out TimeSpan)" />
        public static bool TryParse(string str, out ClockTime value)
        {
            value = default(ClockTime);

            TimeSpan ts;
            if (TimeSpan.TryParse(str, out ts))
            {
                value = (ClockTime)ts;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Konvertiert einen <see cref="ClockTime" /> implizit
        /// nach <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator TimeSpan(ClockTime clock)
        {
            return new TimeSpan(ticks: clock.Ticks);
        }

        /// <summary>
        /// Konvertiert einen <see cref="TimeSpan" /> explizit
        /// nach <see cref="ClockTime" />.
        /// </summary>
        /// <param name="ts">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ts" /> ist ein Wert ausserhalb des gültigen Bereichs.
        /// </exception>
        public static explicit operator ClockTime(TimeSpan ts)
        {
            return new ClockTime(ts.Ticks);
        }

        /// <summary>
        /// Explizite Konvertierung von <see cref="ClockTime" /> nach <see cref="string" />.
        /// </summary>
        /// <param name="clock">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static explicit operator string(ClockTime clock)
        {
            return clock.ToString();
        }

        /// <summary>
        /// Implizite Konvertierung von <see cref="string" /> nach <see cref="ClockTime" />.
        /// </summary>
        /// <param name="str">Der Quellwert.</param>
        /// <returns>Der Zielwert.</returns>
        public static implicit operator ClockTime(string str)
        {
            return ClockTime.Parse(str);
        }

        /// <summary>
        /// Prüft, ob ein Wert grösser ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist grösser als <paramref name="right" /> oder nicht.</returns>
        public static bool operator >(ClockTime left, ClockTime right)
        {
            return left._TICKS > right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert grösser oder gleich ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist grösser oder gleich als <paramref name="right" /> oder nicht.</returns>
        public static bool operator >=(ClockTime left, ClockTime right)
        {
            return left._TICKS >= right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert kleiner ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist kleiner als <paramref name="right" /> oder nicht.</returns>
        public static bool operator <(ClockTime left, ClockTime right)
        {
            return left._TICKS < right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert kleiner oder gleich ist als ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist kleiner oder gleich als <paramref name="right" /> oder nicht.</returns>
        public static bool operator <=(ClockTime left, ClockTime right)
        {
            return left._TICKS <= right._TICKS;
        }

        /// <summary>
        /// Prüft, ob ein Wert gleich ist wie ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist gleich <paramref name="right" /> oder nicht.</returns>
        public static bool operator ==(ClockTime left, ClockTime right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Prüft, ob ein Wert ungleich ist wie ein anderer.
        /// </summary>
        /// <param name="left">Der linke Wert.</param>
        /// <param name="right">Der rechte Wert.</param>
        /// <returns><paramref name="left" /> ist ungleich <paramref name="right" /> oder nicht.</returns>
        public static bool operator !=(ClockTime left, ClockTime right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf einen <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ts">Der zu addierende Wert.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        public static ClockTime operator +(ClockTime clock, TimeSpan ts)
        {
            return clock.Add(ts);
        }

        /// <summary>
        /// Addiert eine Zeitspanne auf einen <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der aufaddierte Wert.</returns>
        public static ClockTime operator +(ClockTime clock, long ticks)
        {
            return clock.Add(ticks);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von einem <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ts">Der zu subtrahierende Wert.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        public static ClockTime operator -(ClockTime clock, TimeSpan ts)
        {
            return clock.Subtract(ts);
        }

        /// <summary>
        /// Subtrahiert eine Zeitspanne von einem <see cref="ClockTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der urspüngliche Wert.</param>
        /// <param name="ticks">Der zu addierende Wert in Ticks.</param>
        /// <returns>Der subtrahierte Wert.</returns>
        public static ClockTime operator -(ClockTime clock, long ticks)
        {
            return clock.Subtract(ticks);
        }

        /// <summary>
        /// Überträgt einen <see cref="ClockTime" />-Wert auf einen
        /// <see cref="DateTime" />-Wert.
        /// </summary>
        /// <param name="dateTime">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTime" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTime operator <=(DateTime dateTime, ClockTime clock)
        {
            return dateTime.Date
                           .AddTicks(clock.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTime operator >=(DateTime dateTime, ClockTime clock)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="ClockTime" />-Wert auf einen
        /// <see cref="DateTimeOffset" />-Wert.
        /// </summary>
        /// <param name="dateTimeOff">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTimeOff" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTimeOffset operator <=(DateTimeOffset dateTimeOff, ClockTime clock)
        {
            return new DateTimeOffset(dateTimeOff.Date,
                                      dateTimeOff.Offset).AddTicks(clock.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTimeOffset operator >=(DateTimeOffset dateTimeOff, ClockTime clock)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="ClockTime" />-Wert auf einen
        /// <see cref="DateTime" />-Wert.
        /// </summary>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <param name="dateTime">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTime" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTime operator >=(ClockTime clock, DateTime dateTime)
        {
            return dateTime <= clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTime operator <=(ClockTime clock, DateTime dateTime)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Überträgt einen <see cref="ClockTime" />-Wert auf einen
        /// <see cref="DateTimeOffset" />-Wert.
        /// </summary>
        /// <param name="clock">Der zu übertragende Wert.</param>
        /// <param name="dateTimeOff">Der Wert auf den <paramref name="clock" /> übertragen werden soll.</param>
        /// <returns>
        /// Der verschmolzene Wert aus <paramref name="dateTimeOff" /> und <paramref name="clock" />.
        /// </returns>
        public static DateTimeOffset operator >=(ClockTime clock, DateTimeOffset dateTimeOff)
        {
            return dateTimeOff <= clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException">Operation wird nicht unterstützt.</exception>
        public static DateTimeOffset operator <=(ClockTime clock, DateTimeOffset dateTimeOff)
        {
            throw new NotSupportedException();
        }

        #endregion Methods

        #region Properties (10)

        /// <summary>
        /// Gibt die Stunden-Komponente dieses <see cref="ClockTime" />-Wertes zurück.
        /// </summary>
        public int Hours
        {
            get { return (int)(this._TICKS / 36000000000L % 24L); }
        }

        /// <summary>
        /// Gibt die Millisekunden.Komponente dieses <see cref="ClockTime" />-Wertes zurück.
        /// </summary>
        public int Milliseconds
        {
            get { return (int)(this._TICKS / 10000L % 1000L); }
        }

        /// <summary>
        /// Gibt die Minuten-Komponente dieses <see cref="ClockTime" />-Wertes zurück.
        /// </summary>
        public int Minutes
        {
            get { return (int)(this._TICKS / 600000000L % 60L); }
        }

        /// <summary>
        /// Gibt die aktuelle Uhrzeit zurück.
        /// </summary>
        public static ClockTime Now
        {
            get
            {
                var now = DateTime.Now;
                return new ClockTime((now - now.Date).Ticks);
            }
        }

        /// <summary>
        /// Gibt die Sekunden-Komponente dieses <see cref="ClockTime" />-Wertes zurück.
        /// </summary>
        public int Seconds
        {
            get { return (int)(this._TICKS / 10000000L % 60L); }
        }

        /// <summary>
        /// Gibt den zugrundeliegenden Wert in Ticks zurück.
        /// </summary>
        public long Ticks
        {
            get { return this._TICKS; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="ClockTime" />-Struktur in ganzen Tagen
        /// und Bruchteilen von Stunden ab.
        /// </summary>
        public double TotalHours
        {
            get { return (double)this._TICKS * 2.7777777777777777E-11; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="ClockTime" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Millisekunden ab.
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
        /// Ruft den Wert der aktuellen <see cref="ClockTime" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Minuten ab.
        /// </summary>
        public double TotalMinutes
        {
            get { return (double)this._TICKS * 1.6666666666666667E-09; }
        }

        /// <summary>
        /// Ruft den Wert der aktuellen <see cref="ClockTime" />-Struktur
        /// in ganzen Tagen und Bruchteilen von Sekunden ab.
        /// </summary>
        public double TotalSeconds
        {
            get { return (double)this._TICKS * 1E-07; }
        }

        #endregion Properties
    }
}

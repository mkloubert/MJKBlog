// s. http://blog.marcel-kloubert.de


using System;
using System.Data;
using Microsoft.SqlServer.Server;

/// <summary>
/// 
/// </summary>
public partial class StoredProcedures
{
    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// 
    /// </summary>
    [global::Microsoft.SqlServer.Server.SqlProcedure]
    public static void MeineTestSP()
    {
        // Struktur festlegen
        var rec = new SqlDataRecord(new SqlMetaData[] {
            // Name    : Spalte1
            // Datentyp: nvarchar(max), s. -1
            new SqlMetaData("Spalte1", SqlDbType.NVarChar, -1),
 
            // Name    : Spalte2
            // Datentyp: date
            new SqlMetaData("Spalte2", SqlDbType.Date),
 
            // Name    : Spalte3
            // Datentyp: int
            new SqlMetaData("Spalte3", SqlDbType.Int),
        });

        // mit dem Senden der Daten beginnen
        SqlContext.Pipe.SendResultsStart(rec);

        // 1. Zeile definieren und senden
        {
            rec.SetString(0, "TM");
            rec.SetDateTime(1, new DateTime(1979, 9, 5));
            rec.SetInt32(2, 5979);

            SqlContext.Pipe.SendResultsRow(rec);
        }

        // 2. Zeile definieren und senden
        {
            rec.SetString(0, "MK");
            rec.SetDateTime(1, new DateTime(1979, 9, 23));
            rec.SetInt32(2, 23979);

            SqlContext.Pipe.SendResultsRow(rec);
        }

        // das Senden der Daten abschließen
        SqlContext.Pipe.SendResultsEnd();
    }

    #endregion Methods
}

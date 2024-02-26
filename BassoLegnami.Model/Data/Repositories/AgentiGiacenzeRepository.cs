using BassoLegnami.Model.Models.Support;
using In.Core.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;

namespace BassoLegnami.Model.Data.Repositories
{
    public interface IAgentiGiacenzeRepository : IGenericRepository<AgentiGiacenze>
    {
        List<AgentiGiacenze> GetData(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza);
        List<AgentiGiacenze> GetAllData(int? id);
        List<AgentiGiacenze> GetDataRank(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza);
        public byte[] Print(List<AgentiGiacenze> giacenze, int? filterTipoStampa, int? filterTipoTavola);
    }

    public class AgentiGiacenzeRepository : GenericRepository<AgentiGiacenze>, IAgentiGiacenzeRepository
    {
        public AgentiGiacenzeRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
        {
        }

        public List<AgentiGiacenze> GetData(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza)
        {
            List<AgentiGiacenze> Giacenze = new();
            List<AgentiGiacenze> EstrazioneGiacenze = new();
            string sqlWhere = string.Empty;

            if (IdEssenza.HasValue || IdClassifica.HasValue || IdStatoLegno.HasValue || IdStagionatura.HasValue || IdDeposito.HasValue || IdProvenienza.HasValue)
            {
                sqlWhere += " WHERE ";
                if (IdEssenza.HasValue)
                {
                    sqlWhere += $"[IdEssenza] = {IdEssenza} AND ";
                }
                if (IdClassifica.HasValue)
                {
                    sqlWhere += $"[IdClassifiche] = {IdClassifica} AND ";
                }
                if (IdStatoLegno.HasValue)
                {
                    sqlWhere += $"[IdStatoLegno] = {IdStatoLegno} AND ";
                }
                if (IdStagionatura.HasValue)
                {
                    sqlWhere += $"[IdStagionatura] = {IdStagionatura} AND ";
                }
                if (IdDeposito.HasValue)
                {
                    sqlWhere += $"[IdDeposito] = {IdDeposito} AND ";
                }
                if (IdProvenienza.HasValue)
                {
                    sqlWhere += $"[IdProvenienza] = {IdProvenienza} AND ";
                }
                sqlWhere = sqlWhere[..^5];
            }

            string sql = ($"SELECT [TipoPacco]" +
                        $", [Essenza] , [Classifica], [StatoLegno], [Stagionatura], [Deposito], [Provenienza], [Fornitore], [UnitaMisuraPrezzoAcquisto], [Misura], [DIM1], [DIM2]" +
                        $", [DIM3], [Quantità], [Pacco], [Tipo], [Volume], [NPackList], [Marchio], [PrezzoAcquisto], [QuantitàVenduta], [DataImpegno], [DataVendita], [ClienteImpegno]," +
                        $" [Certificazione], [Qualita], [Note], [LunghezzaDescr], [Strati], [NumeroCarico], [Id] FROM [AgentiGiacenza] {sqlWhere} ORDER BY [Id] DESC");

            // Create the connection.
            using (SqlConnection connection = new(_context.Database.GetConnectionString()))
            {
                // Create a SqlCommand
                using (SqlCommand sqlCommand = new(sql, connection))
                {
                    // Treat command as a text sql command
                    sqlCommand.CommandType = CommandType.Text;

                    try
                    {
                        // Open connection
                        connection.Open();

                        // Run the sql command
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        // Read records
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Giacenze.Add(new AgentiGiacenze()
                                {
                                    Id = reader.GetInt32("Id"),
                                    TipoPacco = reader.IsDBNull(reader.GetOrdinal("TipoPacco")) ? null : reader.GetString(reader.GetOrdinal("TipoPacco")).Trim(),
                                    Essenza = reader.IsDBNull(reader.GetOrdinal("Essenza")) ? null : reader.GetString(reader.GetOrdinal("Essenza")).Trim(),
                                    Classifica = reader.IsDBNull(reader.GetOrdinal("Classifica")) ? null : reader.GetString(reader.GetOrdinal("Classifica")).Trim(),
                                    StatoLegno = reader.IsDBNull(reader.GetOrdinal("StatoLegno")) ? null : reader.GetString(reader.GetOrdinal("StatoLegno")).Trim(),
                                    Stagionatura = reader.IsDBNull(reader.GetOrdinal("Stagionatura")) ? null : reader.GetString(reader.GetOrdinal("Stagionatura")).Trim(),
                                    Deposito = reader.IsDBNull(reader.GetOrdinal("Deposito")) ? null : reader.GetString(reader.GetOrdinal("Deposito")).Trim(),
                                    Provenienza = reader.IsDBNull(reader.GetOrdinal("Provenienza")) ? null : reader.GetString(reader.GetOrdinal("Provenienza")).Trim(),
                                    Fornitore = reader.IsDBNull(reader.GetOrdinal("Fornitore")) ? null : reader.GetString(reader.GetOrdinal("Fornitore")).Trim(),
                                    UnitaMisuraPrezzoAcquisto = reader.IsDBNull(reader.GetOrdinal("UnitaMisuraPrezzoAcquisto")) ? null : reader.GetString(reader.GetOrdinal("UnitaMisuraPrezzoAcquisto")).Trim(),
                                    Misura = reader.IsDBNull(reader.GetOrdinal("Misura")) ? null : reader.GetString(reader.GetOrdinal("Misura")).Trim(),
                                    Dim1 = reader.GetDecimal("DIM1"),
                                    Dim2 = reader.GetDecimal("DIM2"),
                                    Dim3 = reader.GetDecimal("DIM3"),
                                    Quantita = reader.IsDBNull(reader.GetOrdinal("Quantità")) ? 0 : reader.GetInt16("Quantità"),
                                    Pacco = reader.IsDBNull(reader.GetOrdinal("Pacco")) ? null : reader.GetString(reader.GetOrdinal("Pacco")).Trim(),
                                    Tipo = reader.GetInt16("Tipo"),
                                    Volume = reader.IsDBNull(reader.GetOrdinal("Volume")) ? null : reader.GetDecimal("Volume"),
                                    NPackList = reader.GetInt32("NPackList"),
                                    Marchio = reader.IsDBNull(reader.GetOrdinal("Marchio")) ? null : reader.GetString(reader.GetOrdinal("Marchio")).Trim(),
                                    PrezzoAcquisto = reader.IsDBNull(reader.GetOrdinal("PrezzoAcquisto")) ? null : reader.GetDecimal("PrezzoAcquisto"),
                                    QuantitaVenduta = reader.IsDBNull(reader.GetOrdinal("QuantitàVenduta")) ? null : reader.GetDecimal("QuantitàVenduta"),
                                    DataImpegno = reader.IsDBNull(reader.GetOrdinal("DataImpegno")) ? null : reader.GetDateTime("DataImpegno"),
                                    DataVendita = reader.GetDateTime("DataVendita"),
                                    ClienteImpegno = reader.IsDBNull(reader.GetOrdinal("ClienteImpegno")) ? null : reader.GetString(reader.GetOrdinal("ClienteImpegno")).Trim(),
                                    Certificazione = reader.IsDBNull(reader.GetOrdinal("Certificazione")) ? null : reader.GetString(reader.GetOrdinal("Certificazione")).Trim(),
                                    Qualita = reader.IsDBNull(reader.GetOrdinal("Qualita")) ? null : reader.GetString(reader.GetOrdinal("Qualita")).Trim(),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")).Trim(),
                                    LunghezzaDescr = reader.IsDBNull(reader.GetOrdinal("LunghezzaDescr")) ? null : reader.GetString(reader.GetOrdinal("LunghezzaDescr")).Trim(),
                                    Strati = reader.GetInt16("Strati"),
                                    NumeroCarico =  reader.GetInt32("NumeroCarico"),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Giacenze.Clear();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                try
                {
                    if (Giacenze != null)
                    {
                        foreach (AgentiGiacenze item in Giacenze)
                        {
                            AgentiGiacenze agentiGiancenza = new()
                            {
                                Id = item.Id,
                                TipoPacco = item.TipoPacco,
                                Essenza = item.Essenza,
                                Classifica = item.Classifica,
                                StatoLegno = item.StatoLegno,
                                Stagionatura = item.Stagionatura,
                                Deposito = item.Deposito,
                                Provenienza = item.Provenienza,
                                Fornitore = item.Fornitore,
                                UnitaMisuraPrezzoAcquisto = item.UnitaMisuraPrezzoAcquisto,
                                Misura = item.Misura,
                                Dim1 = item.Dim1,
                                Dim2 = item.Dim2,
                                Dim3 = item.Dim3,
                                Quantita = item.Quantita,
                                Pacco = item.Pacco,
                                Tipo = item.Tipo,
                                Volume = item.Volume,
                                NPackList = item.NPackList,
                                Marchio = item.Marchio,
                                PrezzoAcquisto = item.PrezzoAcquisto,
                                QuantitaVenduta = item.QuantitaVenduta,
                                DataImpegno = item.DataImpegno,
                                DataVendita = item.DataVendita,
                                ClienteImpegno = item.ClienteImpegno,
                                Certificazione = item.Certificazione,
                                Qualita = item.Qualita,
                                Note = item.Note,
                                LunghezzaDescr = item.LunghezzaDescr,
                                Strati = item.Strati,
                                NumeroCarico = item.NumeroCarico
                            };
                            EstrazioneGiacenze.Add(agentiGiancenza);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EstrazioneGiacenze.Clear();
                }
            }
            return EstrazioneGiacenze;
        }

        public List<AgentiGiacenze> GetAllData(int? id)
        {
            List<AgentiGiacenze> Giacenze = new();
            List<AgentiGiacenze> EstrazioneGiacenze = new();
            // Create the connection.
            using (SqlConnection connection = new(_context.Database.GetConnectionString()))
            {
                // Create a SqlCommand
                using (SqlCommand sqlCommand = new
                        ($"SELECT [TipoPacco]" +
                        $", [Essenza] , [Classifica], [StatoLegno], [Stagionatura], [Deposito], [Provenienza], [Fornitore], [UnitaMisuraPrezzoAcquisto], [Misura], [DIM1], [DIM2]" +
                        $", [DIM3], [Quantità], [Pacco], [Tipo], [Volume], [NPackList], [Marchio], [PrezzoAcquisto], [QuantitàVenduta], [DataImpegno], [DataVendita], [ClienteImpegno]," +
                        $" [Certificazione], [Qualita], [Note], [LunghezzaDescr], [Strati], [NumeroCarico], [Id] FROM [Magazzino].[dbo].[AgentiGiacenza] WHERE [Id] =" + id + " ORDER BY [Id] DESC", connection))
                {
                    // Treat command as a text sql command
                    sqlCommand.CommandType = CommandType.Text;

                    try
                    {
                        // Open connection
                        connection.Open();

                        // Run the sql command
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        // Read records
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Giacenze.Add(new AgentiGiacenze()
                                {
                                    Id = reader.GetInt32("Id"),
                                    TipoPacco = reader.IsDBNull(reader.GetOrdinal("TipoPacco")) ? null : reader.GetString(reader.GetOrdinal("TipoPacco")).Trim(),
                                    Essenza = reader.IsDBNull(reader.GetOrdinal("Essenza")) ? null : reader.GetString(reader.GetOrdinal("Essenza")).Trim(),
                                    Classifica = reader.IsDBNull(reader.GetOrdinal("Classifica")) ? null : reader.GetString(reader.GetOrdinal("Classifica")).Trim(),
                                    StatoLegno = reader.IsDBNull(reader.GetOrdinal("StatoLegno")) ? null : reader.GetString(reader.GetOrdinal("StatoLegno")).Trim(),
                                    Stagionatura = reader.IsDBNull(reader.GetOrdinal("Stagionatura")) ? null : reader.GetString(reader.GetOrdinal("Stagionatura")).Trim(),
                                    Deposito = reader.IsDBNull(reader.GetOrdinal("Deposito")) ? null : reader.GetString(reader.GetOrdinal("Deposito")).Trim(),
                                    Provenienza = reader.IsDBNull(reader.GetOrdinal("Provenienza")) ? null : reader.GetString(reader.GetOrdinal("Provenienza")).Trim(),
                                    Fornitore = reader.IsDBNull(reader.GetOrdinal("Fornitore")) ? null : reader.GetString(reader.GetOrdinal("Fornitore")).Trim(),
                                    UnitaMisuraPrezzoAcquisto = reader.IsDBNull(reader.GetOrdinal("UnitaMisuraPrezzoAcquisto")) ? null : reader.GetString(reader.GetOrdinal("UnitaMisuraPrezzoAcquisto")).Trim(),
                                    Misura = reader.IsDBNull(reader.GetOrdinal("Misura")) ? null : reader.GetString(reader.GetOrdinal("Misura")).Trim(),
                                    Dim1 = reader.GetDecimal("DIM1"),
                                    Dim2 = reader.GetDecimal("DIM2"),
                                    Dim3 = reader.GetDecimal("DIM3"),
                                    Quantita = reader.IsDBNull(reader.GetOrdinal("Quantità")) ? 0 : reader.GetInt16("Quantità"),
                                    Pacco = reader.IsDBNull(reader.GetOrdinal("Pacco")) ? null : reader.GetString(reader.GetOrdinal("Pacco")).Trim(),
                                    Tipo = reader.GetInt16("Tipo"),
                                    Volume = reader.IsDBNull(reader.GetOrdinal("Volume")) ? null : reader.GetDecimal("Volume"),
                                    NPackList = reader.GetInt32("NPackList"),
                                    Marchio = reader.IsDBNull(reader.GetOrdinal("Marchio")) ? null : reader.GetString(reader.GetOrdinal("Marchio")).Trim(),
                                    PrezzoAcquisto = reader.IsDBNull(reader.GetOrdinal("PrezzoAcquisto")) ? null : reader.GetDecimal("PrezzoAcquisto"),
                                    QuantitaVenduta = reader.IsDBNull(reader.GetOrdinal("QuantitàVenduta")) ? null : reader.GetDecimal("QuantitàVenduta"),
                                    DataImpegno = reader.IsDBNull(reader.GetOrdinal("DataImpegno")) ? null : reader.GetDateTime("DataImpegno"),
                                    DataVendita = reader.GetDateTime("DataVendita"),
                                    ClienteImpegno = reader.IsDBNull(reader.GetOrdinal("ClienteImpegno")) ? null : reader.GetString(reader.GetOrdinal("ClienteImpegno")).Trim(),
                                    Certificazione = reader.IsDBNull(reader.GetOrdinal("Certificazione")) ? null : reader.GetString(reader.GetOrdinal("Certificazione")).Trim(),
                                    Qualita = reader.IsDBNull(reader.GetOrdinal("Qualita")) ? null : reader.GetString(reader.GetOrdinal("Qualita")).Trim(),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")).Trim(),
                                    LunghezzaDescr = reader.IsDBNull(reader.GetOrdinal("LunghezzaDescr")) ? null : reader.GetString(reader.GetOrdinal("LunghezzaDescr")).Trim(),
                                    Strati = reader.GetInt16("Strati"),
                                    NumeroCarico = reader.GetInt32("NumeroCarico"),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Giacenze.Clear();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                try
                {
                    if (Giacenze != null)
                    {
                        foreach (AgentiGiacenze item in Giacenze)
                        {
                            AgentiGiacenze agentiGiancenza = new()
                            {
                                Id = item.Id,
                                TipoPacco = item.TipoPacco,
                                Essenza = item.Essenza,
                                Classifica = item.Classifica,
                                StatoLegno = item.StatoLegno,
                                Stagionatura = item.Stagionatura,
                                Deposito = item.Deposito,
                                Provenienza = item.Provenienza,
                                Fornitore = item.Fornitore,
                                UnitaMisuraPrezzoAcquisto = item.UnitaMisuraPrezzoAcquisto,
                                Misura = item.Misura,
                                Dim1 = item.Dim1,
                                Dim2 = item.Dim2,
                                Dim3 = item.Dim3,
                                Quantita = item.Quantita,
                                Pacco = item.Pacco,
                                Tipo = item.Tipo,
                                Volume = item.Volume,
                                NPackList = item.NPackList,
                                Marchio = item.Marchio,
                                PrezzoAcquisto = item.PrezzoAcquisto,
                                QuantitaVenduta = item.QuantitaVenduta,
                                DataImpegno = item.DataImpegno,
                                DataVendita = item.DataVendita,
                                ClienteImpegno = item.ClienteImpegno,
                                Certificazione = item.Certificazione,
                                Qualita = item.Qualita,
                                Note = item.Note,
                                LunghezzaDescr = item.LunghezzaDescr,
                                Strati = item.Strati,
                                NumeroCarico = item.NumeroCarico
                            };
                            EstrazioneGiacenze.Add(agentiGiancenza);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EstrazioneGiacenze.Clear();
                }
            }
            return EstrazioneGiacenze;
        }

        public List<AgentiGiacenze> GetDataRank(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza)
        {
            List<AgentiGiacenze> Giacenze = new();
            List<AgentiGiacenze> EstrazioneGiacenze = new();
            string sqlWhere = string.Empty;

            if (IdEssenza.HasValue || IdClassifica.HasValue || IdStatoLegno.HasValue || IdStagionatura.HasValue || IdDeposito.HasValue || IdProvenienza.HasValue)
            {
                sqlWhere += " WHERE ";
                if (IdEssenza.HasValue)
                {
                    sqlWhere += $"[IdEssenza] = {IdEssenza} AND ";
                }
                if (IdClassifica.HasValue)
                {
                    sqlWhere += $"[IdClassifiche] = {IdClassifica} AND ";
                }
                if (IdStatoLegno.HasValue)
                {
                    sqlWhere += $"[IdStatoLegno] = {IdStatoLegno} AND ";
                }
                if (IdStagionatura.HasValue)
                {
                    sqlWhere += $"[IdStagionatura] = {IdStagionatura} AND ";
                }
                if (IdDeposito.HasValue)
                {
                    sqlWhere += $"[IdDeposito] = {IdDeposito} AND ";
                }
                if (IdProvenienza.HasValue)
                {
                    sqlWhere += $"[IdProvenienza] = {IdProvenienza} AND ";
                }
                sqlWhere = sqlWhere[..^5];
            }

            // Create the connection.
            using (SqlConnection connection = new(_context.Database.GetConnectionString()))
            {
                // Create a SqlCommand
                using (SqlCommand sqlCommand = new
                        ($"SELECT TOP 500 RANK() OVER (ORDER BY TipoPacco, Essenza, Classifica, StatoLegno, Stagionatura, Deposito) AS [Rank], * " +
                        $"FROM [AgentiGiacenza]" +
                        $" {sqlWhere} " +
                        $"ORDER BY TipoPacco, Essenza, Classifica, StatoLegno, Stagionatura, Deposito", connection))
                {
                    // Treat command as a text sql command
                    sqlCommand.CommandType = CommandType.Text;

                    try
                    {
                        // Open connection
                        connection.Open();

                        // Run the sql command
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        // Read records
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Giacenze.Add(new AgentiGiacenze()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Volume = reader.IsDBNull(reader.GetOrdinal("Volume")) ? null : reader.GetDecimal("Volume"),
                                    TipoPacco = reader.IsDBNull(reader.GetOrdinal("TipoPacco")) ? null : reader.GetString(reader.GetOrdinal("TipoPacco")).Trim(),
                                    Essenza = reader.IsDBNull(reader.GetOrdinal("Essenza")) ? null : reader.GetString(reader.GetOrdinal("Essenza")).Trim(),
                                    Classifica = reader.IsDBNull(reader.GetOrdinal("Classifica")) ? null : reader.GetString(reader.GetOrdinal("Classifica")).Trim(),
                                    StatoLegno = reader.IsDBNull(reader.GetOrdinal("StatoLegno")) ? null : reader.GetString(reader.GetOrdinal("StatoLegno")).Trim(),
                                    Stagionatura = reader.IsDBNull(reader.GetOrdinal("Stagionatura")) ? null : reader.GetString(reader.GetOrdinal("Stagionatura")).Trim(),
                                    Deposito = reader.IsDBNull(reader.GetOrdinal("Deposito")) ? null : reader.GetString(reader.GetOrdinal("Deposito")).Trim(),
                                    RankID = reader.GetInt64("Rank"),
                                    Quantita = reader.IsDBNull(reader.GetOrdinal("Quantità")) ? 0 : reader.GetInt16("Quantità"),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Giacenze.Clear();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                try
                {
                    if (Giacenze != null)
                    {
                        //if (id.HasValue)
                        //    Giacenze = Giacenze.Where(r => r.Id == id).ToList();

                        foreach (AgentiGiacenze item in Giacenze)
                        {
                            AgentiGiacenze agentiGiancenza = new()
                            {
                                Id = item.Id,
                                TipoPacco = item.TipoPacco,
                                Essenza = item.Essenza,
                                Classifica = item.Classifica,
                                StatoLegno = item.StatoLegno,
                                Stagionatura = item.Stagionatura,
                                Deposito = item.Deposito,
                                Quantita = item.Quantita,
                                Volume = item.Volume,
                                RankID = item.RankID
                            };
                            EstrazioneGiacenze.Add(agentiGiancenza);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EstrazioneGiacenze.Clear();
                }
            }
            return EstrazioneGiacenze;
        }

        public byte[] Print(List<AgentiGiacenze> giacenze, int? filterTipoStampa, int? filterTipoTavola)
        {
            MemoryStream outputStream = new();

            Reports.StampaTavoleNormailUfficio StampaTavoleNormailUfficioPDF = new();

            Reports.StampaTavoleNormailUfficio.GiacenzeDATA data = new Reports.StampaTavoleNormailUfficio.GiacenzeDATA
            {
                Classifica = giacenze.FirstOrDefault()?.Classifica ?? string.Empty,
                Essenza = giacenze.FirstOrDefault()?.Essenza ?? string.Empty,
                Stagionatura = giacenze.FirstOrDefault()?.Stagionatura ?? string.Empty,
                StatoLegno = giacenze.FirstOrDefault()?.StatoLegno ?? string.Empty
            };

            foreach (AgentiGiacenze item in giacenze)
            {
                Reports.StampaTavoleNormailUfficio.GiacenzeDATA.GiacenzeDATAList giacenzeList = new()
                {
                    Certificazione = item.Certificazione,
                    ClienteImpegno = item.ClienteImpegno,
                    DataImpegno = item.DataImpegno.ToString(),
                    DataVendita = item.DataVendita.ToString(),
                    Deposito = item.Deposito,
                    Dim1 = item.Dim1.ToString(),
                    Dim2 = item.Dim2.ToString(),
                    Dim3 = item.Dim3.ToString(),
                    Fornitore = item.Fornitore,
                    Id = item.Id,
                    LunghezzaDescr = item.LunghezzaDescr,
                    Marchio = item.Marchio,
                    Misura = item.Misura,
                    Note = item.Note,
                    NPackList = item.NPackList.ToString(),
                    NumeroCarico = item.NumeroCarico.ToString(),
                    Pacco = item.Pacco,
                    PrezzoAcquisto = item.PrezzoAcquisto.ToString(),
                    Provenienza = item.Provenienza,
                    Qualita = item.Qualita,
                    Quantita = item.Quantita,
                    QuantitaVenduta = item.QuantitaVenduta.ToString(),
                    RankID = item.RankID,
                    Strati = item.Strati.ToString(),
                    Tipo = item.Tipo.ToString(),
                    TipoPacco = item.TipoPacco,
                    UnitaMisuraPrezzoAcquisto = item.UnitaMisuraPrezzoAcquisto,
                    Volume = item.Volume,
                    Classifica = item.Classifica,
                    Essenza = item.Essenza,
                    Stagionatura = item.Stagionatura,
                    StatoLegno = item.StatoLegno
                };
                data.GiacenzeList.Add(giacenzeList);
            }

            StampaTavoleNormailUfficioPDF.Variables.Add(Reports.StampaTavoleNormailUfficio.DATA, data);
            StampaTavoleNormailUfficioPDF.PrintReport(outputStream);

            return outputStream.ToArray();
        }
    }
}
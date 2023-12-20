using BassoLegnami.Model.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System;
using BassoLegnami.Model.Models.Support;
using System.Data.SqlClient;
using BassoLegnami.Controllers;
using BassoLegnami.Model.Data;
using System.Threading.Tasks;

namespace BassoLegnami.Areas.Support.Controllers
{
    [Area("Support")]
    public class AgentiGiacenzesController : BassoLegnami.Controllers.BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AgentiGiacenzesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: Support/AgentiGiacenzes
        public IActionResult Index()
        {
            List<AgentiGiacenze> Giacenze = new();
            // Create the connection.
            using (SqlConnection connection = new("Server=192.168.0.17;Database=Magazzino;User Id=magazzino;Password=magazzino$;"))
            {
                // Create a SqlCommand
                using (SqlCommand sqlCommand = new
                        ($"SELECT [TipoPacco]" +
                        $", [Essenza] , [Classifica], [StatoLegno], [Stagionatura], [Deposito], [Provenienza], [Fornitore], [UnitaMisuraPrezzoAcquisto], [Misura], [DIM1], [DIM2]" +
                        $", [DIM3], [Quantità], [Pacco], [Tipo], [Volume], [NPackList], [Marchio], [PrezzoAcquisto], [QuantitàVenduta], [DataImpegno], [DataVendita], [ClienteImpegno]," +
                        $" [Certificazione], [Qualita], [Note], [LunghezzaDescr], [Strati], [NumeroCarico]  FROM [Magazzino].[dbo].[AgentiGiacenza]", connection))
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
                                    //Dim1 = reader.IsDBNull(reader.GetOrdinal("DIM1")) ? null : reader.GetString(reader.GetOrdinal("DIM1")).Trim(),
                                    //Dim2 = reader.IsDBNull(reader.GetOrdinal("DIM2")) ? null : reader.GetString(reader.GetOrdinal("DIM2")).Trim(),
                                    //Dim3 = reader.IsDBNull(reader.GetOrdinal("DIM3")) ? null : reader.GetString(reader.GetOrdinal("DIM3")).Trim(),
                                    //Quantita = reader.IsDBNull(reader.GetOrdinal("Quantità")) ? null : reader.GetString(reader.GetOrdinal("Quantità")).Trim(),
                                    //Pacco = reader.IsDBNull(reader.GetOrdinal("Pacco")) ? null : reader.GetString(reader.GetOrdinal("Pacco")).Trim(),
                                    //Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? null : reader.GetString(reader.GetOrdinal("Tipo")).Trim(),
                                    //Volume = reader.IsDBNull(reader.GetOrdinal("Volume")) ? null : reader.GetString(reader.GetOrdinal("Volume")).Trim(),
                                    //NPackList = reader.IsDBNull(reader.GetOrdinal("NPackList")) ? null : reader.GetString(reader.GetOrdinal("NPackList")).Trim(),
                                    Marchio = reader.IsDBNull(reader.GetOrdinal("Marchio")) ? null : reader.GetString(reader.GetOrdinal("Marchio")).Trim(),
                                    //PrezzoAcquisto = reader.IsDBNull(reader.GetOrdinal("PrezzoAcquisto")) ? null : reader.GetString(reader.GetOrdinal("PrezzoAcquisto")).Trim(),
                                    //QuantitaVenduta = reader.IsDBNull(reader.GetOrdinal("QuantitàVenduta")) ? null : reader.GetString(reader.GetOrdinal("QuantitàVenduta")).Trim(),
                                    //DataImpegno = reader.IsDBNull(reader.GetOrdinal("DataImpegno")) ? null : reader.GetString(reader.GetOrdinal("DataImpegno")).Trim(),
                                    //DataVendita = reader.IsDBNull(reader.GetOrdinal("DataVendita")) ? null : reader.GetString(reader.GetOrdinal("DataVendita")).Trim(),
                                    ClienteImpegno = reader.IsDBNull(reader.GetOrdinal("ClienteImpegno")) ? null : reader.GetString(reader.GetOrdinal("ClienteImpegno")).Trim(),
                                    Certificazione = reader.IsDBNull(reader.GetOrdinal("Certificazione")) ? null : reader.GetString(reader.GetOrdinal("Certificazione")).Trim(),
                                    Qualita = reader.IsDBNull(reader.GetOrdinal("Qualita")) ? null : reader.GetString(reader.GetOrdinal("Qualita")).Trim(),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")).Trim(),
                                    LunghezzaDescr = reader.IsDBNull(reader.GetOrdinal("LunghezzaDescr")) ? null : reader.GetString(reader.GetOrdinal("LunghezzaDescr")).Trim(),
                                    //Strati = reader.IsDBNull(reader.GetInt32("Strati")) ? null : reader.GetString(reader.GetInt32("Strati")).Trim(),
                                    //NumeroCarico = reader.IsDBNull(reader.GetInt32("NumeroCarico")) ? null : reader.GetString(reader.GetInt32("NumeroCarico")).Trim(),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO
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
                            Giacenze.Add(agentiGiancenza);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                }
            }
            return View(Giacenze);
        }
    }
}

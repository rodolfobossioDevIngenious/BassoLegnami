using BassoLegnami.Model.Models.Support;
using DocumentFormat.OpenXml.Office2010.Excel;
using In.Core.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BassoLegnami.Model.Data.Repositories
{
    public interface IClientiRepository : IGenericRepository<Clienti>
    {
        List<Clienti> GetData(int? id);
    }

    //public class ClientiRepository : GenericRepository<Clienti>, IClientiRepository
    //{
    //    public ClientiRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
    //    {
    //    }

    //    public List<Clienti> GetData(int? id)
    //    {
    //        List<Clienti> Clienti = new();
    //        List<Clienti> EstrazioneClienti = new();
    //        // Create the connection.
    //        using (SqlConnection connection = new("Server=192.168.0.17;Database=Magazzino;User Id=magazzino;Password=magazzino$;"))
    //        {
    //            // Create a SqlCommand
    //            using (SqlCommand sqlCommand = new
    //                    ($"SELECT TOP 1000 [Id], [Codice],[RagioneSociale],[Indirizzo],[CAP],[Citta],[Provincia],[PartitaIva],[Pagamento]," +
    //                    $"[Incarico1],[Telefono1],[Telefono2],[Fax],[Cellulare],[Email],[id_Agente],[Categoria] FROM [dbo].[Clienti] ORDER BY [Id] DESC", connection))
    //            {
    //                // Treat command as a text sql command
    //                sqlCommand.CommandType = CommandType.Text;

    //                try
    //                {
    //                    // Open connection
    //                    connection.Open();

    //                    // Run the sql command
    //                    SqlDataReader reader = sqlCommand.ExecuteReader();

    //                    // Read records
    //                    if (reader.HasRows)
    //                    {
    //                        while (reader.Read())
    //                        {
    //                            Clienti.Add(new Clienti()
    //                            {
    //                                Id = reader.GetInt32("Id"),
    //                                Codice = reader.IsDBNull(reader.GetOrdinal("Codice")) ? null : reader.GetString(reader.GetOrdinal("Codice")).Trim(),
    //                                RagioneSociale = reader.IsDBNull(reader.GetOrdinal("RagioneSociale")) ? null : reader.GetString(reader.GetOrdinal("RagioneSociale")).Trim(),
    //                                Indirizzo = reader.IsDBNull(reader.GetOrdinal("Indirizzo")) ? null : reader.GetString(reader.GetOrdinal("Indirizzo")).Trim(),
    //                                CAP = reader.IsDBNull(reader.GetOrdinal("CAP")) ? null : reader.GetString(reader.GetOrdinal("CAP")).Trim(),
    //                                Citta = reader.IsDBNull(reader.GetOrdinal("Citta")) ? null : reader.GetString(reader.GetOrdinal("Citta")).Trim(),
    //                                Provincia = reader.IsDBNull(reader.GetOrdinal("Provincia")) ? null : reader.GetString(reader.GetOrdinal("Provincia")).Trim(),
    //                                PartitaIva = reader.IsDBNull(reader.GetOrdinal("PartitaIva")) ? null : reader.GetString(reader.GetOrdinal("PartitaIva")).Trim(),
    //                                Pagamento = reader.IsDBNull(reader.GetOrdinal("Pagamento")) ? null : reader.GetString(reader.GetOrdinal("Pagamento")).Trim(),
    //                                Incarico1 = reader.IsDBNull(reader.GetOrdinal("Incarico1")) ? null : reader.GetString(reader.GetOrdinal("Incarico1")).Trim(),
    //                                Telefono1 = reader.IsDBNull(reader.GetOrdinal("Telefono1")) ? null : reader.GetString(reader.GetOrdinal("Telefono1")).Trim(),
    //                                Telefono2 = reader.IsDBNull(reader.GetOrdinal("Telefono2")) ? null : reader.GetString(reader.GetOrdinal("Telefono2")).Trim(),
    //                                Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax")).Trim(),
    //                                Cellulare = reader.IsDBNull(reader.GetOrdinal("Cellulare")) ? null : reader.GetString(reader.GetOrdinal("Cellulare")).Trim(),
    //                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")).Trim(),
    //                                id_Agente = reader.GetInt32("id_Agente"),
    //                                Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? null : reader.GetString(reader.GetOrdinal("Categoria")).Trim(),
    //                            });
    //                        }
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    EstrazioneClienti.Clear();
    //                }
    //                finally
    //                {
    //                    connection.Close();
    //                }
    //            }

    //            try
    //            {
    //                if (Clienti != null)
    //                {
    //                    if (id.HasValue)
    //                        Clienti = Clienti.Where(r => r.Id == id).ToList();

    //                    foreach (Clienti item in Clienti)
    //                    {
    //                        Clienti clienti = new()
    //                        {
    //                            Id = item.Id,
    //                            RagioneSociale = item.RagioneSociale,
    //                            PartitaIva = item.PartitaIva,
    //                            CAP = item.CAP,
    //                            Categoria = item.Categoria,
    //                            Cellulare = item.Cellulare,
    //                            Citta = item.Citta,
    //                            Codice = item.Codice,
    //                            Email = item.Email,
    //                            Fax = item.Fax,
    //                            id_Agente = item.id_Agente,
    //                            Incarico1 = item.Incarico1,
    //                            Indirizzo = item.Indirizzo,
    //                            Pagamento = item.Pagamento,
    //                            Provincia = item.Provincia,
    //                            Telefono1 = item.Telefono1,
    //                            Telefono2 = item.Telefono2,
    //                        };
    //                        EstrazioneClienti.Add(clienti);
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                EstrazioneClienti.Clear();
    //            }
    //        }
    //        return EstrazioneClienti;
    //    }
    //}
}
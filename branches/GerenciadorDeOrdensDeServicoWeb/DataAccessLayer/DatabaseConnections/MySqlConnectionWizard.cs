using System.Configuration;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections {
	public class MySqlConnectionWizard {
		public static MySqlConnection getConnection() {

			string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			MySqlConnection conn = new MySqlConnection( connStr );

			return conn;
		}
	}
}
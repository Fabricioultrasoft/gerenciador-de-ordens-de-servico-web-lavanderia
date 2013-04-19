using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql {
	public class MySqlFilter {

		private String _whereClause;
		private List<MySqlParameter> _parametersList;

		public MySqlFilter() {
			_whereClause = String.Empty;
			_parametersList = new List<MySqlParameter>();
		}

		public String whereClause {
			get { return _whereClause; }
			set { _whereClause = value; }
		}
		public List<MySqlParameter> parametersList {
			get { return _parametersList; }
			set { _parametersList = value; }
		}
	}
}
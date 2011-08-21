using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql {
	public class Filter {
		private String _property;
		private String _value;

		public Filter() {
			_property = String.Empty;
			_value = String.Empty;
		}

		public String property {
			get { return _property; }
			set { _property = value; }
		}

		public String value {
			get { return _value; }
			set { _value = value; }
		}
	}
}
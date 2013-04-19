using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql {
	public class Sorter {
		private String _property;
		private String _direction;

		public Sorter() {
			_property = String.Empty;
			_direction = String.Empty;
		}

		public String property {
			get { return _property; }
			set { _property = value; }
		}

		public String direction {
			get { return _direction; }
			set { _direction = value; }
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico {
	public class Status {

		private UInt32 _codigo;
		private String _nome;

		public Status() {
			_codigo = 0;
			_nome = String.Empty;
		}

		public Status(UInt32 codigo, String nome) {
			_codigo = codigo;
			_nome = nome;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}
	}
}
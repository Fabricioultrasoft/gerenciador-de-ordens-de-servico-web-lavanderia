using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Cidade {
		private UInt32 _codigo;
		private String _nome;

		private Estado _estado;

		public Cidade() {
			_codigo = 0;
			_nome = String.Empty;

			_estado = new Estado();
		}

		public Cidade( UInt32 codCidade, String nomCidade ) {
			codigo = codCidade;
			nome = nomCidade;

			_estado = new Estado();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public Estado estado {
			get { return _estado; }
			set { _estado = value; }
		}
	}
}
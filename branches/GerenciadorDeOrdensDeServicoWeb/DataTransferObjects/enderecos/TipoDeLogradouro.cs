using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class TipoDeLogradouro {

		private UInt32 _codigo;
		private String _nome;

		public TipoDeLogradouro() {
			_codigo = 0;
			_nome = String.Empty;
		}

		public TipoDeLogradouro( UInt32 codTipoDeLogradouro, String nomTipoDeLogradouro ) {
			codigo = codTipoDeLogradouro;
			nome = nomTipoDeLogradouro;
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
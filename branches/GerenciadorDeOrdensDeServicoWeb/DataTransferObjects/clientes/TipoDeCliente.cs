using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes {
	public class TipoDeCliente {

		private UInt32 _codigo;
		private String _nome;
		private bool _ativo;

		public TipoDeCliente() {
			_codigo = 0;
			_nome = String.Empty;
			_ativo = true;
		}

		public TipoDeCliente( UInt32 codigo, String nome, bool ativo ) {
			_codigo = codigo;
			_nome = nome;
			_ativo = ativo;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}
		public bool ativo {
			get { return _ativo; }
			set { _ativo = value; }
		}
	}
}
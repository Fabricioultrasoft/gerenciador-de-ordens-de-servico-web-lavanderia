using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes {
	public class TipoDeContato {

		private UInt32 _codigo;
		private String _nome;

		public TipoDeContato() {
			this.codigo = 0;
			this.nome = String.Empty;
		}

		public TipoDeContato(UInt32 codigo, String nome) {
			this.codigo = codigo;
			this.nome = nome;
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
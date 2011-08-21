using System;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Pais {

		private UInt32 _codigo;
		private String _nome;

		public Pais() {
			_codigo = 0;
			_nome = String.Empty;
		}

		public Pais(UInt32 codPais, String nomPais) {
			codigo = codPais;
			nome = nomPais;
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
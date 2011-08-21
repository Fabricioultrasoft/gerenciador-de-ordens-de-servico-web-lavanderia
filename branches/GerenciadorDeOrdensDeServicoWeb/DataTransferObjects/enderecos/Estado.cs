using System;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Estado {
		private UInt32 _codigo;
		private String _nome;

		private Pais _pais;

		public Estado() {
			_codigo = 0;
			_nome = String.Empty;

			_pais = new Pais();
		}

		public Estado( UInt32 codEstado, String nomEstado ) {
			codigo = codEstado;
			nome = nomEstado;

			_pais = new Pais();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public Pais pais {
			get { return _pais; }
			set { _pais = value; }
		}
	}
}